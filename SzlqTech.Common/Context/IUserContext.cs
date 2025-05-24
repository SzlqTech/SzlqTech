using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Common.Context
{
    public interface IUserContext
    {
        long? UserId { get; set; }

        string? UserCode { get; set; }

        string? Username { get; set; }

        string? PersonName { get; set; }

        string? RoleCode { get; }

        long? RoleId { get; set; }

        string? Session { get; }
    }
}
