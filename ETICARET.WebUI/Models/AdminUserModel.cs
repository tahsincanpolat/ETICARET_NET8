using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class AdminUserModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
