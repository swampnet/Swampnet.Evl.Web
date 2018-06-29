using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class MetaData
    {
        public ActionMetaData[] ActionMetaData { get; set; }
        public ExpressionOperator[] Operators { get; set; }
        public MetaDataCapture[] Operands { get; set; }
    }


    public class Option
    {
        public string Display { get; set; }
        public string Value { get; set; }
    }


    public class ActionMetaData
    {
        public string Type { get; set; }
        public MetaDataCapture[] Properties { get; set; }

    }



    public class ExpressionOperator
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RuleOperatorType Code { get; set; }
        public string Display { get; set; }
        public bool IsGroup { get; set; }
        public bool RequiresOperand { get; set; }
        public bool RequiresValue { get; set; }
    }


    public class MetaDataCapture
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public string DataType { get; set; }
        public Option[] Options { get; set; }
    }
}
