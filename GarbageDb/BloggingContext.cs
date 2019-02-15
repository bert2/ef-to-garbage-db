namespace GarbageDb {
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class BloggingContext : DbContext {
        public BloggingContext() { }

        public BloggingContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CritiqueText> CritiqueTexts { get; set; }
        public DbSet<PraiseText> PraiseTexts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlite("Data Source=blogging.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<Review>()
                .HasDiscriminator<string>("Type")
                .HasValue<PositiveReview>("P")
                .HasValue<NegativeReview>("N");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            // Note that this is internal code to force cascade deletes to happen.
            // It may stop working in any future release.
            ChangeTracker.DetectChanges();
            this.GetService<IStateManager>().GetEntriesToSave();

            try {
                ChangeTracker.AutoDetectChangesEnabled = false;

                foreach (var entry in ChangeTracker
                    .Entries<CritiqueText>()
                    .Where(e => Entry(e.Entity.Review).State == EntityState.Deleted)) {
                    entry.State = EntityState.Deleted;
                }

                foreach (var entry in ChangeTracker
                    .Entries<PraiseText>()
                    .Where(e => Entry(e.Entity.Review).State == EntityState.Deleted)) {
                    entry.State = EntityState.Deleted;
                }
            } finally {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
