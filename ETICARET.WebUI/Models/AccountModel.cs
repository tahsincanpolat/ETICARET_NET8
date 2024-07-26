using ETICARET.WebUI.Identity;

namespace ETICARET.WebUI.Models
{
    public class AccountModel : ApplicationUser
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
