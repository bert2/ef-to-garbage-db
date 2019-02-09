namespace Tests {
    using System.Data.SQLite;

    public static class BloggingDbSeed {
        public static void Reset() {
            using (var db = new SQLiteConnection("Data Source=blogging.db")) {
                db.Open();
                SeedSchema(db);
                SeedData(db);
            }
        }

        private static void SeedSchema(SQLiteConnection db) => 
            new SQLiteCommand(@"
                drop table if exists M_Blogs;
                create table M_Blogs (
                    Pid      integer primary key,
                    Url      text not null
                );
                
                drop table if exists M_Posts;
                create table M_Posts (
                    Pid      integer primary key,
                    BlogXid  integer not null,
                    Title    text,
                    Content  text
                );

                drop table if exists M_Comments;
                create table M_Comments (
                    Pid      integer primary key,
                    PostXid  integer not null,
                    Message  text
                );

                drop table if exists M_Reviews;
                create table M_Reviews (
                    Pid      integer primary key,
                    PostXid  integer not null,
                    Type     text not null,
                    TextXid  integer not null
                );

                drop table if exists M_CritiqueTexts;
                create table M_CritiqueTexts (
                    Pid      integer primary key,
                    MeanText text not null
                );

                drop table if exists M_PraiseTexts;
                create table M_PraiseTexts (
                    Pid      integer primary key,
                    NiceText text not null
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
                    (4, 2, 'No more space!', 'Too many developers sent to our facility lately.');
                
                insert into M_Comments (Pid, PostXid, Message) values
                    (1, 2, 'I WANT MY MONEY BACK!'),
                    (2, 2, 'You will hear from our lawyers...');
                
                insert into M_Reviews (Pid, PostXid, Type, TextXid) values
                    (1, 2, 'P', 1),
                    (2, 2, 'N', 1),
                    (3, 2, 'N', 2);
                
                insert into M_CritiqueTexts (Pid, MeanText) values
                    (1, 'Yuk!'),
                    (2, 'Ugh...');
                
                insert into M_PraiseTexts (Pid, NiceText) values
                    (1, 'I was not bribed into doing this!')",
                db)
                .ExecuteNonQuery();
    }
}
