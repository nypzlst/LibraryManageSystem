using Microsoft.AspNetCore.Identity;

namespace LibraryManageSystem.Database
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
