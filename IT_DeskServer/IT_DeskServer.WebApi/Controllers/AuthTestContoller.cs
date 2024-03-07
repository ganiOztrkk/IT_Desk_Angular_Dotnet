using IT_DeskServer.WebApi.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace IT_DeskServer.WebApi.Controllers;

public class AuthTestContoller : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Auth Test Edildi.");
    }
}