namespace Application.Common.Models.OtherModel
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Avatar { get; set; } = null!;
    }
}
