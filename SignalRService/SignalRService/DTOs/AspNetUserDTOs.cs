using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.DTOs
{
    public class AspNetUsersDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AcccessFailedCount { get; set; }
        public string UserName { get; set; }
    }

    public class AspNetUserLoginsDTO
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }
    }

    public class AspNetRolesDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AspNetUserRolesDTO
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }

    public class AspNetUserDataDTO
    {
        public List<AspNetUsersDTO> Users { get; set; }
        public List<AspNetUserLoginsDTO> UserLogins { get; set; }
        public List<AspNetRolesDTO>Roles { get; set; }
        public List<AspNetUserRolesDTO>UserRoles { get; set; }
    }

}