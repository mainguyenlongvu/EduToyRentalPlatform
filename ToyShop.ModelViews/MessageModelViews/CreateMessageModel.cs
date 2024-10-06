using System.ComponentModel.DataAnnotations;

namespace ToyShop.ModelViews.MessageModelViews
{
    public class CreateMessageModel
    {
        [Required]
        [Display(Name = "Chat ID")]
        public string ChatId { get; set; }

        [Required]
        [Display(Name = "Sender ID")]
        public string SenderId { get; set; }

        [Required]
        [Display(Name = "Message Text")]
        public string MessageText { get; set; }
    }
}
