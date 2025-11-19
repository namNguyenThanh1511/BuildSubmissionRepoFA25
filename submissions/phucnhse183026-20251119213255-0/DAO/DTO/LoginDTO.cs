namespace DAO.DTO
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public LoginRequest() { }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string RoleName { get; set; }
    }
}
