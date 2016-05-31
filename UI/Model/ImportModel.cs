using System;
using System.Collections.Generic;

namespace UI.Model
{
    public class ImportModel
    {
        public List<ImportCategoryModel> Categories { get; set; } 
        public List<string> Assets { get; set; } 
        public List<ImportTransactionModel>  Transactions { get; set; }
    }

    public class ImportCategoryModel
    {
        public string Name { get; set; }
        public List<string> Subcategories { get; set; } 
    }

    public class ImportTransactionModel
    {
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Asset { get; set; }
        public double Cost { get; set; }
        public string Comment { get; set; }
        public string Curency { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
    }
}
