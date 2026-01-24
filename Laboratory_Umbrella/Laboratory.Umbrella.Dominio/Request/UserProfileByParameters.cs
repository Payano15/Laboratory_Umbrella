namespace Laboratory.Umbrella.Dominio.Request;

public class UserProfileByParameters
{
    public string UserId { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
