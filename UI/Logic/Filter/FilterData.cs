using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Logic.Filter
{
    public class FilterData
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public int Type { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
