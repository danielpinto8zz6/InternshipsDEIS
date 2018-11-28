using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class Company
    {
        // TODO : data anotations
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "You must provide a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide an address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Contact { get; set; }
    }
}