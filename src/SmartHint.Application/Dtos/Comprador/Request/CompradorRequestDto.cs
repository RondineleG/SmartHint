namespace SmartHint.Application.Dtos.Comprador.Request;

public class CompradorRequestDto
{
    public CompradorRequestDto()
    {
        NomeRazaoSocial = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        DataCadastro = DateTime.Now;
        Bloqueado = false;
        TipoPessoa = (int)ETipoPessoa.Fisica;
        CpfCnpj = string.Empty;
        InscricaoEstadual = string.Empty;
        InscricaoEstadualIsenta = false;
        Genero = (int)EGenero.Feminino;
        DataNascimento = DateTime.MinValue;
        Senha = string.Empty;
    }

    [Required(ErrorMessage = "O campo Nome/Razão Social é obrigatório")]
    [MaxLength(150, ErrorMessage = "O Nome/Razão Social deve ter no máximo 150 caracteres")]
    public string NomeRazaoSocial { get; set; }

    [Required(ErrorMessage = "O campo E-mail é obrigatório")]
    [MaxLength(150, ErrorMessage = "O E-mail deve ter no máximo 150 caracteres")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório")]
    [MaxLength(11, ErrorMessage = "O Telefone deve ter no máximo 11 caracteres")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "A Data de Cadastro é obrigatória")]
    public DateTime DataCadastro { get; set; }

    public bool Bloqueado { get; set; }

    [Required(ErrorMessage = "O Tipo de Pessoa é obrigatório")]
    [Range(1, 2)]
    public int TipoPessoa { get; set; }

    [Required(ErrorMessage = "O campo CPF/CNPJ é obrigatório")]
    [MaxLength(14, ErrorMessage = "O CPF/CNPJ deve ter no máximo 14 caracteres")]
    public string CpfCnpj { get; set; }

    [MaxLength(12, ErrorMessage = "A Inscrição Estadual deve ter no máximo 12 caracteres")]
    public string InscricaoEstadual { get; set; }

    public bool InscricaoEstadualIsenta { get; set; }

    [Range(1, 3)]
    public int Genero { get; set; }

    public DateTime? DataNascimento { get; set; }

    [Required(ErrorMessage = "A Senha é obrigatória")]
    [MinLength(8, ErrorMessage = "A Senha deve ter no mínimo 8 caracteres")]
    [MaxLength(15, ErrorMessage = "A Senha deve ter no máximo 15 caracteres")]
    public string Senha { get; set; }


    public static Core.Entities.Comprador Create(CompradorRequestDto request)
    {
        return new Core.Entities.Comprador(
            request.NomeRazaoSocial,
            request.Email,
            request.Telefone,
            request.DataCadastro,
            request.Bloqueado,
            (ETipoPessoa)request.TipoPessoa,
            request.CpfCnpj,
            request.InscricaoEstadual,
            request.InscricaoEstadualIsenta,
            (EGenero)request.Genero,
            request.DataNascimento,
            request.Senha
        );
    }

    public static Core.Entities.Comprador Update(CompradorRequestDto request, int id)
    {
        return new Core.Entities.Comprador(
            id,
            request.NomeRazaoSocial,
            request.Email,
            request.Telefone,
            request.DataCadastro,
            request.Bloqueado,
            (ETipoPessoa)request.TipoPessoa,
            request.CpfCnpj,
            request.InscricaoEstadual,
            request.InscricaoEstadualIsenta,
            (EGenero)request.Genero,
            request.DataNascimento,
            request.Senha
        );
    }

}
