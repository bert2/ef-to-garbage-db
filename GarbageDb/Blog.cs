namespace GarbageDb {
    using System.Collections.Generic;

    public class Blog {
        public int Id { get; set; }

        public string Url { get; set; }

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
