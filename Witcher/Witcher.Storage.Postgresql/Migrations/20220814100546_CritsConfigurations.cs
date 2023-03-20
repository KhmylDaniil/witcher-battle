using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class CritsConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatureParameters_Creatures_CreatureId",
                schema: "Battles",
                table: "CreatureParameters");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureParameters_Skills_SkillId",
                schema: "Battles",
                table: "CreatureParameters");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplateParameters_CreatureTemplates_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateParameters");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplateParameters_Skills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatureTemplateParameters",
                schema: "GameRules",
                table: "CreatureTemplateParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatureParameters",
                schema: "Battles",
                table: "CreatureParameters");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleWingCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleTorso1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleTailCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleLegCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleHead2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleHead1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "SimpleArmCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultTorso1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultTailCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultHead2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultHead1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DifficultArmCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyWingCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyTorso1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyTailCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyLegCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyHead2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyHead1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyArmCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexWingCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexTorso1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexTailCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexLegCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexHead2CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexHead1CritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Effects",
                table: "ComplexArmCritEffects");

            migrationBuilder.RenameTable(
                name: "CreatureTemplateParameters",
                schema: "GameRules",
                newName: "CreatureTemplateSkills",
                newSchema: "GameRules");

            migrationBuilder.RenameTable(
                name: "CreatureParameters",
                schema: "Battles",
                newName: "CreatureSkills",
                newSchema: "Battles");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureTemplateParameters_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                newName: "IX_CreatureTemplateSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureTemplateParameters_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                newName: "IX_CreatureTemplateSkills_CreatureId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureParameters_SkillId",
                schema: "Battles",
                table: "CreatureSkills",
                newName: "IX_CreatureSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureParameters_CreatureId",
                schema: "Battles",
                table: "CreatureSkills",
                newName: "IX_CreatureSkills_CreatureId");

            migrationBuilder.AlterTable(
                name: "CreatureTemplateSkills",
                schema: "GameRules",
                comment: "Навыки шаблона существа",
                oldComment: "Параметры шаблона существа");

            migrationBuilder.AlterTable(
                name: "CreatureSkills",
                schema: "Battles",
                comment: "Навыки существа",
                oldComment: "Параметры существа");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Skills",
                type: "text",
                nullable: false,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleWingCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleTailCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleLegCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleArmCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "Counter",
                schema: "Effects",
                table: "DyingEffects",
                type: "integer",
                nullable: false,
                comment: "Модификатор сложности",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatAthleticsModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatDodgeModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatSpeedModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики");

            migrationBuilder.AddColumn<int>(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения");

            migrationBuilder.AddColumn<int>(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatAthleticsModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatDodgeModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики");

            migrationBuilder.AddColumn<int>(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatAthleticsModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatDodgeModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatSpeedModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики");

            migrationBuilder.AddColumn<int>(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения");

            migrationBuilder.AddColumn<int>(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости");

            migrationBuilder.AlterColumn<int>(
                name: "RoundCounter",
                schema: "Effects",
                table: "DifficultHead1CritEffects",
                type: "integer",
                nullable: false,
                comment: "Счетчик раундов",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NextCheck",
                schema: "Effects",
                table: "DifficultHead1CritEffects",
                type: "integer",
                nullable: false,
                comment: "Раунд следующей проверки",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultArmCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyWingCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DeadlyWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики");

            migrationBuilder.AddColumn<int>(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DeadlyWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения");

            migrationBuilder.AddColumn<int>(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DeadlyWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatBodyModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор телосложения после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatSpeedModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatStaModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор стамины после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "BodyModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор телосложения");

            migrationBuilder.AddColumn<int>(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости");

            migrationBuilder.AddColumn<int>(
                name: "StaModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор стамины");

            migrationBuilder.AddColumn<int>(
                name: "AfterTreatStaModifier",
                schema: "Effects",
                table: "DeadlyTorso1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор стамины после стабилизации");

            migrationBuilder.AddColumn<int>(
                name: "StaModifier",
                schema: "Effects",
                table: "DeadlyTorso1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор стамины");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyTailCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DeadlyTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики");

            migrationBuilder.AddColumn<int>(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DeadlyTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyLegCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DeadlyLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атлетики");

            migrationBuilder.AddColumn<int>(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DeadlyLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор уклонения");

            migrationBuilder.AddColumn<int>(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DeadlyLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор скорости");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyArmCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Battles",
                table: "CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Тяжесть критического эффекта");

            migrationBuilder.AlterColumn<int>(
                name: "Stun",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                comment: "Устойчивость",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MaxWill",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                comment: "Максимальная воля",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Максимальнпя воля");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexWingCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexTailCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexLegCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexArmCritEffects",
                type: "boolean",
                nullable: false,
                comment: "Пенальти применено",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                type: "text",
                nullable: false,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "Battles",
                table: "CreatureSkills",
                type: "text",
                nullable: false,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatureTemplateSkills",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatureSkills",
                schema: "Battles",
                table: "CreatureSkills",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("32ee830e-7bee-4924-9ddf-1070ceffecdd"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("4fcbd3d6-fde0-47c1-899d-a8c82c291751"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("754ef5e9-8960-4c38-a1be-a3c43c92b1cd"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f00eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f01eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f03eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f04eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f05eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f06eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f07eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f08eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f09eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f10eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f11eea-10d5-428e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f12eea-10d5-428e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f13eea-10d5-726e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Dex");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f14eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Dex");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f15eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f16eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f17eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f18eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f19eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f20eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f21eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f22eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f23eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f24eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f25eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f26eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f27eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f28eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f29eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f30eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f31eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f32eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f33eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f34eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Will");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Emp");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Dex");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Int");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Cra");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Body");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Ref");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Dex");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Dex");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Dex");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"),
                column: "StatName",
                value: "Body");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureSkills_Creatures_CreatureId",
                schema: "Battles",
                table: "CreatureSkills",
                column: "CreatureId",
                principalSchema: "Battles",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureSkills_Skills_SkillId",
                schema: "Battles",
                table: "CreatureSkills",
                column: "SkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplateSkills_CreatureTemplates_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                column: "CreatureId",
                principalSchema: "GameRules",
                principalTable: "CreatureTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplateSkills_Skills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                column: "SkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatureSkills_Creatures_CreatureId",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureSkills_Skills_SkillId",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplateSkills_CreatureTemplates_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplateSkills_Skills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatureTemplateSkills",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatureSkills",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropColumn(
                name: "AfterTreatAthleticsModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatDodgeModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatSpeedModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DifficultWingCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatAthleticsModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatDodgeModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects");

            migrationBuilder.DropColumn(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects");

            migrationBuilder.DropColumn(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DifficultTailCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatAthleticsModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatDodgeModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatSpeedModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DifficultLegCritEffects");

            migrationBuilder.DropColumn(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DeadlyWingCritEffects");

            migrationBuilder.DropColumn(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DeadlyWingCritEffects");

            migrationBuilder.DropColumn(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DeadlyWingCritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatBodyModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatSpeedModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatStaModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "BodyModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "StaModifier",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects");

            migrationBuilder.DropColumn(
                name: "AfterTreatStaModifier",
                schema: "Effects",
                table: "DeadlyTorso1CritEffects");

            migrationBuilder.DropColumn(
                name: "StaModifier",
                schema: "Effects",
                table: "DeadlyTorso1CritEffects");

            migrationBuilder.DropColumn(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DeadlyTailCritEffects");

            migrationBuilder.DropColumn(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DeadlyTailCritEffects");

            migrationBuilder.DropColumn(
                name: "AthleticsModifier",
                schema: "Effects",
                table: "DeadlyLegCritEffects");

            migrationBuilder.DropColumn(
                name: "DodgeModifier",
                schema: "Effects",
                table: "DeadlyLegCritEffects");

            migrationBuilder.DropColumn(
                name: "SpeedModifier",
                schema: "Effects",
                table: "DeadlyLegCritEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Battles",
                table: "CritEffects");

            migrationBuilder.RenameTable(
                name: "CreatureTemplateSkills",
                schema: "GameRules",
                newName: "CreatureTemplateParameters",
                newSchema: "GameRules");

            migrationBuilder.RenameTable(
                name: "CreatureSkills",
                schema: "Battles",
                newName: "CreatureParameters",
                newSchema: "Battles");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureTemplateSkills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                newName: "IX_CreatureTemplateParameters_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureTemplateSkills_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                newName: "IX_CreatureTemplateParameters_CreatureId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureSkills_SkillId",
                schema: "Battles",
                table: "CreatureParameters",
                newName: "IX_CreatureParameters_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_CreatureSkills_CreatureId",
                schema: "Battles",
                table: "CreatureParameters",
                newName: "IX_CreatureParameters_CreatureId");

            migrationBuilder.AlterTable(
                name: "CreatureTemplateParameters",
                schema: "GameRules",
                comment: "Параметры шаблона существа",
                oldComment: "Навыки шаблона существа");

            migrationBuilder.AlterTable(
                name: "CreatureParameters",
                schema: "Battles",
                comment: "Параметры существа",
                oldComment: "Навыки существа");

            migrationBuilder.AlterColumn<int>(
                name: "StatName",
                schema: "GameRules",
                table: "Skills",
                type: "integer",
                nullable: false,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleWingCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleTorso1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleTailCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleLegCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleHead2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleHead1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "SimpleArmCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "SimpleArmCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Counter",
                schema: "Effects",
                table: "DyingEffects",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Модификатор сложности");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultTorso1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultHead2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RoundCounter",
                schema: "Effects",
                table: "DifficultHead1CritEffects",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Счетчик раундов");

            migrationBuilder.AlterColumn<int>(
                name: "NextCheck",
                schema: "Effects",
                table: "DifficultHead1CritEffects",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Раунд следующей проверки");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultHead1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DifficultArmCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DifficultArmCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyWingCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyTorso1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyTailCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyLegCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyHead2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyHead1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "DeadlyArmCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "DeadlyArmCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Stun",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Устойчивость");

            migrationBuilder.AlterColumn<int>(
                name: "MaxWill",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                comment: "Максимальнпя воля",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Максимальная воля");

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexWingCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexWingCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexTorso2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexTorso1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexTailCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexTailCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexLegCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexLegCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexHead2CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexHead1CritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "PenaltyApplied",
                schema: "Effects",
                table: "ComplexArmCritEffects",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Пенальти применено");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Effects",
                table: "ComplexArmCritEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StatName",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                type: "integer",
                nullable: false,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<int>(
                name: "StatName",
                schema: "Battles",
                table: "CreatureParameters",
                type: "integer",
                nullable: false,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatureTemplateParameters",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatureParameters",
                schema: "Battles",
                table: "CreatureParameters",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("32ee830e-7bee-4924-9ddf-1070ceffecdd"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("4fcbd3d6-fde0-47c1-899d-a8c82c291751"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("754ef5e9-8960-4c38-a1be-a3c43c92b1cd"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f00eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f01eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f03eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f04eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f05eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f06eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f07eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f08eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f09eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f10eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f11eea-10d5-428e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f12eea-10d5-428e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f13eea-10d5-726e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f14eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f15eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f16eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f17eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f18eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f19eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f20eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f21eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f22eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f23eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f24eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f25eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f26eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f27eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f28eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f29eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f30eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f31eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f32eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f33eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f34eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"),
                column: "StatName",
                value: 4);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureParameters_Creatures_CreatureId",
                schema: "Battles",
                table: "CreatureParameters",
                column: "CreatureId",
                principalSchema: "Battles",
                principalTable: "Creatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureParameters_Skills_SkillId",
                schema: "Battles",
                table: "CreatureParameters",
                column: "SkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplateParameters_CreatureTemplates_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                column: "CreatureId",
                principalSchema: "GameRules",
                principalTable: "CreatureTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplateParameters_Skills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                column: "SkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
