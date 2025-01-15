using Application.Features.ZoneFeatures.Commands;
using Domain.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/v1/zones")]
    [ApiController]
    public class ZoneController : Controller
    {
        private readonly ISender _sender;

        public ZoneController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<ErrorValidation>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(command, cancellationToken);

            return StatusCode((int)result.Status, result);
        }

    }
}
