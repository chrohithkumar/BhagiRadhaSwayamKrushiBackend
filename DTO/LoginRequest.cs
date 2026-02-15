namespace BhagiRadhaSwayamKrushi.DTO
{
    // Login request
    public class LoginRequest
    {
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Login response
    namespace BhagiRadhaSwayamKrushi.DTO
    {
        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
            public DateTime Expiration { get; set; }
            public string UserName { get; set; } = string.Empty;

            public string Mobile { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string ActiveStatus { get; set; } = string.Empty;
        }
    }


    // Registration request
    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
