using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class RuleSummaryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Execution order
        /// </summary>
        public int Order { get; set; }
        public string[] Actions { get; set; }
        public bool IsActive { get; set; }
    }
}
