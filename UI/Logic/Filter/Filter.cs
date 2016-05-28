using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Logic.Filter
{
    public class Filter
    {
        public static readonly int AllFilter = -1;
        public Filter()
        {
            Categories = new List<FilterData>();
            //SelectedValue = AllFilter;
        }
        public string Name { get; set; }
        public FilterData SelectedItem { get; set; }
        public List<FilterData> Categories { get; set; }

        public FilterData DefaultValue => Categories.Count > 0 ? Categories[0] : null;
    }
}
