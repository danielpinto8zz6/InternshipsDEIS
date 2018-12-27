using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntershipsDEIS.Models
{
    public class Message
    {
        public string MessageId { get; set; }

        [Display(Name = "Sender")]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

        [Required]
        [Display(Name = "Recipient")]
        [ForeignKey("Recipient")]
        public string RecipientId { get; set; }

        public ApplicationUser Recipient { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public Boolean read { get; set; }

        public Message () {
            read = false;
        }
    }
}