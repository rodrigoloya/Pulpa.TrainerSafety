using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulpa.TrainerSafety.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorizacion_esto_estamal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId1",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId2",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResults_AspNetUsers_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResults_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_OwnerId1",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignTargets_AspNetUsers_TargetUserId1",
                schema: "TrainerSafety",
                table: "CampaignTargets");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignTargets_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignTargets");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyGroupId",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyGroupId1",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyGroupId2",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyGroups",
                schema: "TrainerSafety",
                table: "FamilyGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignTargets",
                schema: "TrainerSafety",
                table: "CampaignTargets");

            migrationBuilder.DropIndex(
                name: "IX_CampaignTargets_TargetUserId1",
                schema: "TrainerSafety",
                table: "CampaignTargets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_OwnerId1",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignResults",
                schema: "TrainerSafety",
                table: "CampaignResults");

            migrationBuilder.DropColumn(
                name: "EnableNotifications",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyGroupId",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyGroupId1",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyGroupId2",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UsuarioExternalId",
                schema: "TrainerSafety",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TargetUserId1",
                schema: "TrainerSafety",
                table: "CampaignTargets");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                schema: "TrainerSafety",
                table: "Campaigns");

            migrationBuilder.RenameTable(
                name: "FamilyGroups",
                schema: "TrainerSafety",
                newName: "FamilyGroup",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameTable(
                name: "CampaignTargets",
                schema: "TrainerSafety",
                newName: "CampaignTarget",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameTable(
                name: "Campaigns",
                schema: "TrainerSafety",
                newName: "Campaign",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameTable(
                name: "CampaignResults",
                schema: "TrainerSafety",
                newName: "CampaignResult",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameColumn(
                name: "TargetUserId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                newName: "TargetUserUsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignTargets_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                newName: "IX_CampaignTarget_CampaignId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "OwnerUsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaign",
                newName: "IX_Campaign_TemplatePhishingTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignResults_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResult",
                newName: "IX_CampaignResult_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignResults_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResult",
                newName: "IX_CampaignResult_CampaignId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 1, 6, 54, 12, 445, DateTimeKind.Utc).AddTicks(7895),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResult",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyGroup",
                schema: "TrainerSafety",
                table: "FamilyGroup",
                column: "FamilyGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignTarget",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                column: "CampaignTargetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaign",
                schema: "TrainerSafety",
                table: "Campaign",
                column: "CampaignId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignResult",
                schema: "TrainerSafety",
                table: "CampaignResult",
                column: "CampaignResultId");

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "TrainerSafety",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionType = table.Column<int>(type: "int", nullable: false),
                    FamilyGroupId1 = table.Column<int>(type: "int", nullable: true),
                    EnableNotifications = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FamilyGroupId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserUpdated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_FamilyGroup_FamilyGroupId",
                        column: x => x.FamilyGroupId,
                        principalSchema: "TrainerSafety",
                        principalTable: "FamilyGroup",
                        principalColumn: "FamilyGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_FamilyGroup_FamilyGroupId1",
                        column: x => x.FamilyGroupId1,
                        principalSchema: "TrainerSafety",
                        principalTable: "FamilyGroup",
                        principalColumn: "FamilyGroupId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyGroup_OwnerId",
                schema: "TrainerSafety",
                table: "FamilyGroup",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTarget_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                column: "TargetUserUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaign",
                column: "OwnerUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_FamilyGroupId",
                schema: "TrainerSafety",
                table: "Usuario",
                column: "FamilyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_FamilyGroupId1",
                schema: "TrainerSafety",
                table: "Usuario",
                column: "FamilyGroupId1");

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
                name: "FK_CampaignResult_Campaign_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResult",
                column: "CampaignId",
                principalSchema: "TrainerSafety",
                principalTable: "Campaign",
                principalColumn: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignResult_Usuario_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResult",
                column: "UsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignTarget_Campaign_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                column: "CampaignId",
                principalSchema: "TrainerSafety",
                principalTable: "Campaign",
                principalColumn: "CampaignId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignTarget_Usuario_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget",
                column: "TargetUserUsuarioId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyGroup_Usuario_OwnerId",
                schema: "TrainerSafety",
                table: "FamilyGroup",
                column: "OwnerId",
                principalSchema: "TrainerSafety",
                principalTable: "Usuario",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_CampaignResult_Campaign_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignResult_Usuario_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResult");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignTarget_Campaign_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignTarget");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignTarget_Usuario_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyGroup_Usuario_OwnerId",
                schema: "TrainerSafety",
                table: "FamilyGroup");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "TrainerSafety");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyGroup",
                schema: "TrainerSafety",
                table: "FamilyGroup");

            migrationBuilder.DropIndex(
                name: "IX_FamilyGroup_OwnerId",
                schema: "TrainerSafety",
                table: "FamilyGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignTarget",
                schema: "TrainerSafety",
                table: "CampaignTarget");

            migrationBuilder.DropIndex(
                name: "IX_CampaignTarget_TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTarget");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignResult",
                schema: "TrainerSafety",
                table: "CampaignResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaign",
                schema: "TrainerSafety",
                table: "Campaign");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaign");

            migrationBuilder.RenameTable(
                name: "FamilyGroup",
                schema: "TrainerSafety",
                newName: "FamilyGroups",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameTable(
                name: "CampaignTarget",
                schema: "TrainerSafety",
                newName: "CampaignTargets",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameTable(
                name: "CampaignResult",
                schema: "TrainerSafety",
                newName: "CampaignResults",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameTable(
                name: "Campaign",
                schema: "TrainerSafety",
                newName: "Campaigns",
                newSchema: "TrainerSafety");

            migrationBuilder.RenameColumn(
                name: "TargetUserUsuarioId",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                newName: "TargetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignTarget_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                newName: "IX_CampaignTargets_CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignResult_UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                newName: "IX_CampaignResults_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignResult_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                newName: "IX_CampaignResults_CampaignId");

            migrationBuilder.RenameColumn(
                name: "OwnerUsuarioId",
                schema: "TrainerSafety",
                table: "Campaigns",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns",
                newName: "IX_Campaigns_TemplatePhishingTemplateId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 1, 6, 54, 12, 445, DateTimeKind.Utc).AddTicks(7895));

            migrationBuilder.AddColumn<bool>(
                name: "EnableNotifications",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyGroupId",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyGroupId1",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyGroupId2",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionType",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioExternalId",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetUserId1",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                schema: "TrainerSafety",
                table: "CampaignResults",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId1",
                schema: "TrainerSafety",
                table: "Campaigns",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyGroups",
                schema: "TrainerSafety",
                table: "FamilyGroups",
                column: "FamilyGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignTargets",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                column: "CampaignTargetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignResults",
                schema: "TrainerSafety",
                table: "CampaignResults",
                column: "CampaignResultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns",
                schema: "TrainerSafety",
                table: "Campaigns",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyGroupId",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                column: "FamilyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyGroupId1",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                column: "FamilyGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyGroupId2",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                column: "FamilyGroupId2",
                unique: true,
                filter: "[FamilyGroupId2] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignTargets_TargetUserId1",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                column: "TargetUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_OwnerId1",
                schema: "TrainerSafety",
                table: "Campaigns",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                column: "FamilyGroupId",
                principalSchema: "TrainerSafety",
                principalTable: "FamilyGroups",
                principalColumn: "FamilyGroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId1",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                column: "FamilyGroupId1",
                principalSchema: "TrainerSafety",
                principalTable: "FamilyGroups",
                principalColumn: "FamilyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId2",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                column: "FamilyGroupId2",
                principalSchema: "TrainerSafety",
                principalTable: "FamilyGroups",
                principalColumn: "FamilyGroupId");

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
                name: "FK_Campaigns_AspNetUsers_OwnerId1",
                schema: "TrainerSafety",
                table: "Campaigns",
                column: "OwnerId1",
                principalSchema: "TrainerSafety",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_PhishingTemplate_TemplatePhishingTemplateId",
                schema: "TrainerSafety",
                table: "Campaigns",
                column: "TemplatePhishingTemplateId",
                principalSchema: "TrainerSafety",
                principalTable: "PhishingTemplate",
                principalColumn: "PhishingTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignTargets_AspNetUsers_TargetUserId1",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                column: "TargetUserId1",
                principalSchema: "TrainerSafety",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignTargets_Campaigns_CampaignId",
                schema: "TrainerSafety",
                table: "CampaignTargets",
                column: "CampaignId",
                principalSchema: "TrainerSafety",
                principalTable: "Campaigns",
                principalColumn: "CampaignId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
