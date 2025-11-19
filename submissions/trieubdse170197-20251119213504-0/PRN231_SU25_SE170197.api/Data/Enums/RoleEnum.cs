using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enums
{
    public static class RoleEnum
    {
        public const int Administrator = 5;
        public const int Moderator = 6;
        public const int Developer = 7;
        public const int Member = 4;

        public static string GetRoleName(int? roleId)
        {
            return roleId switch
            {
                Administrator => "Administrator",
                Moderator => "Moderator",
                Developer => "Developer",
                Member => "Member",
                _ => "Unknown"
            };
        }

        public static string GetRoleNames(string roleIds)
        {
            if (string.IsNullOrEmpty(roleIds))
                return "Unknown";

            var roles = roleIds.Split(',')
                .Select(id => int.TryParse(id.Trim(), out int roleId) ? GetRoleName(roleId) : "Unknown")
                .Where(role => role != "Unknown");

            return string.Join(", ", roles);
        }
    }
}
