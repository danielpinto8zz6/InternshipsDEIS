using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stagesDEIS.Models
{
    public class InternshipCommittee
    {
        public string InternshipCommitteeId { get; set; }

        public IList<Professor> Professors { get; set; }

        [ForeignKey("Proposal")]
        public int ProposalId { get; set; }
        public Proposal Proposal { get; set; }

        public Statistics Statistics { get; }

        [Required]
        public int MaximumStagesByStudent { get; set; }
    }
}