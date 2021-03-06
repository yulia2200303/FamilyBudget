﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Model.Common;

namespace DAL.Model
{
    public class Transaction : IEntity
    {
        [Required]
        public int Type { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int AssetId { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("ProductId")]
        public Category Product { get; set; }

        [ForeignKey("AssetId")]
        public Asset Asset { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}