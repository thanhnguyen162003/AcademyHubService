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

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> UpdateZone(Guid id, [FromBody] UpdateZoneCommand updateZoneCommand, CancellationToken cancellationToken = default)
        {
            updateZoneCommand.Id = id;
            var result = await _sender.Send(updateZoneCommand, cancellationToken);

            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> LeaveZone(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteZoneCommand { ZoneId = id }, cancellationToken);
            
            return StatusCode((int)result.Status, result);
        }

    }
}
