using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Request;

public record ClientRequest
{
    public string Name { get; init; } = string.Empty;
    public int PageSize { get; init; }
    public int PageNumber { get; init; }

}
