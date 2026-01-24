namespace Laboratory.Umbrella.Dominio.Request;

public class ProfileOptionPermissionByParameters
{
    public string ProfileId { get; set; } = string.Empty;
    public string OptionId { get; set; } = string.Empty;
    public int? PermissionId { get; set; }
    public bool? Granted { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
