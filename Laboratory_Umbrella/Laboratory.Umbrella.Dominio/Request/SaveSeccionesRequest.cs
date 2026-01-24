namespace Laboratory.Umbrella.Dominio.Request;

public class SaveSeccionesRequest
{
    public string? Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public int Status { get; set; }
}
