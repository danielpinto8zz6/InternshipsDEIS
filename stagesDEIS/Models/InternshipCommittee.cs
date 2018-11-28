using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class InternshipCommittee
    {
        public IList<Professor> Professors { get; set; }

        public Proposal Proposal { get; set; }
        public int ProposalId { get; set; }

        [Required(ErrorMessage = "You must provide a justification")]
        [DataType(DataType.MultilineText)]
        public string Justification { get; set; }
    }
}