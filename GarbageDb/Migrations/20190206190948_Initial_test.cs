using Microsoft.EntityFrameworkCore.Migrations;

namespace GarbageDb.Migrations
{
    public partial class Initial_test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "M_Blogs",
                columns: table => new
                {
                    Pid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Blogs", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "M_Posts",
                columns: table => new
                {
                    Pid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    BlogXid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Posts", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_M_Posts_M_Blogs_BlogXid",
                        column: x => x.BlogXid,
                        principalTable: "M_Blogs",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_Comments",
                columns: table => new
                {
                    Pid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(nullable: true),
                    PostXid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Comments", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_M_Comments_M_Posts_PostXid",
                        column: x => x.PostXid,
                        principalTable: "M_Posts",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_Reviews",
                columns: table => new
                {
                    Pid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<char>(nullable: false),
                    PostXid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Reviews", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_M_Reviews_M_Posts_PostXid",
                        column: x => x.PostXid,
                        principalTable: "M_Posts",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_M_Comments_PostXid",
                table: "M_Comments",
                column: "PostXid");

            migrationBuilder.CreateIndex(
                name: "IX_M_Posts_BlogXid",
                table: "M_Posts",
                column: "BlogXid");

            migrationBuilder.CreateIndex(
                name: "IX_M_Reviews_PostXid",
                table: "M_Reviews",
                column: "PostXid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "M_Comments");

            migrationBuilder.DropTable(
                name: "M_Reviews");

            migrationBuilder.DropTable(
                name: "M_Posts");

            migrationBuilder.DropTable(
                name: "M_Blogs");
        }
    }
}
