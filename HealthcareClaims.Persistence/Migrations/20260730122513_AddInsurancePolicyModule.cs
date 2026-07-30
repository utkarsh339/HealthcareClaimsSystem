using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareClaims.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInsurancePolicyModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsurancePolicies_Patients_PatientId",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "CoveragePercentage",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "DeductibleAmount",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "OutOfPocketMaximum",
                table: "InsurancePolicies");

            migrationBuilder.AlterColumn<string>(
                name: "PolicyNumber",
                table: "InsurancePolicies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PlanName",
                table: "InsurancePolicies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "InsurancePolicies",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceCompany",
                table: "InsurancePolicies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "InsurancePolicies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GroupNumber",
                table: "InsurancePolicies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "InsurancePolicies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_PolicyNumber",
                table: "InsurancePolicies",
                column: "PolicyNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InsurancePolicies_Patients_PatientId",
                table: "InsurancePolicies",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsurancePolicies_Patients_PatientId",
                table: "InsurancePolicies");

            migrationBuilder.DropIndex(
                name: "IX_InsurancePolicies_PolicyNumber",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "GroupNumber",
                table: "InsurancePolicies");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "InsurancePolicies");

            migrationBuilder.AlterColumn<string>(
                name: "PolicyNumber",
                table: "InsurancePolicies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PlanName",
                table: "InsurancePolicies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "InsurancePolicies",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceCompany",
                table: "InsurancePolicies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<decimal>(
                name: "CoveragePercentage",
                table: "InsurancePolicies",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeductibleAmount",
                table: "InsurancePolicies",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OutOfPocketMaximum",
                table: "InsurancePolicies",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_InsurancePolicies_Patients_PatientId",
                table: "InsurancePolicies",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
