using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntershipsDEIS.Models
{
    public class Statistics
    {
        [DataType(DataType.Text)]
        public List<string> CompaniesByYear { get; }

        [DataType(DataType.Text)]
        public int TotalIntershipsByYear { get; set; }
    }
}