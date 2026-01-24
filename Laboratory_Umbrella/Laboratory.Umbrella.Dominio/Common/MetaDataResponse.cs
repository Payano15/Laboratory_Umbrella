using Laboratory.Umbrella.Dominio.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Laboratory.Umbrella.Dominio.Common;

public class MetaDataResponse<TData>
{
    public TData? data { get; set; }
    public MetaResponse? meta { get; set; }

    public MetaDataResponse(TData? Data)
    {
        data = Data;
        meta = new();
    }

    public MetaDataResponse(TData data, MetaResponse meta)
    {
        this.data = data;
        this.meta = meta;
    }
}