namespace EShop.Api.Models.Users;

public class UserModel
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public string? AvatarUrl { get; set; }
    public string Status { get; set; }
}