using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHE.InfraData.Migrations
{
    /// <inheritdoc />
    public partial class Create_Table_GHE_And_Training : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ghe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Matricule = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    GHE = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Unhealthiness = table.Column<bool>(type: "INTEGER", nullable: false),
                    Dangerousness = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotApplicable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ghe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrainingName = table.Column<string>(type: "TEXT", nullable: true),
                    TrainingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TrainingDateFinal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ASO = table.Column<string>(type: "TEXT", nullable: true),
                    GheId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_Ghe_GheId",
                        column: x => x.GheId,
                        principalTable: "Ghe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Training_GheId",
                table: "Training",
                column: "GheId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "Ghe");
        }
    }
}
