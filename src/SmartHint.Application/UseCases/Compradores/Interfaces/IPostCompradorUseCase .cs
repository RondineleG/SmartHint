using SmartHint.Application.Dtos.Comprador.Request;
using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Core.Interfaces.UseCases;

namespace SmartHint.Application.UseCases.Compradores.Interfaces;

public interface IPostCompradorUseCase : IUseCase<CompradorRequestDto, CompradorResponseDto> { }
