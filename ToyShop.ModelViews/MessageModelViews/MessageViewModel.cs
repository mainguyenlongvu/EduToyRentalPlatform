using System.ComponentModel.DataAnnotations;

namespace ToyShop.ModelViews.MessageModelViews
{
    public class MessageViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Chat ID")]
        public string ChatId { get; set; }

        [Display(Name = "Sender ID")]
        public string SenderId { get; set; }

        [Display(Name = "Message Text")]
        public string MessageText { get; set; }

        [Display(Name = "Created Time")]
        public DateTimeOffset CreatedTime { get; set; }

        [Display(Name = "Last Updated Time")]
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
