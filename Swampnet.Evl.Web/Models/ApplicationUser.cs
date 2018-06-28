using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Swampnet.Evl.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            ApiKeys = new List<ApiKey>();
        }

        public Guid? ActiveApiKey { get; set; }
        public string TimeZone { get; set; }
        public ICollection<ApiKey> ApiKeys { get; set; }
    }
}
