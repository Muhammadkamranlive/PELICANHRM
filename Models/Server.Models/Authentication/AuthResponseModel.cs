

namespace Server.Models
{
    public class AuthResponseModel
    {
        public string Token                 { get; set; }
        public string UserId                { get; set; }
        public string Message               { get; set; }
        public bool EmailStatus             { get; set; }
        
    }
}
