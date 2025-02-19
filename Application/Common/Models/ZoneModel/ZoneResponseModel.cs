namespace Application.Common.Models.ZoneModel
{
    public class ZoneResponseModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? LogoUrl { get; set; }

        public string? BannerUrl { get; set; }
        public int DocumentCount { get; set; }
        public int FlashcardCount { get; set; }
        public int FolderCount { get; set; }
        public int AssignmentCount { get; set; }
        public Guid? CreatedBy { get; set; }

    }
}
