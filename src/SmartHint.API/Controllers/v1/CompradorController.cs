namespace SmartHint.API.Controllers.v1;

[ApiVersion("1.0")]
public class CompradorController : ApiBaseController
{
    private readonly IGetByIdCompradorUseCase _getByIdCompradorUseCase;
    private readonly IGetAllCompradorUseCase _getAllCompradorUseCase;
    private readonly IPutCompradorUseCase _putCompradorUseCase;
    private readonly IPostCompradorUseCase _postCompradorUseCase;
    private readonly IDeleteByIdCompradorUseCase _deleteByIdCompradorUseCase;
    private readonly IPatchCompradorUseCase _patchCompradorUseCase;
    private readonly ILogger _logger;

    public CompradorController(
        IGetByIdCompradorUseCase getByIdCompradorUseCase,
        IGetAllCompradorUseCase getAllCompradorUseCase,
        IPutCompradorUseCase putCompradorUseCase,
        IPostCompradorUseCase postCompradorUseCase,
        IDeleteByIdCompradorUseCase deleteByIdCompradorUseCase,
        IPatchCompradorUseCase patchCompradorUseCase,
        ILogger logger
    )
    {
        _getByIdCompradorUseCase = getByIdCompradorUseCase;
        _getAllCompradorUseCase = getAllCompradorUseCase;
        _putCompradorUseCase = putCompradorUseCase;
        _postCompradorUseCase = postCompradorUseCase;
        _deleteByIdCompradorUseCase = deleteByIdCompradorUseCase;
        _patchCompradorUseCase = patchCompradorUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos compradores cadastrados na base.
    /// </summary>
    /// <param name="pagina">Número da página (padrão: 1)</param>
    /// <param name="itensPorPagina">Número de itens por página (padrão: 20)</param>
    /// <returns>Retorna os compradores encontrados</returns>
    /// <response code="200">Retorna os compradores encontrados</response>
    /// <response code="400">Se ocorrer um erro na requisição.</response>
    /// <response code="404">Se não houver compradores cadastrados.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [ProducesResponseType(typeof(CustomResult<IEnumerable<CompradorResponseDto>>), StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status404NotFound)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet("Todos")]
    public async Task<IActionResult> ObterTodosAsync([FromQuery] int pagina = 1, [FromQuery] int itensPorPagina = 20)
    {
        if (pagina <= 0 || itensPorPagina <= 0)
        {
            return ResponseBadRequest("Os parâmetros de paginação devem ser maiores que zero.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(ObterTodosAsync),
                "Recebeu os parâmetros: pagina={Pagina}, itensPorPagina={ItensPorPagina}",
                pagina,
                itensPorPagina
            );

            var request = new CompradorPaginacaoRequestDto
            {
                Pagina = pagina,
                ItensPorPagina = itensPorPagina
            };

            var response = await _getAllCompradorUseCase.Execute(request);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(ObterTodosAsync),
                "Completou a execução. Status: {Status}. Tempo de execução: {TempoExecucao} ms",
                response.Status,
                stopwatch.ElapsedMilliseconds
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };

        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(CompradorController),
                nameof(ObterTodosAsync),
                "Encontrou uma exceção: {MensagemExcecao}",
                ex.Message
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(ObterTodosAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    /// <summary>
    /// Retorna um comprador com base no ID informado.
    /// </summary>
    /// <param name="compradorId">O ID do comprador a ser retornado.</param>
    /// <returns>Retorna o comprador encontrado.</returns>
    /// <response code="200">Retorna o comprador encontrado.</response>
    /// <response code="400">Se ocorrer um erro na requisição.</response>
    /// <response code="404">Se o comprador não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>

    [ProducesResponseType(typeof(CompradorRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CompradorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{compradorId}")]
    public async Task<IActionResult> ObterPorIdAsync(int compradorId)
    {
        if (compradorId <= 0)
        {
            return ResponseBadRequest("ID de comprador inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(ObterPorIdAsync),
                "Recebeu o parâmetro: compradorId={compradorId}",
                compradorId
            );

            var response = await _getByIdCompradorUseCase.Execute(compradorId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(ObterPorIdAsync),
                "Completou a execução. Status: {Status}. Tempo de execução: {TempoExecucao} ms",
                response.Status,
                stopwatch.ElapsedMilliseconds
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(CompradorController),
                nameof(ObterPorIdAsync),
                $"Encontrou uma exceção: {ex.Message}",
                ex.Message
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(ObterPorIdAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    /// <summary>
    /// Cria um comprador.
    /// </summary>
    /// <param name="request">Dados do comprador</param>
    /// <returns>Um novo comprador criado</returns>
    /// <response code="201">Retorna com o Id criado</response>
    /// <response code="400">Se o comprador passado for nulo</response>
    /// <response code="500">Se houver um erro ao criar um comprador</response>
    [ProducesResponseType(typeof(CompradorRequestDto), StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status201Created)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> AdicionarAsync([FromBody] CompradorRequestDto request)
    {
        if (request == null)
        {
            return ResponseBadRequest("O comprador não pode ser nulo.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AdicionarAsync),
                "Recebeu os parâmetros: compradorRequestDto={request}",
                request
            );

            var response = await _postCompradorUseCase.Execute(request);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AdicionarAsync),
                "Completou a execução. Status: {Status}. Tempo de execução: {TempoExecucao} ms",
                response.Status,
                stopwatch.ElapsedMilliseconds
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseCreated(response.Data),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(CompradorController),
                nameof(AdicionarAsync),
                $"Encontrou uma exceção: {ex.Message}",
                ex.Message
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AdicionarAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    /// <summary>
    /// Atualiza um comprador existente.
    /// </summary>
    /// <param name="compradorId">Id do comprador para atualizar</param>
    /// <param name="compradorUpdateRequestDto">Dados do comprador para atualizar</param>
    /// <returns>Retorna o comprador atualizado</returns>
    /// <response code="200">Retorna o comprador atualizado</response>
    /// <response code="400">Se ocorrer um erro na requisição.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [ProducesResponseType(typeof(CompradorRequestDto), StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPut("{compradorId}")]
    public async Task<IActionResult> AtualizarAsync(int compradorId, [FromBody] CompradorRequestDto compradorUpdateRequestDto)
    {
        if (compradorId <= 0)
        {
            return ResponseBadRequest("ID de comprador inválido.");
        }

        if (compradorUpdateRequestDto == null)
        {
            return ResponseBadRequest("Os dados do comprador não podem ser nulos.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AtualizarAsync),
                "Recebeu os parâmetros: compradorId={CompradorId}, compradorUpdateRequestDto={Request}",
                compradorId,
                compradorUpdateRequestDto
            );

            var request = new CompradorUpdateRequestDto
            {
                Id = compradorId,
                Comprador = compradorUpdateRequestDto
            };

            var response = await _putCompradorUseCase.Execute(request);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AtualizarAsync),
                "Completou a execução. Status: {Status}. Tempo de execução: {TempoExecucao} ms",
                response.Status,
                stopwatch.ElapsedMilliseconds
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(CompradorController),
                nameof(AtualizarAsync),
                $"Encontrou uma exceção: {ex.Message}",
                ex.Message
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AtualizarAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    /// <summary>
    /// Exclui um comprador.
    /// </summary>
    /// <param name="compradorId">Id do comprador</param>
    /// <response code="204">Retorna com o status da exclusão</response>
    /// <response code="400">Se ocorrer um erro na requisição.</response>
    /// <response code="404">Se o comprador passado for nulo</response>
    /// <response code="500">Se houver um erro ao excluir um comprador</response>
    [CustomResponse(StatusCodes.Status204NoContent)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status404NotFound)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{compradorId}")]
    public async Task<IActionResult> DeletarAsync(int compradorId)
    {
        if (compradorId <= 0)
        {
            return ResponseBadRequest("ID de comprador inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(DeletarAsync),
                "Recebeu o parâmetro: compradorId={CompradorId}",
                compradorId
            );

            var resultado = await _deleteByIdCompradorUseCase.Execute(compradorId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(DeletarAsync),
                "Exclusão concluída com sucesso para o compradorId={CompradorId}",
                compradorId
            );

            return resultado.Status switch
            {
                ECustomResultStatus.Success => ResponseNoContent(),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(resultado.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => ResponseBadRequest(resultado.Error.Description),
                _ => ResponseInternalServerError(resultado.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(CompradorController),
                nameof(DeletarAsync),
                $"Encontrou uma exceção: {ex.Message}",
                ex.Message
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(DeletarAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    /// <summary>
    /// Atualiza o status de bloqueio de um comprador.
    /// </summary>
    /// <param name="compradorId">Identificador único do comprador.</param>
    /// <param name="bloqueado">Novo status de bloqueio do comprador.</param>
    /// <returns>Retorna o response da operação de atualização.</returns>
    /// <response code="204">Indica que o status de bloqueio foi atualizado com sucesso.</response>
    /// <response code="404">Indica que o comprador com o ID fornecido não foi encontrado.</response>
    /// <response code="400">Indica que o ID do comprador ou o status de bloqueio são inválidos.</response>
    /// <response code="500">Indica que ocorreu um erro no servidor ao tentar atualizar o status de bloqueio do comprador.</response>
    [CustomResponse(StatusCodes.Status204NoContent)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status404NotFound)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPatch("{compradorId}/bloqueio")]
    public async Task<IActionResult> AlterarBloqueioAsync(int compradorId, bool bloqueado)
    {
        if (compradorId <= 0)
        {
            return ResponseBadRequest("ID de comprador inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AlterarBloqueioAsync),
                "Recebeu os parâmetros: compradorId={Id}, bloqueado={Bloqueado}",
                compradorId,
                bloqueado
            );

            var request = new CompradorBloqueadoRequestDto { Id = compradorId, Bloqueado = bloqueado };
            var response = await _patchCompradorUseCase.Execute(request);

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseNoContent(),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(CompradorController),
                nameof(AlterarBloqueioAsync),
                $"Encontrou uma exceção: {ex.Message}",
                ex.Message
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(CompradorController),
                nameof(AlterarBloqueioAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    private IActionResult HandleErrors<T>(CustomResult<T> response)
    {
        if (response.GeneralErrors != null && response.GeneralErrors.Count != 0)
        {
            return ResponseBadRequest(string.Join("; ", response.GeneralErrors));
        }
        else if (response.EntityErrors != null && response.EntityErrors.Count != 0)
        {
            var entityErrors = response.EntityErrors
                .SelectMany(e => e.Value.Select(v => $"{e.Key}: {v}"))
                .ToList();
            return ResponseBadRequest(string.Join("; ", entityErrors));
        }
        else if (response.Error != null)
        {
            return ResponseBadRequest(response.Error.Description);
        }
        else
        {
            return ResponseBadRequest(response.Message);
        }
    }
}
