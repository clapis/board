using System.Threading.Tasks;
using Board.Application.Companies.GetCompanies;
using Board.Application.Companies.GetLogo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WebHost.Controllers
{
    [ApiController]
    [Route("companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [ResponseCache(Duration = (5 * 60), VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> Get(int page = 0)
        {
            var result = await _mediator.Send(new GetCompaniesQuery(page));

            return Ok(result.Companies);
        }


        [HttpGet("{id}/logo", Name = "Logo")]
        [ResponseCache(Duration = (15 * 24 * 60 * 60))]
        public async Task<IActionResult> GetLogo(int id)
        {
            var result = await _mediator.Send(new GetLogoQuery(id));

            if (result.Logo == null) return NotFound();

            return File(result.Logo, "image/png");
        }

    }
}
