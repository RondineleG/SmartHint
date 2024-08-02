using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHint.Persistence.Migrations;

/// <inheritdoc />
public partial class Start : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Compradores",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                NomeRazaoSocial = table.Column<string>(type: "TEXT", nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                Telefone = table.Column<string>(type: "TEXT", nullable: false),
                DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                Bloqueado = table.Column<bool>(type: "INTEGER", nullable: false),
                TipoPessoa = table.Column<byte>(type: "INTEGER", nullable: false),
                CpfCnpj = table.Column<string>(type: "TEXT", nullable: false),
                InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: false),
                InscricaoEstadualIsenta = table.Column<bool>(type: "INTEGER", nullable: false),
                Genero = table.Column<byte>(type: "INTEGER", nullable: true),
                DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                Senha = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Compradores", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Compradores_CpfCnpj",
            table: "Compradores",
            column: "CpfCnpj",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Compradores_Email",
            table: "Compradores",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Compradores_InscricaoEstadual",
            table: "Compradores",
            column: "InscricaoEstadual",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Compradores");
    }
}
