using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Model.Common;

namespace DAL.Model
{
    public class Asset : IEntity
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}