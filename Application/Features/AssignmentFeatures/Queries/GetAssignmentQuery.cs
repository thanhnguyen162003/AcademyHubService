using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Application.Features.AssignmentFeatures.Queries
{
    public record GetAssignmentQuery : IRequest<PagedList<Assignment>>
    {
        public string? Search { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public Guid ZoneId;
    }

    public class GetAssignmentQueryHandler: IRequestHandler<GetAssignmentQuery, PagedList<Assignment>>
    {

        public async Task<PagedList<Assignment>> Handle(GetAssignmentQuery request, CancellationToken cancellationToken)
        {
           
            return new PagedList<Assignment>();

        }
    }
}