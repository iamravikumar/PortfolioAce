using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioAce.Domain.Models.Dimensions
{
    public class AssetClassDIM
    {
        [Key]
        public int AssetClassID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
