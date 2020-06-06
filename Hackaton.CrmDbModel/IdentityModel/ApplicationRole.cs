using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Hackaton.CrmDbModel.IdentityModel
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public ApplicationRole(string roleName)
            : base(roleName) { }

        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
}
