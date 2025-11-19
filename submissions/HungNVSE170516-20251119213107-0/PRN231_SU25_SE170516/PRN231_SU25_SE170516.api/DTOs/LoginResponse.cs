namespace PRN231_SU25_SE170516.api.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public int Role { get; set; }
    }
}
