using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet
{
    public interface IProperty
	{
		string Category { get; set; }

		string Name { get; set; }

		string Value { get; set; }
	}
}
