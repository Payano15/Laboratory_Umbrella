using Laboratory.Umbrella.Dominio.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Services.Services;

public class BaseService
{
    #region Properties
    protected string Token { get; set; } = string.Empty;
    #endregion

    public virtual void InitService(CurrentParametersHelpers CurrentParametersHelpers)
    {
        Token = CurrentParametersHelpers.Token;
    }
}
