using System;
using System.Linq;
using SIS.Framework.Security.Interfaces;

namespace SIS.Framework.Attributes.Action
{
    public class AuthorizeAttribute : Attribute
    {
        private readonly string role;

        public AuthorizeAttribute()
        {

        }

        public AuthorizeAttribute(string role)
        {
            this.role = role;
        }

        private bool IsIdentityPresent(IIdentity identity)
        {
            return identity != null;
        }

        private bool IsIdentityInRole(IIdentity identity)
        {
            if (this.IsIdentityPresent(identity))
            {
                return identity.Roles.Contains(this.role);
            }

            return false;
        }

        public bool IsAuthorized(IIdentity identity)
        {
            if (this.role == null)
            {
                return this.IsIdentityPresent(identity);
            }

            return this.IsIdentityInRole(identity);
        }
    }
}
