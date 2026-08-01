using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareClaims.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorAuthorizationModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizationNumber",
                table: "PriorAuthorizations");

            migrationBuilder.RenameColumn(
                name: "ReviewerComments",
                table: "PriorAuthorizations",
                newName: "ReviewNotes");

            migrationBuilder.RenameColumn(
                name: "ReviewedAt",
                table: "PriorAuthorizations",
                newName: "DecisionDate");

            migrationBuilder.RenameColumn(
                name: "RequestedProcedureCode",
                table: "PriorAuthorizations",
                newName: "ProcedureCode");

            migrationBuilder.RenameColumn(
                name: "RequestedAt",
                table: "PriorAuthorizations",
                newName: "RequestedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PriorAuthorizations",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PriorAuthorizations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ServiceDescription",
                table: "PriorAuthorizations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PriorAuthorizations_Status",
                table: "PriorAuthorizations",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriorAuthorizations_Status",
                table: "PriorAuthorizations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PriorAuthorizations");

            migrationBuilder.DropColumn(
                name: "ServiceDescription",
                table: "PriorAuthorizations");

            migrationBuilder.RenameColumn(
                name: "ReviewNotes",
                table: "PriorAuthorizations",
                newName: "ReviewerComments");

            migrationBuilder.RenameColumn(
                name: "RequestedDate",
                table: "PriorAuthorizations",
                newName: "RequestedAt");

            migrationBuilder.RenameColumn(
                name: "ProcedureCode",
                table: "PriorAuthorizations",
                newName: "RequestedProcedureCode");

            migrationBuilder.RenameColumn(
                name: "DecisionDate",
                table: "PriorAuthorizations",
                newName: "ReviewedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PriorAuthorizations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AddColumn<string>(
                name: "AuthorizationNumber",
                table: "PriorAuthorizations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
