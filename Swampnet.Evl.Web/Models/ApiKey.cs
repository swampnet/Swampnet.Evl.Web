using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class ApiKey
    {
        public ApiKey()
        {
            IsEnabled = true;
        }

        public long Id { get; set; }
        public Guid Key { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }

        public ApplicationUser User { get; set; }
    }
}
