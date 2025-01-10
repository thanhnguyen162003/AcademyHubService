namespace AcademyHubService.PL.Events;

public record OrganizationRegistered : Event
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

public record OrganizationNameCorrected : Event
{
    public string Name { get; set; }
}