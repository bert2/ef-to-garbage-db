namespace Tests {
    using System;
    using System.Data.SQLite;

    public class BloggingDbSeed : IDisposable {
        public BloggingDbSeed() {
            using (var db = new SQLiteConnection("Data Source=blogging.db")) {
                db.Open();
                SeedSchema(db);
                SeedData(db);
            }
        }

        public void Dispose() { }

        private static void SeedSchema(SQLiteConnection db) => 
            new SQLiteCommand(@"
                drop table if exists M_Blogs;
                create table M_Blogs (
                    Pid     integer primary key,
                    Url     text not null
                );
                
                drop table if exists M_Posts;
                create table M_Posts (
                    Pid     integer primary key,
                    BlogXid integer not null,
                    Title   text,
                    Content text
                );",
                db)
                .ExecuteNonQuery();

        private static void SeedData(SQLiteConnection db) => 
            new SQLiteCommand(@"
                insert into M_Blogs (Pid, Url) values
                    (1, 'http://www.illusions-online.com/'),
                    (2, 'https://azhd.ae/services-2/psychiatry/');
                
                insert into M_Posts (Pid, BlogXid, Title, Content) values
                    (1, 1, 'Foo', 'Bar'),
                    (2, 1, 'We turn YOUR money into shit!', 'Like... literally.'),
                    (3, 2, 'Title...', 'Content...'),
                    (4, 2, 'No more space!', 'Too many developers sent to our facility lately.');",
                db)
                .ExecuteNonQuery();
    }
}
