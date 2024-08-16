using SmartHint.Core.Exceptions;

namespace SmartHint.Application.Services;

public class CompradorServico : ICompradorServico
{
    private readonly ICompradorRepositorio _compradorRepositorio;
    private readonly IValidacaoService _validacaoService;

    public CompradorServico(ICompradorRepositorio compradorRepositorio, IValidacaoService validacaoService)
    {
        _compradorRepositorio = compradorRepositorio;
        _validacaoService = validacaoService;
    }

    public async Task<CustomResult<Comprador>> ObterPorIdAsync(int id)
    {
        try
        {
            return await _compradorRepositorio.ObterPorIdAsync(id);
        }
        catch (Exception exception)
        {
            return CustomResult<Comprador>.WithError(exception.Message);
        }
    }

    public async Task<CustomResult<IEnumerable<Comprador>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        try
        {
            return await _compradorRepositorio.ObterTodosAsync(pagina, tamanhoPagina);
        }
        catch (Exception exception)
        {
            return CustomResult<IEnumerable<Comprador>>.WithError(exception.Message);
        }
    }

    public async Task<CustomResult<Comprador>> AdicionarAsync(Comprador comprador)
    {
        var resultadoValidacao = await ValidarCompradorAsync(comprador);
        if (!resultadoValidacao.IsValid)
        {
            var resultado = new CustomResult<Comprador>();
            if (resultadoValidacao.Errors.Any())
            {
                foreach (var error in resultadoValidacao.Errors)
                {
                    resultado.AddError(error);
                }
            }
            return resultado;
        }

        try
        {
            return await _compradorRepositorio.AdicionarAsync(comprador);
        }
        catch (CustomResultException cre)
        {
            return CustomResult<Comprador>.WithError(cre.CustomResult.Message);
        }
        catch (Exception ex)
        {
            return CustomResult<Comprador>.WithError($"Erro ao adicionar comprador: {ex.Message}");
        }
    }

    public async Task<CustomResult<Comprador>> AtualizarAsync(Comprador comprador)
    {
        var isUpadate = true;
        var resultadoValidacao = await ValidarCompradorAsync(comprador, isUpadate);
        if (!resultadoValidacao.IsValid)
        {
            var resultado = new CustomResult<Comprador>();
            if (resultadoValidacao.Errors.Any())
            {
                foreach (var error in resultadoValidacao.Errors)
                {
                    resultado.AddError(error);
                }
            }
            return resultado;
        }

        try
        {
            return await _compradorRepositorio.AtualizarAsync(comprador);
        }
        catch (CustomResultException cre)
        {
            return CustomResult<Comprador>.WithError(cre.CustomResult.Message);
        }
        catch (Exception ex)
        {
            return CustomResult<Comprador>.WithError($"Erro ao atualizar comprador: {ex.Message}");
        }
    }

    public async Task<CustomResult> DeletarAsync(int id)
    {
        try
        {
            return await _compradorRepositorio.DeletarAsync(id);
        }
        catch (Exception exception)
        {
            return CustomResult.WithError(exception.Message);
        }
    }

    public async Task<CustomResult<Comprador>> AlterarBloqueioAsync(int id, bool bloqueado)
    {
        var compradorResult = await _compradorRepositorio.ObterPorIdAsync(id);
        if (compradorResult.Status != ECustomResultStatus.Success || compradorResult.Data == null)
        {
            return compradorResult;
        }

        var comprador = compradorResult.Data;
        comprador.AtualizarBloqueado(bloqueado);

        try
        {
            return await _compradorRepositorio.AtualizarAsync(comprador);
        }
        catch (CustomResultException cre)
        {
            return CustomResult<Comprador>.WithError(cre.CustomResult.Message);
        }
        catch (Exception ex)
        {
            return CustomResult<Comprador>.WithError($"Erro ao atualizar comprador: {ex.Message}");
        }
    }

    private async Task<CustomValidationResult> ValidarCompradorAsync(Comprador comprador, bool isUpadate = false)
    {
        var resultadoValidacao = new CustomValidationResult();
        resultadoValidacao.Merge(_validacaoService.ValidarComprador(comprador));

        if (string.IsNullOrEmpty(comprador.NomeRazaoSocial))
        {
            resultadoValidacao.AddError("O campo Nome/Razão Social é obrigatório.", "NomeRazaoSocial");
        }
        else if (comprador.NomeRazaoSocial.Length > 150)
        {
            resultadoValidacao.AddError("O Nome/Razão Social deve ter no máximo 150 caracteres.", "NomeRazaoSocial");
        }

        if (string.IsNullOrEmpty(comprador.Email))
        {
            resultadoValidacao.AddError("O campo E-mail é obrigatório.", "Email");
        }
        else if (comprador.Email.Length > 150)
        {
            resultadoValidacao.AddError("O E-mail deve ter no máximo 150 caracteres.", "Email");
        }
        else if (!isUpadate && await _compradorRepositorio.EmailJaCadastradoAsync(comprador.Email))
        {
            resultadoValidacao.AddError("Este e-mail já está cadastrado.", "Email");
        }

        if (string.IsNullOrEmpty(comprador.Telefone))
        {
            resultadoValidacao.AddError("O campo Telefone é obrigatório.", "Telefone");
        }
        else if (!Regex.IsMatch(comprador.Telefone, @"^\d{1,11}$"))
        {
            resultadoValidacao.AddError("O Telefone deve ter no máximo 11 caracteres numéricos.", "Telefone");
        }

        if (comprador.TipoPessoa is not (ETipoPessoa)(int)ETipoPessoa.Fisica and not (ETipoPessoa)(int)ETipoPessoa.Juridica)
        {
            resultadoValidacao.AddError("O campo Tipo de Pessoa é obrigatório e deve ser 'Física' ou 'Jurídica'.", "TipoPessoa");
        }

        // Validação do CPF/CNPJ
        if (string.IsNullOrEmpty(comprador.CpfCnpj))
        {
            resultadoValidacao.AddError("O campo CPF/CNPJ é obrigatório.", "CpfCnpj");
        }
        else if (comprador.CpfCnpj.Length > 14)
        {
            resultadoValidacao.AddError("O CPF/CNPJ deve ter no máximo 14 caracteres.", "CpfCnpj");
        }
        else if (!isUpadate && await _compradorRepositorio.CpfCnpjJaCadastradoAsync(comprador.CpfCnpj))
        {
            resultadoValidacao.AddError("Este CPF/CNPJ já está cadastrado.", "CpfCnpj");
        }

        // Validação da Inscrição Estadual
        if (!string.IsNullOrEmpty(comprador.InscricaoEstadual))
        {
            if (comprador.InscricaoEstadual.Length > 12)
            {
                resultadoValidacao.AddError("A Inscrição Estadual deve ter no máximo 12 caracteres.", "InscricaoEstadual");
            }
            else if (!isUpadate && await _compradorRepositorio.InscricaoEstadualJaCadastradaAsync(comprador.InscricaoEstadual))
            {
                resultadoValidacao.AddError("Esta Inscrição Estadual já está cadastrada.", "InscricaoEstadual");
            }
        }
        else if (comprador.TipoPessoa == ETipoPessoa.Juridica)
        {
            resultadoValidacao.AddError("O campo Inscrição Estadual é obrigatório para Pessoa Jurídica.", "InscricaoEstadual");
        }

        return resultadoValidacao;
    }
}
