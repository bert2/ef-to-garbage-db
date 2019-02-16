namespace GarbageDb {
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class BloggingContext : DbContext {
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

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder
            .Entity<Review>(x => x
                .HasDiscriminator<string>("Type")
                .HasValue<PositiveReview>("P")
                .HasValue<NegativeReview>("N"));

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            CascadeDeleteDependantsOfInvertedOneToOneRelations();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private static readonly ILookup<Type, PropertyInfo> DependantsToDelete = Assembly
            .GetAssembly(typeof(BloggingContext))
            .GetTypes()
            .SelectMany(
                t => t.GetCustomAttributes<ForceCascadeDeleteAttribute>(),
                (t, a) => (principal: t, dependant: a.Name))
            .Select(x => x.principal.GetProperty(x.dependant))
            .Tap(p => Debug.Assert(
                p.PropertyType.GetInterface(nameof(IEnumerable)) == null,
                $"Property {p.DeclaringType.Name}.{p.Name} targeted by {nameof(ForceCascadeDeleteAttribute)} has to be a one-to-one relation."))
            .ToLookup(p => p.DeclaringType);

        // Note that this is internal code to force cascade deletes to happen.
        // It may stop working in any future release.
        private void CascadeDeleteDependantsOfInvertedOneToOneRelations() {
            ChangeTracker.DetectChanges();
            this.GetService<IStateManager>().GetEntriesToSave();

            try {
                ChangeTracker.AutoDetectChangesEnabled = false;
                ChangeTracker
                    .Entries()
                    .Where(e => e.State == EntityState.Deleted)
                    .Select(e => e.Entity)
                    .SelectMany(e => DependantsToDelete[e.GetType()], (e, p) => (entity: e, prop: p))
                    .Select(x => x.prop.GetValue(x.entity))
                    .ForEach(e => Entry(e).State = EntityState.Deleted);
            } finally {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
