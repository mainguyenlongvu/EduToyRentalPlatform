using ToyShop.Core.Base;
using ToyShop.ModelViews.DeliveryModelView;

namespace ToyShop.Contract.Services.Interface
{
    public interface IDeliveryService
	{
		Task<ResponseDeliveryModel> Insert(CreateDeliveryModel deliveryDTO);

		Task Update(string id, CreateDeliveryModel deliveryDTO);

		Task<bool> Delete(string id);

		Task<ResponseDeliveryModel> GetById(string id);

		Task<IEnumerable<ResponseDeliveryModel>> GetAll();

		Task<BasePaginatedList<ResponseDeliveryModel>> GetPaging(int page, int pageSize);
	}
}
