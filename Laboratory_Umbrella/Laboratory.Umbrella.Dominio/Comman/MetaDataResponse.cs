using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Comman;

public class MetaDataResponse<TData, TMeta>
{
    public TData? data { get; set; }
    public TMeta? meta { get; set; }

    public MetaDataResponse(TData data, TMeta meta)
    {
        this.data = data;
        this.meta = meta;
    }
}