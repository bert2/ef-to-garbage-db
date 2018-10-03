namespace Tests {
    using System;
    using System.Linq;

    using GarbageDb;

    using Microsoft.EntityFrameworkCore;

    using Shouldly;

    using Xunit;

    public class BloggingContextTests {
        [Fact]
        public void LoadsBlogs() => BloggingContext(db => 
            db.Blogs.Count().ShouldBe(2));

        [Fact]
        public void LoadsPosts() => BloggingContext(db => 
            db.Posts.Count().ShouldBe(4));

        [Fact]
        public void LoadsPostsOfFilteredBlog() => BloggingContext(db => {
            var blog = db.Blogs.Single(b => b.Id == 2);
            var posts = db.Blogs.Where(b => b.Id == 2).Select(b => b.Posts).ToArray();
            var blogCount = blog.Posts.Count;
            var count = posts.Count();
            var length = posts.Length;
            posts.Count().ShouldBe(2);
        });

        [Fact]
        public void LoadsIncludedPosts() => BloggingContext(db => {
            var blogs = db.Blogs.Include(b => b.Posts).ToArray();
            blogs.SelectMany(b => b.Posts).Count().ShouldBe(4);
        });

        [Fact]
        public void AddsPostToBlog() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).Single(b => b.Id == 2);
            var newPost = new Post {Blog = blog, Title = "NEW!", Content = "...and fresh."};

            blog.Posts.Add(newPost);
            db.SaveChanges();

            Should.NotThrow(() => db.Posts.Single(p => p.Title == "NEW!"));
        });

        [Fact]
        public void UpdatesPostOfBlog() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).Single(b => b.Id == 2);
            var post = blog.Posts.Single(p => p.Title == "Title...");

            post.Title = "Super Title!";
            db.SaveChanges();

            db.Posts.Any(p => p.Title == "Super Title!").ShouldBe(true);
        });

        [Fact]
        public void DeletesPostFromBlog() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).Single(b => b.Id == 2);
            var post = blog.Posts.Single(p => p.Title == "No more space!");

            blog.Posts.Remove(post);
            db.SaveChanges();

            db.Posts.Any(p => p.Id == post.Id).ShouldBe(false);
        });

        [Fact]
        public void DeletesPostsWhenRemovingBlogFromDbSet() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).Single(b => b.Id == 1);

            db.Blogs.Remove(blog);
            db.SaveChanges();

            db.Posts.Any(p => p.BlogId == 1).ShouldBe(false);
        });

        [Fact]
        public void DeletesCommentsWhenRemovingPostFromBlog() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).ThenInclude(p => p.Comments).Single(b => b.Id == 1);
            var post = blog.Posts.Single(p => p.Title == "We turn YOUR money into shit!");

            blog.Posts.Remove(post);
            db.SaveChanges();

            db.Comments.Any(c => c.PostId == post.Id).ShouldBe(false);
        });

        [Fact]
        public void CascadesDeletes() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).ThenInclude(p => p.Comments).Single(b => b.Id == 1);

            db.Blogs.Remove(blog);
            db.SaveChanges();

            db.Comments.Any(c => blog.Posts.Contains(c.Post)).ShouldBe(false);
        });

        private static void BloggingContext(Action<BloggingContext> action) {
            BloggingDbSeed.Reset();
            using (var db = new BloggingContext()) {
                action(db);
            }
        }
    }
}
