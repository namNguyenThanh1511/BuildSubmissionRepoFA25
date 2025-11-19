using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public static class RoleMappings
    {
        public static readonly Dictionary<int, string> RoleIdToName = new()
        {
            { 5, "administrator" },
            { 6, "moderator" },
            { 7, "developer" },
            { 8, "member" }
        };

        public static string? GetRoleName(int? roleId)
        {
            if (roleId == null) return null;
            return RoleIdToName.TryGetValue(roleId.Value, out var roleName) ? roleName : null;
        }

        public static string[] AllowedRoles => RoleIdToName.Values.ToArray();
    }
}
