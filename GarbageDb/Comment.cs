namespace GarbageDb {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("M_Comments")]
    public class Comment {
        [Key]
        [Column("Pid")]
        public int Id { get; set; }

        public string Message { get; set; }

        [Column("PostXid")]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}
