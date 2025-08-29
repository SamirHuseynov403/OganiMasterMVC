using Microsoft.AspNetCore.Identity;

namespace OganiMasterMVC.Data.DataContext
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
