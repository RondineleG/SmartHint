namespace SmartHint.Application.Dtos.Comprador.Response;

/// <summary>
/// DTO que representa os dados de resposta de um comprador.
/// </summary>
public class CompradorResponseDto
{
    /// <summary>
    /// Construtor padr�o do DTO CompradorResponseDto.
    /// </summary>
    public CompradorResponseDto()
    {
        NomeRazaoSocial = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        DataCadastro = DateTime.Now;
        Bloqueado = false;
        TipoPessoa = string.Empty;
        CpfCnpj = string.Empty;
        InscricaoEstadual = string.Empty;
        InscricaoEstadualIsenta = false;
        Genero = string.Empty;
        DataNascimento = DateTime.MinValue;
        Senha = string.Empty;
    }



    /// <summary>
    /// Id do comprador.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do comprador ou Raz�o Social.
    /// </summary>
    /// <example>Jo�o da Silva</example>
    public string NomeRazaoSocial { get; set; }

    /// <summary>
    /// E-mail do comprador.
    /// </summary>
    /// <example>joao.silva@example.com</example>
    public string Email { get; set; }

    /// <summary>
    /// Telefone do comprador.
    /// </summary>
    /// <example>11999999999</example>
    public string Telefone { get; set; }

    /// <summary>
    /// Data de cadastro do comprador.
    /// </summary>
    /// <example>2023-07-29</example>
    public DateTime DataCadastro { get; set; }

    /// <summary>
    /// Indica se o comprador est� bloqueado.
    /// </summary>
    /// <example>false</example>
    public bool Bloqueado { get; set; }

    /// <summary>
    /// Tipo de pessoa do comprador (F�sica ou Jur�dica).
    /// </summary>
    /// <example>Fisica</example>
    public string TipoPessoa { get; set; }

    /// <summary>
    /// CPF ou CNPJ do comprador.
    /// </summary>
    /// <example>123.456.789-00</example>
    public string CpfCnpj { get; set; }

    /// <summary>
    /// Inscri��o Estadual do comprador.
    /// </summary>
    /// <example>123.456.789.012</example>
    public string InscricaoEstadual { get; set; }

    /// <summary>
    /// Indica se a Inscri��o Estadual est� isenta.
    /// </summary>
    /// <example>false</example>
    public bool InscricaoEstadualIsenta { get; set; }

    /// <summary>
    /// G�nero do comprador.
    /// </summary>
    /// <example>Masculino</example>
    public string? Genero { get; set; }

    /// <summary>
    /// Data de nascimento do comprador.
    /// </summary>
    /// <example>1990-01-01</example>
    public DateTime? DataNascimento { get; set; }

    /// <summary>
    /// Senha do comprador.
    /// </summary>
    /// <example>SenhaSecreta123</example>
    public string Senha { get; set; }
}
