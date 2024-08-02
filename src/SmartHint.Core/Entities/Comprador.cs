namespace SmartHint.Core.Entities;
public class Comprador : BaseEntity
{
    [JsonConstructor]
    public Comprador()
    {
        NomeRazaoSocial = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        DataCadastro = DateTime.Now;
        Bloqueado = false;
        TipoPessoa = ETipoPessoa.Fisica;
        CpfCnpj = string.Empty;
        InscricaoEstadual = string.Empty;
        InscricaoEstadualIsenta = false;
        Genero = EGenero.Feminino;
        DataNascimento = DateTime.MinValue;
        Senha = string.Empty;
    }

    public Comprador(string nomeRazaoSocial, 
                              string email,
                              string telefone,
                              DateTime dataCadastro, 
                              bool bloqueado, 
                              ETipoPessoa tipoPessoa,
                              string cpfCnpj,
                              string inscricaoEstadual,
                              bool inscricaoEstadualIsenta,
                              EGenero? genero,
                              DateTime? dataNascimento,
                              string senha)
    {
        NomeRazaoSocial = nomeRazaoSocial;
        Email = email;
        Telefone = telefone;
        DataCadastro = dataCadastro;
        Bloqueado = bloqueado;
        TipoPessoa = tipoPessoa;
        CpfCnpj = cpfCnpj;
        InscricaoEstadual = inscricaoEstadual;
        InscricaoEstadualIsenta = inscricaoEstadualIsenta;
        Genero = genero;
        DataNascimento = dataNascimento;
        Senha = senha;
    }
    public Comprador(int id,
                              string nomeRazaoSocial,
                              string email,
                              string telefone,
                              DateTime dataCadastro,
                              bool bloqueado,
                              ETipoPessoa tipoPessoa,
                              string cpfCnpj,
                              string inscricaoEstadual,
                              bool inscricaoEstadualIsenta,
                              EGenero? genero,
                              DateTime? dataNascimento,
                              string senha) : base(id)
    {
        NomeRazaoSocial = nomeRazaoSocial;
        Email = email;
        Telefone = telefone;
        DataCadastro = dataCadastro;
        Bloqueado = bloqueado;
        TipoPessoa = tipoPessoa;
        CpfCnpj = cpfCnpj;
        InscricaoEstadual = inscricaoEstadual;
        InscricaoEstadualIsenta = inscricaoEstadualIsenta;
        Genero = genero;
        DataNascimento = dataNascimento;
        Senha = senha;
    }

    public string NomeRazaoSocial { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Bloqueado { get; private set; }
    public ETipoPessoa TipoPessoa { get; private set; }
    public string CpfCnpj { get; private set; }
    public string InscricaoEstadual { get; private set; }
    public bool InscricaoEstadualIsenta { get; private set; }
    public EGenero? Genero { get; private set; }
    public DateTime? DataNascimento { get; private set; }
    public string Senha { get; private set; }

    public int PegarId() => base.Id;

    public void AtualizarBloqueado(bool bloqueado) => Bloqueado = bloqueado;
}

