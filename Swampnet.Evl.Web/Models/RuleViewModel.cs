using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swampnet.Core.Evl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Models
{
    public class RuleViewModel
    {
        /// <summary>
        /// Rule ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Active flag
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Rule name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Execution oprder
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Rule expression
        /// </summary>
        /// <remarks>
        /// If this expression evaluates to true, run any actions
        /// </remarks>
        public Expression Expression { get; set; }

        /// <summary>
        /// Actions to run if expression evaluates to true
        /// </summary>
        public ActionDefinition[] Actions { get; set; }

        //public IEnumerable<string> Operands => Enum.GetValues(typeof(RuleOperandType)).Cast<RuleOperandType>().Select(x => x.ToString());

        public MetaData MetaData { get; set; }
    }



    public class Expression
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RuleOperatorType Operator { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public RuleOperandType Operand { get; set; }


        public string Argument { get; set; }     // eg, property name

        public string Value { get; set; }

        public Expression[] Children { get; set; }

        public bool IsActive { get; set; }

        public bool IsContainer => Operator == RuleOperatorType.MATCH_ALL || Operator == RuleOperatorType.MATCH_ANY;

        public override string ToString()
        {
            return IsContainer
                ? $"{Operator} ({Children.Length} children)"
                : $"{Operand} {Operator} {Argument} '{Value}'";
        }
    }

    public class ActionDefinition
    {
        public bool IsActive { get; set; }
        public string Type { get; set; }

        public Property[] Properties { get; set; }

        public override string ToString()
        {
            return $"{Type}" + (IsActive ? "" : " (disabled)");
        }
    }


    public enum RuleOperandType
    {
        Null,
        Source,
        Category,
        Summary,
        Property
    }

    public enum RuleOperatorType
    {
        //[Display(Name = "@TODO: Friendly name")]
        NULL,

        EQ,
        NOT_EQ,
        REGEX,
        GT,
        GTE,
        LT,
        LTE,

        TAGGED,
        NOT_TAGGED,

        MATCH_ALL,
        MATCH_ANY
    }
}
