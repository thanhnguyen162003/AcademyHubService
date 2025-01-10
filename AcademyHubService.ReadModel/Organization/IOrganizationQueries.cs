namespace AcademyHubService.ReadModel;

public interface IOrganizationQueries
{
    Task<PaginatedResult<Organization>> Execute(PaginatedQueryRequest qry);
}