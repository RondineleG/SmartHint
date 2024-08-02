namespace SmartHint.Application.Dtos.Comprador.Request;

public class CompradorUpdateRequestDto
{
    public int Id { get; set; }
    public CompradorRequestDto Comprador { get; set; } = new CompradorRequestDto();
}
