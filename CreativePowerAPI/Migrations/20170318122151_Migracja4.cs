using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CreativePowerAPI.Migrations
{
    public partial class Migracja4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_ProjectTask_ProjectTaskId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_Raport_ProjectTask_ProjectTaskId",
                table: "Raport");

            migrationBuilder.DropIndex(
                name: "IX_File_ProjectTaskId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "File");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskId",
                table: "Raport",
                newName: "PriceListId");

            migrationBuilder.RenameIndex(
                name: "IX_Raport_ProjectTaskId",
                table: "Raport",
                newName: "IX_Raport_PriceListId");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "AspNetUsers",
                newName: "Affiliation");

            migrationBuilder.AddColumn<int>(
                name: "BoxNumber",
                table: "Raport",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwitcherNumber",
                table: "Raport",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "ProjectTask",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RaportId",
                table: "ProjectTask",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateTime",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PriceListId",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonId",
                table: "Investors",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskPosition", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_PositionId",
                table: "ProjectTask",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_RaportId",
                table: "ProjectTask",
                column: "RaportId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_PriceListId",
                table: "Project",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_Investors_ContactPersonId",
                table: "Investors",
                column: "ContactPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investors_AspNetUsers_ContactPersonId",
                table: "Investors",
                column: "ContactPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_PriceList_PriceListId",
                table: "Project",
                column: "PriceListId",
                principalTable: "PriceList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_TaskPosition_PositionId",
                table: "ProjectTask",
                column: "PositionId",
                principalTable: "TaskPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Raport_RaportId",
                table: "ProjectTask",
                column: "RaportId",
                principalTable: "Raport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Raport_PriceList_PriceListId",
                table: "Raport",
                column: "PriceListId",
                principalTable: "PriceList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investors_AspNetUsers_ContactPersonId",
                table: "Investors");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_PriceList_PriceListId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_TaskPosition_PositionId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Raport_RaportId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Raport_PriceList_PriceListId",
                table: "Raport");

            migrationBuilder.DropTable(
                name: "TaskPosition");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_PositionId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_RaportId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_Project_PriceListId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Investors_ContactPersonId",
                table: "Investors");

            migrationBuilder.DropColumn(
                name: "BoxNumber",
                table: "Raport");

            migrationBuilder.DropColumn(
                name: "SwitcherNumber",
                table: "Raport");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "RaportId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "CreateDateTime",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PriceListId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ContactPersonId",
                table: "Investors");

            migrationBuilder.RenameColumn(
                name: "PriceListId",
                table: "Raport",
                newName: "ProjectTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Raport_PriceListId",
                table: "Raport",
                newName: "IX_Raport_ProjectTaskId");

            migrationBuilder.RenameColumn(
                name: "Affiliation",
                table: "AspNetUsers",
                newName: "Position");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_ProjectTaskId",
                table: "File",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_ProjectTask_ProjectTaskId",
                table: "File",
                column: "ProjectTaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Raport_ProjectTask_ProjectTaskId",
                table: "Raport",
                column: "ProjectTaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
