using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model.Common;

namespace DAL.Model
{
    public class Currency:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(16)]
        public string Code { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        [Required]
        public double Converter { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
