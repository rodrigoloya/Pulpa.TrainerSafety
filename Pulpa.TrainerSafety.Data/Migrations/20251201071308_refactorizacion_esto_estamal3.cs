using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulpa.TrainerSafety.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorizacion_esto_estamal3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Usuario_OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignTarget_Usuario_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget");

            migrationBuilder.RenameColumn(
                name: "TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignTarget_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                newName: "IX_CampaignTarget_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "PhishingTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "IX_Campaign_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "IX_Campaign_PhishingTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_PhishingTemplate_PhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                column: "PhishingTemplateId",
                principalSchema: "TrainerSafety",
                principalTable: "PhishingTemplate",
                principalColumn: "PhishingTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Usuario_UsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                column: "UsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignTarget_Usuario_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                column: "UsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_PhishingTemplate_PhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Usuario_UsuarioId",
                schema: "TrainerSafety",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignTarget_Usuario_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                newName: "TargetUserUsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignTarget_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                newName: "IX_CampaignTarget_TargetUserUsuarioId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "TemplatePhishingTemplateId");

            migrationBuilder.RenameColumn(
                name: "PhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "OwnerUsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_UsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "IX_Campaign_TemplatePhishingTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_PhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "IX_Campaign_OwnerUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                column: "TemplatePhishingTemplateId",
                principalSchema: "TrainerSafety",
                principalTable: "PhishingTemplate",
                principalColumn: "PhishingTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Usuario_OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                column: "OwnerUsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignTarget_Usuario_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                column: "TargetUserUsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");
        }
    }
}
