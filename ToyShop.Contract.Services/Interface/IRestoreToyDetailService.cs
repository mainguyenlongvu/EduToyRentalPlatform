using System.Threading.Tasks;
using ToyShop.Core.Base;
using ToyShop.ModelViews.RestoreToyDetailModelViews;
using ToyShop.Contract.Repositories.Entity;

namespace ToyShop.Services.Service
{
    public interface IRestoreToyDetailService
    {
        Task<RestoreToyDetail> Insert(RestoreToyDetail restoreToyDetail);
        Task<bool> Delete(string id);
        Task<RestoreToyDetail> GetById(string id);
        Task<BasePaginatedList<RestoreToyDetail>> GetPaging(int page, int pageSize, string id);
        Task Update(string id, UpdateRestoreDetailModel restoreToyDetail);
		Task<RestoreToyDetail> GetByRestoreToyId(string restoreToyId);

	}
}
