using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models
{
    [Table("ApplicationSettings")]
    public class ApplicationSettings
    {
        [Key]
        public int SettingId { get; set; }
        [Required]
        public string SettingName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string SettingValue { get; set; }
    }
}
