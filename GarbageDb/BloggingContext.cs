namespace GarbageDb {
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class BloggingContext : DbContext {
        private static readonly ILookup<Type, PropertyInfo> InvertedOneToOneRelations = Assembly
            .GetAssembly(typeof(BloggingContext))
            .GetTypes()
            .SelectMany(t => t.GetCustomAttributes<ForceCascadeDeleteAttribute>(), (t, a) => (type: t, attr: a))
            .Select(x => x.type.GetProperty(x.attr.Name))
            .ToLookup(p => p.DeclaringType);

        public BloggingContext() { }

        public BloggingContext(DbContextOptions options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CritiqueText> CritiqueTexts { get; set; }
        public DbSet<PraiseText> PraiseTexts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=blogging.db");
        }

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<Review>()
                .HasDiscriminator<string>("Type")
                .HasValue<PositiveReview>("P")
                .HasValue<NegativeReview>("N");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            ForceCascadeDeleteOnInvertedOneToOneRelations();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        // Note that this is internal code to force cascade deletes to happen.
        // It may stop working in any future release.
        private void ForceCascadeDeleteOnInvertedOneToOneRelations() {
            ChangeTracker.DetectChanges();
            this.GetService<IStateManager>().GetEntriesToSave();

            try {
                ChangeTracker.AutoDetectChangesEnabled = false;
                ChangeTracker
                    .Entries()
                    .Where(e => e.State == EntityState.Deleted)
                    .Select(e => e.Entity)
                    .SelectMany(e => InvertedOneToOneRelations[e.GetType()], (e, p) => (entity: e, prop: p))
                    .Select(x => x.prop.GetValue(x.entity))
                    .ForEach(e => Entry(e).State = EntityState.Deleted);
            } finally {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
