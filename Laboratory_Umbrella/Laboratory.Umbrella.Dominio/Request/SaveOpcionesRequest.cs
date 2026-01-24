namespace Laboratory.Umbrella.Dominio.Request;

public class SaveOpcionesRequest
{
    public string? Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string SectionId { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool Visible { get; set; }
    public int Status { get; set; }
}
