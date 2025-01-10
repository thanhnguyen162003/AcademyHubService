namespace AcademyHubService.WebApi.ServiceModel;

[Route("/typeaheads")]
public record FilterTypeahead : PaginatedQueryRequest, IReturn<PaginatedResult<RefEx>>
{
}