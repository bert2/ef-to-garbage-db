namespace GarbageDb {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("M_CritiqueTexts")]
    public class CritiqueText {
        [Key]
        [Column("Pid")]
        public int Id { get; set; }

        public string MeanText { get; set; }

        [InverseProperty(nameof(NegativeReview.CritiqueText))]
        public NegativeReview Review { get; set; }
    }
}
