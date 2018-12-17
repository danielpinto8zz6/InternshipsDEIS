using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class Statistics
    {
        [DataType(DataType.Text)]
        public List<string> CompaniesByYear { get; }

        [DataType(DataType.Text)]
        public int TotalStagesByYear { get; set; }
    }
}