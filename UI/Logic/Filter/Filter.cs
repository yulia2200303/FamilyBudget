using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Logic.Filter
{
    public class Filter
    {
        public Filter()
        {
            Items = new List<FilterData>();
        }
        public string Name { get; set; }
        public FilterData SelectedItem { get; set; }
        public List<FilterData> Items { get; set; }

        public FilterData DefaultValue => Items.Count > 0 ? Items[0] : null;
    }
}
