namespace GarbageDb {
    using Microsoft.EntityFrameworkCore;

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
    }
}
