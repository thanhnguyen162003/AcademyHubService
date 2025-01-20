namespace Application.Common.Models.ProduceModels
{
    public class InviteMailModel
    {
        public string Email { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public Guid InviteBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
