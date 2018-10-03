namespace GarbageDb
{
    using System.Collections.Generic;

    public class Blog
    {
        public int BlogId { get; set; }

        public string Url { get; set; }

        public virtual ICollection<Post> Post { get; set; } = new HashSet<Post>();
    }
}
