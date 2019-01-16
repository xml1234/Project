using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LTMCompanyName.YoyoCmsTemplate.Migrations
{
    public partial class Add_Edtion_Extension_Entity_And_IdentityServer4_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "ABP",
                table: "UserTokens",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                schema: "ABP",
                table: "UserTokens",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "ABP",
                table: "Users",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                schema: "ABP",
                table: "Users",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "ABP",
                table: "UserAccounts",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomCssId",
                schema: "ABP",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInTrialPeriod",
                schema: "ABP",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoFileType",
                schema: "ABP",
                table: "Tenants",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LogoId",
                schema: "ABP",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "ABP",
                table: "Editions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AnnualPrice",
                schema: "ABP",
                table: "Editions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpiringEditionId",
                schema: "ABP",
                table: "Editions",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPrice",
                schema: "ABP",
                table: "Editions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrialDayCount",
                schema: "ABP",
                table: "Editions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitingDayAfterExpire",
                schema: "ABP",
                table: "Editions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                schema: "ABP",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayments",
                schema: "ABP",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Gateway = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    EditionId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    DayCount = table.Column<int>(nullable: false),
                    PaymentPeriodType = table.Column<int>(nullable: true),
                    PaymentId = table.Column<string>(nullable: true),
                    InvoiceNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayments_Editions_EditionId",
                        column: x => x.EditionId,
                        principalSchema: "ABP",
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreationTime",
                schema: "ABP",
                table: "Tenants",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_SubscriptionEndUtc",
                schema: "ABP",
                table: "Tenants",
                column: "SubscriptionEndUtc");

            migrationBuilder.CreateIndex(
                name: "IX_DataFileObjects_TenantId",
                table: "DataFileObjects",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                schema: "ABP",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayments_EditionId",
                schema: "ABP",
                table: "SubscriptionPayments",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayments_PaymentId_Gateway",
                schema: "ABP",
                table: "SubscriptionPayments",
                columns: new[] { "PaymentId", "Gateway" });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayments_Status_CreationTime",
                schema: "ABP",
                table: "SubscriptionPayments",
                columns: new[] { "Status", "CreationTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedGrants",
                schema: "ABP");

            migrationBuilder.DropTable(
                name: "SubscriptionPayments",
                schema: "ABP");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_CreationTime",
                schema: "ABP",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_SubscriptionEndUtc",
                schema: "ABP",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_DataFileObjects_TenantId",
                table: "DataFileObjects");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                schema: "ABP",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "CustomCssId",
                schema: "ABP",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsInTrialPeriod",
                schema: "ABP",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "LogoFileType",
                schema: "ABP",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "LogoId",
                schema: "ABP",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "ABP",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "AnnualPrice",
                schema: "ABP",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "ExpiringEditionId",
                schema: "ABP",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                schema: "ABP",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "TrialDayCount",
                schema: "ABP",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "WaitingDayAfterExpire",
                schema: "ABP",
                table: "Editions");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "ABP",
                table: "UserTokens",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "ABP",
                table: "Users",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                schema: "ABP",
                table: "Users",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "ABP",
                table: "UserAccounts",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
