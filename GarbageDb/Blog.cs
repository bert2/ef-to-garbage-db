namespace GarbageDb {
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("M_Blogs")]
    public class Blog {
        [Key]
        [Column("Pid")]
        public int Id { get; set; }

        public string Url { get; set; }

        [InverseProperty(nameof(Post.Blog))]
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
