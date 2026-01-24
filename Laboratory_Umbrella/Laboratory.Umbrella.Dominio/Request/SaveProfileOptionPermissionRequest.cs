namespace Laboratory.Umbrella.Dominio.Request;

public class SaveProfileOptionPermissionRequest
{
    public string? Id { get; set; }
    public string ProfileId { get; set; } = string.Empty;
    public string OptionId { get; set; } = string.Empty;
    public int PermissionId { get; set; }
    public bool Granted { get; set; }
}
