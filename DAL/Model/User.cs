using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Model.Common;

namespace DAL.Model
{
    public class User : IEntity
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Hash { get; set; }

        [StringLength(1024)]
        public string Salt { get; set; }

        [Required]
        public bool IsPasswordSet { get; set; }

        public List<Asset> Assets { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}