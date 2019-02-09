namespace Tests {
    using System;
    using System.Linq;

    using GarbageDb;

    using Microsoft.EntityFrameworkCore;

    using Shouldly;

    using Xunit;

    public class BloggingContextTests {
        [Fact]
        public void LoadsBlogs() => BloggingContext(db => db
            .Blogs.Count().ShouldBe(2));

        [Fact]
        public void LoadsPosts() => BloggingContext(db => db
            .Posts.Count().ShouldBe(4));

        [Fact]
        public void LoadsPostsOfFilteredBlog() => BloggingContext(db => db
            .Blogs
            .Where(b => b.Id == 2)
            .SelectMany(b => b.Posts)
            .Count().ShouldBe(2));

        [Fact]
        public void AutoIncludesSelectedPosts() => BloggingContext(db => db
            .Blogs
            .SelectMany(b => b.Posts)
            .Count().ShouldBe(4));

        [Fact]
        public void AddsPostToBlog() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).Single(b => b.Id == 2);
            var newPost = new Post {Blog = blog, Title = "NEW!", Content = "...and fresh."};

            blog.Posts.Add(newPost);
            db.SaveChanges();

            db.Posts.Single(p => p.Title == "NEW!").Content.ShouldBe("...and fresh.");
        });

        [Fact]
        public void UpdatesPostOfBlog() => BloggingContext(db => {
            var blog = db.Blogs.Include(b => b.Posts).Single(b => b.Id == 2);
            var post = blog.Posts.Single(p => p.Title == "Title...");

            post.Content = "Fresh content!";
            db.SaveChanges();

            db.Posts.Single(p => p.Title == "Title...").Content.ShouldBe("Fresh content!");
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

        [Fact]
        public void CanDiscriminateReviews() => BloggingContext(db => {
            db.Reviews.OfType<PositiveReview>().Count().ShouldBe(1);
            db.Reviews.OfType<NegativeReview>().Count().ShouldBe(2);
        });

        [Fact]
        public void CanLoadTextsOfDiscriminatedReviews() => BloggingContext(db => {
            var post = db.Posts
                .Include(p => p.NegativeReviews).ThenInclude(r => r.CritiqueText)
                .Include(p => p.PositiveReviews).ThenInclude(r => r.PraiseText)
                .Single(p => p.Id == 2);

            post.NegativeReviews.Count(r => r.CritiqueText?.MeanText != null).ShouldBe(2);
            post.PositiveReviews.Single().PraiseText?.NiceText.ShouldNotBeNull();
        });

        [Fact]
        public void DeletesTextWhenDeletingReview() => BloggingContext(db => {
            var post = db.Posts.Include(p => p.NegativeReviews).Single(p => p.Id == 2);

            post.NegativeReviews.Clear();
            db.SaveChanges();

            db.CritiqueTexts.Any().ShouldBeFalse();
        });

        private static void BloggingContext(Action<BloggingContext> action) {
            BloggingDbSeed.Reset();
            using (var db = new BloggingContext()) {
                action(db);
            }
        }
    }
}
