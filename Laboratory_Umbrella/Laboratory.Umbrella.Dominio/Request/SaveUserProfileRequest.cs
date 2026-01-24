namespace Laboratory.Umbrella.Dominio.Request;

public class SaveUserProfileRequest
{
    public string? Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
}
