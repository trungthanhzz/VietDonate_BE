using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietDonate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Additionpropertyintouserinformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "UserInformations",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankBranch",
                table: "UserInformations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "UserInformations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampaignCount",
                table: "UserInformations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "UserInformations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "UserInformations",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationLegalRepresentative",
                table: "UserInformations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "UserInformations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationRegisterNumber",
                table: "UserInformations",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationTaxCode",
                table: "UserInformations",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffNumber",
                table: "UserInformations",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UserInformations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDonated",
                table: "UserInformations",
                type: "numeric(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRecieved",
                table: "UserInformations",
                type: "numeric(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "UserInformations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationStatus",
                table: "UserInformations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "BankBranch",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "CampaignCount",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "OrganizationLegalRepresentative",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "OrganizationRegisterNumber",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "OrganizationTaxCode",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "StaffNumber",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "TotalDonated",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "TotalRecieved",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "VerificationStatus",
                table: "UserInformations");
        }
    }
}
