using Application.Common.Helper;
using Domain.Models.Common;
using FluentValidation;
using MediatR;
using System.Net;
using static Domain.Enums.ZoneEnums;

namespace Application.Features.MemberFeatures.Commands
{
    public class InviteMemberCommand : IRequest<APIResponse>
    {
        public string? Email { get; set; }
        public string? Type { get; set; }
    }

    public class InviteMemberCommandValidator : AbstractValidator<InviteMemberCommand>
    {
        public InviteMemberCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required")
                .NotNull().WithMessage("Type is required")
                .Must(x => x!.IsInEnum<ZoneMembershipType, string>());
        }
    }

    public class InviteMemberCommandHandler : IRequestHandler<InviteMemberCommand, APIResponse>
    {
        public async Task<APIResponse> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
        {
            return new APIResponse
            {
                Status = HttpStatusCode.OK,
                Message = "Member invited successfully"
            };
        }
    }

}
