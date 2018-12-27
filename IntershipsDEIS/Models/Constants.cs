using System.ComponentModel.DataAnnotations;

namespace IntershipsDEIS.Models
{
    public enum State
    {
        [Display(Name = "Accepted")]
        ACCEPTED,
        [Display(Name = "Rejected")]
        REJECTED,
        [Display(Name = "Standby")]
        STANDBY
    }

    public enum Branch
    {
        [Display(Name = "Application Development")]
        APPLICATION_DEVELOPMENT,
        [Display(Name = "Information Systems")]
        INFORMATION_SYSTEMS,
        [Display(Name = "Networks")]
        NETWORKS
    }
}