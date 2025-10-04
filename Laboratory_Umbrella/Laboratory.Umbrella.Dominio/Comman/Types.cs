using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Comman;

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
        mongo
    }
}
