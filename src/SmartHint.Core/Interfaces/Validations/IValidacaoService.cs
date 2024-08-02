﻿namespace SmartHint.Core.Interfaces.Validations;

public interface IValidacaoService
{
    CustomValidationResult ValidarEmail(string email);

    CustomValidationResult ValidarCPF(string cpf);

    CustomValidationResult ValidarDocumento(string documento);

    CustomValidationResult ValidarRG(string rg);

    CustomValidationResult ValidarCEP(string cep);

    CustomValidationResult ValidarTelefone(string telefone, ETipoContato tipoContato, int ddd);


    CustomValidationResult ValidarComprador(Comprador comprador);


    void AdicionarErroSeInvalido(CustomValidationResult resultado, string contexto, CustomResult response);

    void Validar<T>(
        T entidade,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResult response
    );

    void Validar<T>(
        IEnumerable<T> entidades,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResult response
    );
}
