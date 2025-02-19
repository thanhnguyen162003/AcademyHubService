using Application.Common.Models.TestContent;
using Application.Features.AssignmentFeatures.Commands;
using Application.Features.ZoneFeatures.Commands;
using Domain.Constants;
using Domain.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/v1/testcontent")]
    [ApiController]
    public class TestContentController : Controller
    {
        private readonly ISender _sender;

        public TestContentController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost("{assignmentId}")]
        [ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<ErrorValidation>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [Authorize(PolicyType.Teacher)]
        public async Task<IActionResult> CreateTestContent(Guid assignmentId, [FromBody] List<TestContentCreateModel> testContent, CancellationToken cancellationToken = default)
        {
            var query = new CreateTestContentCommand
            {
                AssignmentId = assignmentId,
                TestContent = testContent
            };
            var result = await _sender.Send(query, cancellationToken);

            return StatusCode((int)result.Status, result);
        }

        //[HttpPatch("{id}")]
        //[ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        //[Authorize(PolicyType.Teacher)]
        //public async Task<IActionResult> UpdateZone(Guid id, [FromBody] UpdateZoneCommand updateZoneCommand, CancellationToken cancellationToken = default)
        //{
        //    updateZoneCommand.Id = id;
        //    var result = await _sender.Send(updateZoneCommand, cancellationToken);

        //    return StatusCode((int)result.Status, result);
        //}

        //[HttpDelete("{id}")]
        //[ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        //[Authorize(PolicyType.Teacher)]
        //public async Task<IActionResult> DeleteZone(Guid id, CancellationToken cancellationToken = default)
        //{
        //    var result = await _sender.Send(new DeleteZoneCommand { ZoneId = id }, cancellationToken);

        //    return StatusCode((int)result.Status, result);
        //}

        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(APIResponse<Guid>), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(APIResponse), StatusCodes.Status403Forbidden)]
        //[Authorize(PolicyType.AcademicUser)]
        //public async Task<IActionResult> LeaveZone(Guid id, CancellationToken cancellationToken = default)
        //{
        //    var result = await _sender.Send(new LeaveZoneCommand { ZoneId = id }, cancellationToken);

        //    return StatusCode((int)result.Status, result);
        //}

    }
}
