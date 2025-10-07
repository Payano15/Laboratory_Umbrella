namespace Laboratory.Umbrella.Dominio.Exception;

public class CustomException : System.Exception
{
    #region Propeties
    public string Code { get; set; } = string.Empty;
    public string FullMessage { get => $"{Code}-{Message}"; }
    #endregion

    #region Constructors
    public CustomException(string code, string message) : base(message)
    {
        Code = code;
    }
    #endregion
}
