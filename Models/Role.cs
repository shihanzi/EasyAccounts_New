﻿namespace EasyAccounts.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleType { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
