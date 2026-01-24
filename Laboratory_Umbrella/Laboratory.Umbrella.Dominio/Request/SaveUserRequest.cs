namespace Laboratory.Umbrella.Dominio.Request;

public class SaveUserRequest
{
    public string? Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Status { get; set; }
    public List<string> UserProfileIds { get; set; } = new();
}
