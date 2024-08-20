using Microsoft.AspNetCore.Mvc;
using NovibetApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class IpInfoController : ControllerBase
{
    private readonly IpInfoService _ipInfoService;

    public IpInfoController(IpInfoService ipInfoService)
    {
        _ipInfoService = ipInfoService;
    }

    [HttpGet("{ip}")]
    public async Task<IActionResult> GetIpInfo(string ip)
    {
        var ipInfo = await _ipInfoService.GetIpInfoAsync(ip);
        if (ipInfo == null)
        {
            return NotFound("IP information could not be retrieved.");
        }

        return Ok(ipInfo);
    }
}
