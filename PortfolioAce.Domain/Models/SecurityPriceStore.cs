﻿using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    [Table("SecurityPrices")]
    public class SecurityPriceStore
    {
        [Key]
        public int PriceId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required]
        public decimal ClosePrice { get; set; }

        [ForeignKey("Security")]
        public int SecurityId { get; set; }
        public SecuritiesDIM Security { get; set; }
    }
}