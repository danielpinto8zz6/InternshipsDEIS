using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stagesDEIS.Models
{
    public class Company
    {
        [ForeignKey("User")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "You must provide a name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name should be minimum 3 characters and a maximum of 100 characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide an address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid phone number")]
        public string Contact { get; set; }

        // public IList<Proposal> Proposals { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}