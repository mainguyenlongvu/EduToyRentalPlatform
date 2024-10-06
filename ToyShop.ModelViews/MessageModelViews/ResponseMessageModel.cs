using System.ComponentModel.DataAnnotations;

namespace ToyShop.ModelViews.MessageModelViews
{
    public class ResponseMessageModel
    {
        [Required]
        [Display(Name = "Message ID")]
        public string MessageId { get; set; }

        [Required]
        [Display(Name = "Response Text")]
        public string ResponseText { get; set; }

        [Display(Name = "Responded By")]
        public string RespondedBy { get; set; }
    }
}
