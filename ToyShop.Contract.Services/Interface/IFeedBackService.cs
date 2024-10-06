using ToyShop.Core.Base;
using ToyShop.ModelViews.FeedBackModelViews;

namespace ToyShop.Contract.Services.Interface
{
    public interface IFeedBackService
    {
        Task<ResponeFeedBackModel> GetFeedBackAsync(string id);
        Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacksAsync(int pageNumber, int pageSize, bool? sortByDate);
        Task<BasePaginatedList<ResponeFeedBackModel>> SearchFeedBacksAsync(int pageNumber, int pageSize, string? content, string? userId);
        Task<ResponeFeedBackModel> CreateFeedBackAsync(CreateFeedBackModel model);
        Task<ResponeFeedBackModel> UpdateFeedBackAsync(string id, CreateFeedBackModel model);
        Task<ResponeFeedBackModel> DeleteFeedBackAsync(string id);
    }
}
