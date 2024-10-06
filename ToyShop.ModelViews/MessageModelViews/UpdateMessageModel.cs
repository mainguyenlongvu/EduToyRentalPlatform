using System;
using System.ComponentModel.DataAnnotations;

namespace ToyShop.ModelViews.MessageModelViews
{
    public class UpdateMessageModel
    {
        public string Id { get; set; }

        [Display(Name = "Chat ID")]
        public string ChatId { get; set; }

        [Display(Name = "Sender ID")]
        public string SenderId { get; set; }

        [Display(Name = "Message Text")]
        public string MessageText { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
