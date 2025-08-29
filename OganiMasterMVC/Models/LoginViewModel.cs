using System.ComponentModel.DataAnnotations;

namespace OganiMasterMVC.Models
{
    public class LoginViewModel
    {
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
