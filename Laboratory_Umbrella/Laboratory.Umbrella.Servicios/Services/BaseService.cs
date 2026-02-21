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
    protected string UserLogged { get; set; } = string.Empty; 
    #endregion

    public virtual void InitService(CurrentParametersHelpers CurrentParametersHelpers)
    {
        Token = CurrentParametersHelpers.Token;
        UserLogged = CurrentParametersHelpers.UserLogged;
    }
    public static string DateToString(DateTime? date) => date is not null ? date.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
    public bool HasNextPage(int page, int pageSize, long totalCount)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return page < totalPages;
    }
}
