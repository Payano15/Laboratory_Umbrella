namespace Laboratory.Umbrella.Dominio.Request;

public class OpcionesByParameters
{
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public int? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
