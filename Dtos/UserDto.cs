using EasyAccounts.Models;

namespace EasyAccounts.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
