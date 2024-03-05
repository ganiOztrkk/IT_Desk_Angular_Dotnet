using Microsoft.AspNetCore.Mvc;

namespace IT_DeskServer.WebApi.Abstracts;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class BaseController : ControllerBase { }