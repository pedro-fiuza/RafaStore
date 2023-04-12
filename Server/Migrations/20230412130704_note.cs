using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RafaStore.Server.Migrations
{
    public partial class note : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteFileModel_Customer_CustomerModelId",
                table: "NoteFileModel");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NoteFileModel",
                type: "smalldatetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "NoteFileModel",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NoteFileModel",
                type: "smalldatetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Blob",
                table: "NoteFileModel",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "NumeroDeParcelas",
                table: "NoteFileModel",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorParcela",
                table: "NoteFileModel",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "NoteFileModel",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File",
                table: "NoteFileModel",
                column: "CustomerModelId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_File",
                table: "NoteFileModel");

            migrationBuilder.DropColumn(
                name: "NumeroDeParcelas",
                table: "NoteFileModel");

            migrationBuilder.DropColumn(
                name: "ValorParcela",
                table: "NoteFileModel");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "NoteFileModel");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "NoteFileModel",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "NoteFileModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "NoteFileModel",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime");

            migrationBuilder.AlterColumn<string>(
                name: "Blob",
                table: "NoteFileModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteFileModel_Customer_CustomerModelId",
                table: "NoteFileModel",
                column: "CustomerModelId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
