using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models.KeyViewModels
{
    public class KeysViewModel
    {
        public IEnumerable<ApiKey> Keys { get; set; }
        public Guid? SelectedKey { get; set; }
    }
}
