namespace Laboratory.Umbrella.Dominio.Request;

public class SaveProfileRequest
{
    public string? Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; }
}
