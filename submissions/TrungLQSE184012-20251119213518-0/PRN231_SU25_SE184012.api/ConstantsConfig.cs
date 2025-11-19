namespace PRN231_SU25_SE184012.api
{
    public static class ConstantsConfig
    {
        public const string Admin = "5";
        public const string Mod = "6";
        public const string Dev = "7";
        public const string Mem = "4";

        public static string GetRoleName(string roleId)
        {
            return roleId switch
            {
                Admin => "Administrator",
                Mod => "Moderator",
                Dev => "Developer",
                Mem => "Member",
                _ => "Unknown",
            };
        }

        public static class ErrorCodes
        {
            public const string BadRequest = "HB40001";
            public const string Unauthorized = "HB40101";
            public const string PermissionDenied = "HB40301";
            public const string NotFound = "HB40401";
            public const string InternalServer = "HB50001";
        }

        public static class ErrorMessages
        {
            public const string BadRequest = "Missing/invalid input";
            public const string Unauthorized = "Token missing/invalid";
            public const string PermissionDenied = "Permission Denied";
            public const string NotFound = "Resource not found";
            public const string InternalServer = "Internal Server Error";
        }
    }
}

