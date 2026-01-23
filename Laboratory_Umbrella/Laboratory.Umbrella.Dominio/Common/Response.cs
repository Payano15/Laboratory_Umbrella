using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Common;

public class Response<T>
{
    public string code { get; set; } = string.Empty;
    public string msg { get; set; } = string.Empty;
    public T? content { get; set; }

    public Response(T content)
    {
        this.code = "0";
        this.content = content;
    }

    public Response(string code, string msg)
    {
        this.code = code;
        this.msg = msg;
    }

    public Response() { }

}
