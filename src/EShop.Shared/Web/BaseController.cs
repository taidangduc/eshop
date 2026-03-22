using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Shared.Web;

[ApiController]
[Route(BaseApiPath)]
public abstract class BaseController : ControllerBase
{
    protected const string BaseApiPath = "api";
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}

