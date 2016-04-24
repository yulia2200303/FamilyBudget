using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model.Common;
using Microsoft.Data.Entity.Metadata.Internal;

namespace DAL.Model
{
    public class User:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        [StringLength(1024)]
        public string Hash { get; set; }
        [StringLength(1024)]
        public string Salt { get; set; }
        [Required]
        public bool IsPasswordSet { get; set; }
        public ICollection<Asset> Assets { get; set; } 
      
    }
}
