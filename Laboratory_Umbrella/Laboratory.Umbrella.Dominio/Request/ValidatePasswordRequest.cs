namespace Laboratory.Umbrella.Dominio.Request;

public class ValidatePasswordRequest
{
    public string UserId { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
