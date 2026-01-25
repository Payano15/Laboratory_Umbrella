namespace Laboratory.Umbrella.Dominio.Response;

public class LoginResponse
{
    public bool IsLoggedIn { get; set; } 
    public string DateExpired { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> RoleName { get; set; } = new();
    public List<SeccionesResponse> AllowSection { get; set; } = [];
    public List<string> Permissions { get; set; } = [];
}
