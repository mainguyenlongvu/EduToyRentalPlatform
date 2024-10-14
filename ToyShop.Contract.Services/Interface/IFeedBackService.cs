using ToyShop.Core.Base;
using ToyShop.ModelViews.FeedBackModelViews;

namespace ToyShop.Contract.Services.Interface
{
    public interface IFeedBackService
    {
        Task<ResponeFeedBackModel> GetFeedBackAsync(string id);
        Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacks_AdminAsync(int pageNumber, int pageSize, bool? sortByDate);
        Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacksAsync(int pageNumber, int pageSize, bool? sortByDate);
        Task<BasePaginatedList<ResponeFeedBackModel>> SearchFeedBacksAsync(int pageNumber, int pageSize, string? content, string? userId);
        Task<bool> CreateFeedBackAsync(CreateFeedBackModel model);
        Task<bool> UpdateFeedBackAsync(string id, ResponeFeedBackModel model);
        Task<bool> DeleteFeedBackAsync(string id);
    }
}
