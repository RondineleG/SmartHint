namespace SmartHint.API.Controllers.v2;

[ApiVersion("2.0")]
public class CompradorController : ApiBaseController
{

    /// <summary>
    /// Controller V2.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public IActionResult Get()
    {
        return Ok("Controller V2 Funcionando!");
    }
}
