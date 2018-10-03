namespace GarbageDb {
    using Microsoft.EntityFrameworkCore;

    public class BloggingContext : DbContext {
        public BloggingContext() { }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options) { }

        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

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
        }
    }
}
