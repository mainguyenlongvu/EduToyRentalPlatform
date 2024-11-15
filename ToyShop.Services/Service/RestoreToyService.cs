using AutoMapper;
using System.Data.Entity;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Core.Base;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.RestoreToyModelViews;

namespace ToyShop.Services.Service
{
    public class RestoreToyService : IRestoreToyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RestoreToyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RestoreToy> Insert(RestoreToy restoreToy)
        {
            ArgumentNullException.ThrowIfNull(restoreToy);
            if (restoreToy.ContractId == null)
            {
                throw new ArgumentNullException("ContractId is required.");
            }
            try
            {
                ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == restoreToy.ContractId && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("Contract not found.");
                restoreToy.LastUpdatedTime = CoreHelper.SystemTimeNow;

                await _unitOfWork.GetRepository<RestoreToy>().InsertAsync(restoreToy);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<RestoreToy>(restoreToy);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to insert restoreToy. Please try again later.", ex);
            }
        }


        public async Task<bool> Delete(string id)
        {
            try
            {
                RestoreToy restoreToy = _unitOfWork.GetRepository<RestoreToy>().Entities.AsNoTracking().FirstOrDefault(d => d.Id == id && !d.DeletedTime.HasValue)
                    ?? throw new KeyNotFoundException($"Delivery with id {id} not found.");

                restoreToy.DeletedTime = CoreHelper.SystemTimeNow;

                _unitOfWork.GetRepository<RestoreToy>().Update(restoreToy);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete restoreToy. Please try again later.", ex);
            }
        }

        public async Task<RestoreToy> GetById(string id)
        {
            try
            {
                var restoreToy = await _unitOfWork.GetRepository<RestoreToy>().Entities.FirstOrDefaultAsync(x => x.Id == id && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("RestoreToy not found or has been deleted.");

                return _mapper.Map<RestoreToy>(restoreToy);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve restoreToy. Please try again later.", ex);
            }
        }

        public async Task<BasePaginatedList<RestoreToy>> GetPaging(int page, int pageSize, string id, string status)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Invalid page or pageSize value.");
                }

                IQueryable<RestoreToy> query = _unitOfWork.GetRepository<RestoreToy>().Entities
                    .Where(d => !d.DeletedTime.HasValue);

                if (!string.IsNullOrWhiteSpace(id))
                {
                    query = query.Where(d => d.Id.Contains(id));
                }
                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(d => d.Id.Contains(id));
                }

                var totalItems = await query.CountAsync();
                var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                // Pass items directly into the constructor instead of `paginatedRestore`
                return new BasePaginatedList<RestoreToy>(items, totalItems, page, pageSize);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while fetching paginated restoreToy.", ex);
            }
        }


        public async Task Update(string id, UpdateRestoreModel restoreToy)
        {
            try
            {
                var existingRestoreToy = _unitOfWork.GetRepository<RestoreToy>().Entities.AsNoTracking().FirstOrDefault(d => d.Id == id)
                    ?? throw new KeyNotFoundException("RestoreToy not found.");

                existingRestoreToy = _mapper.Map<RestoreToy>(restoreToy);
                existingRestoreToy.Id = id;
                existingRestoreToy.LastUpdatedTime = CoreHelper.SystemTimeNow;

                _unitOfWork.GetRepository<RestoreToy>().Update(existingRestoreToy);

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


		public async Task<RestoreToy> GetByContractId(string contractId)
		{
			try
			{
				var existingRestoreToy = await _unitOfWork.GetRepository<RestoreToy>().Entities.AsNoTracking().FirstOrDefaultAsync(d => d.ContractId.Equals(contractId) && !d.DeletedTime.HasValue)
					?? throw new KeyNotFoundException("RestoreToy not found.");

				return existingRestoreToy;
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

