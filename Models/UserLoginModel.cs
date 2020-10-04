using System;
namespace UserAuthentication.Models
{
    public class UserLoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
