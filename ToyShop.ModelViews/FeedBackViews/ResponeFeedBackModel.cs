using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.FeedBackModelViews
{
    public class ResponeFeedBackModel
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ToyId { get; set; } = null!;
        public DateTimeOffset? DeletedTime { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string? Content { get; set; }
    }
}
