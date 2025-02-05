using Application.Common.Helper;
using Application.Messages;
using Application.Services.Authentication;
using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;
using System.Net;
using static Domain.Enums.ZoneEnums;

namespace Application.Features.ZoneFeatures.Commands
{
    public class CreateZoneCommand : IRequest<APIResponse>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? BannerUrl { get; set; }
    }

    public class CreateZoneCommandValidator : AbstractValidator<CreateZoneCommand>
    {
        public CreateZoneCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.LogoUrl)
                .Must(Utils.IsValidUrl).WithMessage("{PropertyName} must be a valid URL.");

            RuleFor(p => p.BannerUrl)
                .Must(Utils.IsValidUrl).WithMessage("{PropertyName} must be a valid URL.");

        }
    }

    public class CreateZoneCommandHandler : IRequestHandler<CreateZoneCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public CreateZoneCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(CreateZoneCommand request, CancellationToken cancellationToken)
        {
            // Create zone
            var zone = _mapper.Map<Zone>(request);
            zone.CreatedBy = _authenticationService.User.UserId;
            await _unitOfWork.ZoneRepository.Add(zone);

            if (await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.Created,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = zone.Id
                };
            }

            return new APIResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Message = MessageCommon.CreateFailed
            };
        }
    }

}
