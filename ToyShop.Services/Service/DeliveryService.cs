using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.DeliveryModelView;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;

namespace ToyShop.Services.Service
{
    public class DeliveryService : IDeliveryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DeliveryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
            _mapper = mapper;
        }

		public async Task<ResponseDeliveryModel> Insert(CreateDeliveryModel deliveryDTO)
		{
			ArgumentNullException.ThrowIfNull(deliveryDTO);
			if (deliveryDTO.ContractId == null)
			{
				throw new ArgumentNullException("ContractId is required.");
			}
			try
            {
                ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().GetByIdAsync(deliveryDTO.ContractId) ?? throw new KeyNotFoundException("Contract not found.");
				Delivery delivery = _mapper.Map<Delivery>(deliveryDTO);
				delivery.LastUpdatedTime = CoreHelper.SystemTimeNow;

				await _unitOfWork.GetRepository<Delivery>().InsertAsync(delivery);
				await _unitOfWork.SaveAsync();

				return _mapper.Map<ResponseDeliveryModel>(delivery);
            }
			catch (KeyNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to insert delivery. Please try again later.", ex);
			}
		}


		public async Task<bool> Delete(string id)
		{
			try
			{
				Delivery delivery = _unitOfWork.GetRepository<Delivery>().Entities.AsNoTracking().FirstOrDefault(d => d.Id == id)
					?? throw new KeyNotFoundException($"Delivery with id {id} not found.");

				delivery.DeletedTime = CoreHelper.SystemTimeNow;
				delivery.LastUpdatedTime = CoreHelper.SystemTimeNow;

                _unitOfWork.GetRepository<Delivery>().Update(delivery);
				await _unitOfWork.SaveAsync();
				return true;
			}
			catch (KeyNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to delete delivery. Please try again later.", ex);
			}
		}

		public async Task<IEnumerable<ResponseDeliveryModel>> GetAll()
		{
			try
			{
				var deliveries = await _unitOfWork.GetRepository<Delivery>().GetAllAsync();
				return deliveries.Where(d => !d.DeletedTime.HasValue).Select(d => _mapper.Map<ResponseDeliveryModel>(d));
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to retrieve deliveries. Please try again later.", ex);
			}
		}


		public async Task<ResponseDeliveryModel> GetById(string id)
		{
			try
			{
                var delivery = await _unitOfWork.GetRepository<Delivery>().GetByIdAsync(id);
                if (delivery is null || delivery.DeletedTime.HasValue)
                {
                    throw new KeyNotFoundException("Delivery not found or has been deleted.");
                }
                return _mapper.Map<ResponseDeliveryModel>(delivery);
            }
			catch (KeyNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to retrieve delivery. Please try again later.", ex);
			}
		}


		public async Task<BasePaginatedList<ResponseDeliveryModel>> GetPaging(int page, int pageSize)
		{
			try
			{
				if(page < 1 || pageSize < 1)
				{
					throw new ArgumentException("Invalid page or pageSize value.");
				}
				var query = _unitOfWork.GetRepository<Delivery>().Entities.Where(d => !d.DeletedTime.HasValue);
				var totalItems = await query.CountAsync();
				var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
				var deliveryReponses = items.Select(d => _mapper.Map<ResponseDeliveryModel>(d)).ToList();

				return new BasePaginatedList<ResponseDeliveryModel>(deliveryReponses, totalItems, page, pageSize);
			}
			catch (ArgumentException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("An error occurred while fetching paginated deliveries.", ex);
			}
		}

		public async Task Update(string id, CreateDeliveryModel deliveryDTO)
		{
			ArgumentNullException.ThrowIfNull(deliveryDTO);
			if (deliveryDTO.ContractId == null)
			{
				throw new ArgumentNullException(nameof(deliveryDTO.ContractId), "ContractId is required.");
			}

			try
			{
				var existingDelivery = _unitOfWork.GetRepository<Delivery>().Entities.AsNoTracking().FirstOrDefault(d => d.Id == id)
					?? throw new KeyNotFoundException("Delivery not found.");

				var contract = await _unitOfWork.GetRepository<ContractEntity>()
					.GetByIdAsync(deliveryDTO.ContractId)
					?? throw new KeyNotFoundException($"Contract with id {deliveryDTO.ContractId} not found.");

				existingDelivery = _mapper.Map<Delivery>(deliveryDTO);
				existingDelivery.Id = id;
				existingDelivery.LastUpdatedTime = CoreHelper.SystemTimeNow;

                _unitOfWork.GetRepository<Delivery>().Update(existingDelivery);

				await _unitOfWork.SaveAsync();

            }
			catch (KeyNotFoundException)
			{
				throw; 
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(ex.Message, ex);
			}
		}

	}
}
