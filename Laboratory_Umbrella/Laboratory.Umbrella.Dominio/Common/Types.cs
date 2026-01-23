namespace Laboratory.Umbrella.Dominio.Common;

public static partial class Types
{
    public enum ResponseCodes
    {
        Success = 0,
        NotAuthorized = 401,
        InvalidRequestBody = 422,
        ServerException = 500
    }

    public enum Database
    {
        mongo,
        sqlserver
    }
}
