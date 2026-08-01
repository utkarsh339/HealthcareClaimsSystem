using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareClaims.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientResponsibilityAmount",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "Claims",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProcedureCode",
                table: "Claims",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DiagnosisCode",
                table: "Claims",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<decimal>(
                name: "BilledAmount",
                table: "Claims",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "PriorAuthorizationId",
                table: "Claims",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "Claims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                table: "Claims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ClaimNumber",
                table: "Claims",
                column: "ClaimNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PriorAuthorizationId",
                table: "Claims",
                column: "PriorAuthorizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_PriorAuthorizations_PriorAuthorizationId",
                table: "Claims",
                column: "PriorAuthorizationId",
                principalTable: "PriorAuthorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_PriorAuthorizations_PriorAuthorizationId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ClaimNumber",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_PriorAuthorizationId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "BilledAmount",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "PriorAuthorizationId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Claims",
                newName: "SubmittedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Claims",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProcedureCode",
                table: "Claims",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "DiagnosisCode",
                table: "Claims",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<decimal>(
                name: "PatientResponsibilityAmount",
                table: "Claims",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Claims",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
