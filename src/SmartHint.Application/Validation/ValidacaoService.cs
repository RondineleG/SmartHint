namespace SmartHint.Application.Validation;

public sealed class ValidacaoService : IValidacaoService
{
    public CustomValidationResult ValidarEmail(string email)
    {
        return ValidarCampos(email, RegexPatterns.Email, "Email");
    }

    public CustomValidationResult ValidarCPF(string cpf)
    {
        cpf = cpf.Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        var resultado = new CustomValidationResult();
        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]) || !ValidarDigitosCPF(cpf))
        {
            resultado.AddError("CPF inválido.", "CPF");
        }
        return resultado;
    }

    public CustomValidationResult ValidarDocumento(string documento)
    {
        documento = documento.Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        var resultado = new CustomValidationResult();
        if (documento.Length == 11)
        {
            if (documento.All(c => c == documento[0]) || !ValidarDigitosCPF(documento))
            {
                resultado.AddError("CPF inválido.", "CpfCnpj");
            }
        }
        else if (documento.Length == 14)
        {
            if (documento.All(c => c == documento[0]) || !ValidarDigitosCNPJ(documento))
            {
                resultado.AddError("CNPJ inválido.", "CpfCnpj");
            }
        }
        else
        {
            resultado.AddError("Documento deve ter 11 dígitos (CPF) ou 14 dígitos (CNPJ).", "CpfCnpj");
        }
        return resultado;
    }

    public CustomValidationResult ValidarRG(string rg)
    {
        return ValidarCampos(rg, RegexPatterns.RG, "CEP");
    }

    public CustomValidationResult ValidarCEP(string cep)
    {
        return ValidarCampos(cep, RegexPatterns.CEP, "CEP");
    }

    public CustomValidationResult ValidarTelefone(
        string telefone,
        ETipoContato tipoContato,
        int ddd
    )
    {
        var resultado = new CustomValidationResult();
        resultado.AddErrorIf(ddd is < 11 or > 99, "DDD inválido.", "DDD");
        var pattern = tipoContato switch
        {
            ETipoContato.Celular => RegexPatterns.Celular,
            ETipoContato.Residencial => RegexPatterns.ResidencialComercial,
            ETipoContato.Comercial => RegexPatterns.ResidencialComercial,
            _ => throw new ArgumentException("Tipo de contato inválido")
        };

        var validacaoTelefone = ValidarCampos(telefone, pattern, "Telefone");
        resultado.Merge(validacaoTelefone);
        return resultado;
    }

    
    public CustomValidationResult ValidarComprador(Comprador comprador)
    {
        var resultado = new CustomValidationResult();

        if (comprador == null)
        {
            return resultado.AddError("O comprador não pode ser nulo.");
        }
        resultado = resultado.Merge(ValidarEmail(comprador.Email));
        resultado = resultado.Merge(ValidarDocumento(comprador.CpfCnpj));
        return resultado;
    }

    public CustomValidationResult ValidarCampos(string valor, string pattern, string nomeCampo)
    {
        var resultado = new CustomValidationResult();
        if (valor == null)
        {
            resultado.AddError($"{nomeCampo} é nulo.", nomeCampo);
            return resultado;
        }
        if (!Regex.IsMatch(valor, pattern))
            resultado.AddError($"{nomeCampo} inválido.", nomeCampo);
        return resultado;
    }



    private bool ValidarDigitosCNPJ(string cnpj)
    {
        int[] multiplicadores1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicadores2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14)
        {
            return false;
        }

        var tempCnpj = cnpj.Substring(0, 12);
        var soma = 0;

        for (var i = 0; i < 12; i++)
        {
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];
        }

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCnpj += digito1;
        soma = 0;

        for (var i = 0; i < 13; i++)
        {
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];
        }

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
    }

    private bool ValidarDigitosCPF(string cpf)
    {
        int[] multiplicadores1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicadores2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
        }

        var resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        var digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (var i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
        }

        resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }

    public void AdicionarErroSeInvalido(
      CustomValidationResult resultado,
      string contexto,
      CustomResult response
  )
    {
        if (!resultado.IsValid)
        {
            foreach (var erro in resultado.Errors)
            {
                response.AddEntityError(contexto, erro);
            }
        }
    }

    public void Validar<T>(
       T entidade,
       Func<T, CustomValidationResult> funcValidacao,
       string nomeEntidade,
       CustomResult response
   )
    {
        var resultado = funcValidacao(entidade);
        AdicionarErroSeInvalido(resultado, $"{nomeEntidade}", response);
    }

    public void Validar<T>(
        IEnumerable<T> entidades,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResult response
    )
    {
        foreach (var entidade in entidades)
        {
            var resultado = funcValidacao(entidade);
            if (entidade is not null)
            {
                var propriedadeId = entidade.GetType().GetProperty("Id");
                var idValor =
                    propriedadeId != null
                        ? propriedadeId.GetValue(entidade)?.ToString() ?? "Desconhecido"
                        : "Desconhecido";
                AdicionarErroSeInvalido(resultado, $"{nomeEntidade} {idValor}", response);
            }
        }
    }

   
}
