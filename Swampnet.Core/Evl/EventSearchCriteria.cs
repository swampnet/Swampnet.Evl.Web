﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Core.Evl
{
    public class EventSearchCriteria
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// Comma separated list of categories to include in search results (empty for all)
        /// </summary>
        public string Categories { get; set; }

        public string Summary { get; set; }

        public DateTime? TimestampUtc { get; set; }

        public string Source { get; set; }

        public string SourceVersion { get; set; }

        /// <summary>
        /// Comma separated list of tags
        /// </summary>
		public string Tags { get; set; }

        public string Properties { get; set; }

        #region Advanced
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        #endregion
    }
}
