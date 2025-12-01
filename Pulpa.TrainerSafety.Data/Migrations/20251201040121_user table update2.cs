using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulpa.TrainerSafety.Data.Migrations
{
    /// <inheritdoc />
    public partial class usertableupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResults_AspNetUsers_UsuarioId1",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResults_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_PhishingTemplate_AspNetUsers_CreatedById",
                schema: "TrainerSafety",
                table: "PhishingTemplate");

            migrationBuilder.DropIndex(
                name: "IX_PhishingTemplate_CreatedById",
                schema: "TrainerSafety",
                table: "PhishingTemplate");

            migrationBuilder.DropIndex(
                name: "IX_CampaignResults_UsuarioId1",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "TrainerSafety",
                table: "PhishingTemplate");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "TrainerSafety",
                table: "EducationalContent");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SmsContent",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LandingPageUrl",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "BodyContent",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "TrainerSafety",
                table: "EducationalContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                schema: "TrainerSafety",
                table: "EducationalContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TargetUserId",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "SmsSent",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailSent",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                schema: "TrainerSafety",
                table: "Campaigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailOpenedAt",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignResults_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignResults_AspNetUsers_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "UsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignResults_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "CampaignId",
                principalSchema: "TrainerSafety",
                principalTable: "Campaigns",
                principalColumn: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns",
                column: "TemplatePhishingTemplateId",
                principalSchema: "TrainerSafety",
                principalTable: "PhishingTemplate",
                principalColumn: "PhishingTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResults_AspNetUsers_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResults_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_CampaignResults_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SmsContent",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LandingPageUrl",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BodyContent",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "TrainerSafety",
                table: "EducationalContent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                schema: "TrainerSafety",
                table: "EducationalContent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "TrainerSafety",
                table: "EducationalContent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "TargetUserId",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "SmsSent",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailSent",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                schema: "TrainerSafety",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailOpenedAt",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhishingTemplate_CreatedById",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignResults_UsuarioId1",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignResults_AspNetUsers_UsuarioId1",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "UsuarioId1",
                principalSchema: "TrainerSafety",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignResults_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "CampaignId",
                principalSchema: "TrainerSafety",
                principalTable: "Campaigns",
                principalColumn: "CampaignId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns",
                column: "TemplatePhishingTemplateId",
                principalSchema: "TrainerSafety",
                principalTable: "PhishingTemplate",
                principalColumn: "PhishingTemplateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhishingTemplate_AspNetUsers_CreatedById",
                schema: "TrainerSafety",
                table: "PhishingTemplate",
                column: "CreatedById",
                principalSchema: "TrainerSafety",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
