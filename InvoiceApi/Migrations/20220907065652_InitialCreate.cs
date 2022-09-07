using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    detail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    detail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstitutionModelId = table.Column<int>(type: "int", nullable: false),
                    UserModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceModels_InstitutionModels_InstitutionModelId",
                        column: x => x.InstitutionModelId,
                        principalTable: "InstitutionModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceModels_UserModels_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "UserModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceModels_InstitutionModelId",
                table: "InvoiceModels",
                column: "InstitutionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceModels_UserModelId",
                table: "InvoiceModels",
                column: "UserModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceModels");

            migrationBuilder.DropTable(
                name: "InstitutionModels");

            migrationBuilder.DropTable(
                name: "UserModels");
        }
    }
}
