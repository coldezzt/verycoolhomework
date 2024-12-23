using Microsoft.AspNetCore.Mvc;
using MongoRedisPractice.Services;

namespace MongoRedisPractice.Controllers;

[Route("api/[controller]/{value}")]
[ApiController]
public class StringController(PairService serv) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string value)
    {
        var resp = await serv.GetAsync(value);
        return resp is null ? NotFound() : Ok(resp);
    }

    [HttpPost]
    public async Task<IActionResult> Post(string value)
    {
        var resp = await serv.PostAsync(value);
        return resp is null ? BadRequest() : Ok(resp);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string value)
    {
        var resp = await serv.DeleteAsync(value);
        return resp ? Ok("Rampage!!!") : BadRequest();
    }
}