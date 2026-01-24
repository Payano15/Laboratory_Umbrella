namespace Laboratory.Umbrella.Dominio.Response;

public class SeccionesResponse
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public int Status { get; set; }
}
