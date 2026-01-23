namespace Laboratory.Umbrella.Dominio.Common;

public class CustomResponse<T>
{
    #region Properties
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Content { get; set; }
    #endregion

    #region Constructors
    public CustomResponse(T? content)
    {
        Content = content;
        Code = (int)Types.ResponseCodes.Success;
    }

    public CustomResponse(string message, Types.ResponseCodes code)
    {
        Code = (int)code;
        Message = message;
    }
    #endregion
}
