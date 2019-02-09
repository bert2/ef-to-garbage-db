namespace GarbageDb {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("M_Reviews")]
    public abstract class Review {
        [Key]
        [Column("Pid")]
        public int Id { get; set; }

        [Column("PostXid")]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        public string Type { get; set; }

        [Column("TextXid")]
        public int TextId { get; set; }
    }

    public class NegativeReview : Review {
        [ForeignKey(nameof(TextId))]
        public CritiqueText CritiqueText { get; set; }
    }

    public class PositiveReview : Review {
        [ForeignKey(nameof(TextId))]
        public PraiseText PraiseText { get; set; }
    }
}
