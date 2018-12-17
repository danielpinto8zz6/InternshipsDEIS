using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public enum State
    {

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