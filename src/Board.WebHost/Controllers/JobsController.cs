using System.Threading.Tasks;
using Board.Application.Jobs.GetJobs;
using Board.Application.Jobs.PostJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Board.WebHost.Controllers
{
    [ApiController]
    [Route("jobs")]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [ResponseCache(Duration = (5 * 60), VaryByQueryKeys = new[] { "*" })]
        public async Task<IActionResult> Get(int page = 0)
        {
            var result = await _mediator.Send(new GetJobsQuery(page));

            return Ok(result.Jobs);
        }


        [HttpPost]
        public async Task<IActionResult> Post(PostJobCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

    }
}
