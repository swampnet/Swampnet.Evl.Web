using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class EventSearchCriteriaViewModel
    {
        public Guid? Id { get; set; }

        public string Summary { get; set; }

        public DateTime? TimestampUtc { get; set; }

        public string Source { get; set; }

        public string SourceVersion { get; set; }

        /// <summary>
        /// Comma separated list of tags
        /// </summary>
		public string Tags { get; set; }
        public string Properties { get; set; }

        public bool ShowDebug { get; set; }
        public bool ShowInformation { get; set; } = true;
        public bool ShowError { get; set; } = true;

        public DateTime? FromDate { get; set; }
        public int FromHour { get; set; }
        public DateTime? ToDate { get; set; }
        public int ToHour { get; set; }

        public int PageSize { get; set; } = 50;
        public int Page { get; set; } = 0;

        public bool HasCriteria =>
            !string.IsNullOrEmpty(Summary)
            || !string.IsNullOrEmpty(Source)
            || !string.IsNullOrEmpty(Tags)
            || !string.IsNullOrEmpty(Properties)
            || FromDate.HasValue
            || ToDate.HasValue;
    }
}
