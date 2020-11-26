using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    [Table("dim_AssetClasses")]
    public class AssetClassDIM
    {
        [Key]
        public int AssetClassId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
