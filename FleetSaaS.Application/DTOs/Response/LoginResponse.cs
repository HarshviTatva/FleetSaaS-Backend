namespace FleetSaaS.Application.DTOs.Response
{
    public class LoginResponse
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
