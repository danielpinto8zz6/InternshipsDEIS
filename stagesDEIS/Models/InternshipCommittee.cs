using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class InternshipCommittee
    {
        public string InternshipCommitteeId { get; set; }

        public IList<Professor> Professors { get; set; }

        public Proposal Proposal { get; set; }
        public int ProposalId { get; set; }

        public Statistics Statistics { get; }

        [Required]
        public int MaximumStagesByStudent { get; set; }
    }
}