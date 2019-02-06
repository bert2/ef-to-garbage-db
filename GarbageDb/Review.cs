namespace GarbageDb {
    public abstract class Review {
        public int Id { get; set; }
        public char Type { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }

    public class NegativeReview : Review {
        //public Critique Critique { get; set; }
    }

    public class PositiveReview : Review {
        //public Praise Praise { get; set; }
    }

    public class Critique {
        public int Id { get; set; }
        public string MeanText { get; set; }
        public int NegativeReviewId { get; set; }
        public NegativeReview NegativeReview { get; set; }
    }

    public class Praise {
        public int Id { get; set; }
        public string NiceText { get; set; }
        public int PositiveReviewId { get; set; }
        public PositiveReview PositiveReview { get; set; }
    }
}
