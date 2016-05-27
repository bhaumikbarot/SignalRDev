using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.BO.Models
{
    [Table("DevTest")]
    public sealed class DevTest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Campaign Name")]
        public string CampaignName { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [Display(Name = "Clicks")]       
        public int? Clicks { get; set; }
        [Display(Name = "Conversions")]
        public int? Conversions { get; set; }
        [Display(Name = "Impressions")]
        public int? Impressions { get; set; }
        [Required]
        [Display(Name = "Affiliate Name")]
        public string AffiliateName { get; set; }
    }
}
