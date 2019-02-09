namespace GarbageDb {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("M_PraiseTexts")]
    public class PraiseText {
        [Key]
        [Column("Pid")]
        public int Id { get; set; }

        public string NiceText { get; set; }

        [InverseProperty(nameof(PositiveReview.PraiseText))]
        public PositiveReview Review { get; set; }
    }
}
