using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_Analytics.Service.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Owners = table.Column<string>(nullable: true),
                    Editors = table.Column<string>(nullable: true),
                    Viewers = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ValidOperation = table.Column<bool>(nullable: false),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeModels_ProjectModels_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComponentModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ValidOperation = table.Column<bool>(nullable: false),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentModels_ProjectModels_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CapabilityModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ValidOperation = table.Column<bool>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    AttributeId = table.Column<int>(nullable: true),
                    ComponentId = table.Column<int>(nullable: true),
                    FrequencyOfFailure = table.Column<int>(nullable: false),
                    Impact = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapabilityModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapabilityModels_AttributeModels_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "AttributeModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CapabilityModels_ComponentModels_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "ComponentModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CapabilityModels_ProjectModels_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeModels_ProjectId",
                table: "AttributeModels",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityModels_AttributeId",
                table: "CapabilityModels",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityModels_ComponentId",
                table: "CapabilityModels",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_CapabilityModels_ProjectId",
                table: "CapabilityModels",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentModels_ProjectId",
                table: "ComponentModels",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CapabilityModels");

            migrationBuilder.DropTable(
                name: "AttributeModels");

            migrationBuilder.DropTable(
                name: "ComponentModels");

            migrationBuilder.DropTable(
                name: "ProjectModels");
        }
    }
}
