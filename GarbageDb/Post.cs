namespace GarbageDb {
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("M_Posts")]
    public class Post {
        [Key]
        [Column("Pid")]
        public int Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        [Column("BlogXid")]
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }

        [InverseProperty(nameof(Comment.Post))]
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        [InverseProperty(nameof(PositiveReview.Post))]
        public ICollection<PositiveReview> PositiveReviews { get; set; } = new HashSet<PositiveReview>();

        [InverseProperty(nameof(NegativeReview.Post))]
        public ICollection<NegativeReview> NegativeReviews { get; set; } = new HashSet<NegativeReview>();
    }
}
