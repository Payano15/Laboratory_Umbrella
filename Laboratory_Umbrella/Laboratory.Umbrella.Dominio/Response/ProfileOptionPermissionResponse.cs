namespace Laboratory.Umbrella.Dominio.Response;

public class ProfileOptionPermissionResponse
{
    public string Id { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public string OptionId { get; set; } = string.Empty;
    public int PermissionId { get; set; }
    public bool Granted { get; set; }
}
