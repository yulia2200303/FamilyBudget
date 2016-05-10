using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Model.Common;

namespace DAL.Model
{
    public class Currency : IEntity
    {
        [Required]
        [StringLength(16)]
        public string Code { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public double Converter { get; set; }

        [Required]
        public DateTime UpadeDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}