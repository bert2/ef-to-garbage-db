namespace GarbageDb {
    using Microsoft.EntityFrameworkCore;

    public class BloggingContext : DbContext {
        public BloggingContext() { }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<NegativeReview> NegativeReviews { get; set; }
        public DbSet<PositiveReview> PositiveReviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlite("Data Source=blogging.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<Blog>(x => x.ToTable("M_Blogs"));
            mb.Entity<Blog>(x => x.Property(b => b.Id).HasColumnName("Pid"));

            mb.Entity<Post>(x => x.ToTable("M_Posts"));
            mb.Entity<Post>(x => x.Property(p => p.Id).HasColumnName("Pid"));
            mb.Entity<Post>(x => x.Property(p => p.BlogId).HasColumnName("BlogXid"));
            mb.Entity<Post>(x => x
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogId));

            mb.Entity<Comment>(x => x.ToTable("M_Comments"));
            mb.Entity<Comment>(x => x.Property(c => c.Id).HasColumnName("Pid"));
            mb.Entity<Comment>(x => x.Property(c => c.PostId).HasColumnName("PostXid"));
            mb.Entity<Comment>(x => x
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId));

            mb.Entity<Review>().ToTable("M_Reviews");
            mb.Entity<Review>().Property(r => r.Id).HasColumnName("Pid");
            mb.Entity<Review>().Property(r => r.PostId).HasColumnName("PostXid");
            mb.Entity<Review>()
                .HasDiscriminator<char>("Type")
                .HasValue<PositiveReview>('P')
                .HasValue<NegativeReview>('N');
            mb.Entity<Review>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PostId);
        }
    }
}
