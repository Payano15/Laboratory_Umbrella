using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Entities;

public class UserAuthentication: IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime DateIssue { get; set; } = DateTime.Now;
    public string UserName { get; set; } = string.Empty;
    public DateTime DateExpired { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AnulledAt { get; set; }
}
