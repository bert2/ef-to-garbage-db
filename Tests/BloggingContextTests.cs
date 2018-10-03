namespace Tests {
    using System;
    using System.Linq;

    using GarbageDb;

    using Shouldly;

    using Xunit;

    public class BloggingContextTests : IClassFixture<BloggingDbSeed> {
        [Fact]
        public void LoadsBlogs() => BloggingContext(db => {
            var blogs = db.Blogs.ToArray();
            blogs.Length.ShouldBe(2);
        });

        [Fact]
        public void LoadsPosts() => BloggingContext(db => {
            var posts = db.Posts.ToArray();
            posts.Length.ShouldBe(3);
        });

        [Fact]
        public void LoadsPostsOfFilteredBlog() => BloggingContext(db => {
            var posts = db.Blogs.Where(b => b.Id == 2).Select(b => b.Posts).ToArray();
            posts.Length.ShouldBe(1);
        });

        private static void BloggingContext(Action<BloggingContext> action) {
            using (var db = new BloggingContext()) {
                action(db);
            }
        }
    }
}
