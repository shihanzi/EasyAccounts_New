using System.ComponentModel.DataAnnotations;

namespace EasyAccounts.Models
{
    public class User
    {
            [Key]
            public int UserId { get; set; }

            [Required]
            public string Username { get; set; }

            [Required]
            public string PasswordHash { get; set; }
            public string Salt { get; set; }
            public ICollection<UserRole> UserRoles { get; set; }
    }
}
