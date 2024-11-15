using AutoMapper;
using System.Data.Entity;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Core.Base;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.RestoreToyDetailModelViews;

namespace ToyShop.Services.Service
{
	public class RestoreToyDetailService : IRestoreToyDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RestoreToyDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RestoreToyDetail> Insert(RestoreToyDetail restoreToyDetail)
        {
            ArgumentNullException.ThrowIfNull(restoreToyDetail);
            if (restoreToyDetail.RestoreToyId == null)
            {
                throw new ArgumentNullException("RestoreToyId is required.");
            }
            try
            {
                RestoreToy restoreToy = await _unitOfWork.GetRepository<RestoreToy>().Entities.FirstOrDefaultAsync(x => x.Id == restoreToyDetail.RestoreToyId && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("RestoreToy not found.");
                restoreToy.LastUpdatedTime = CoreHelper.SystemTimeNow;

                await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
                await _unitOfWork.SaveAsync();

                return restoreToyDetail;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to insert restoreToy detail. Please try again later.", ex);
            }
        }


        public async Task<bool> Delete(string id)
        {
            try
            {
                RestoreToyDetail restoreToyDetail = _unitOfWork.GetRepository<RestoreToyDetail>().Entities.AsNoTracking().FirstOrDefault(d => d.Id == id && !d.DeletedTime.HasValue)
                    ?? throw new KeyNotFoundException($"Delivery with restoreToyId {id} not found.");

                restoreToyDetail.DeletedTime = CoreHelper.SystemTimeNow;

                _unitOfWork.GetRepository<RestoreToyDetail>().Update(restoreToyDetail);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete restoreToy detail. Please try again later.", ex);
            }
        }

        public async Task<RestoreToyDetail> GetById(string id)
        {
            try
            {
                var restoreToyDetail = await _unitOfWork.GetRepository<RestoreToyDetail>().Entities.FirstOrDefaultAsync(x => x.Id == id && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("RestoreToy not found or has been deleted.");

                return _mapper.Map<RestoreToyDetail>(restoreToyDetail);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve restoreToy detail. Please try again later.", ex);
            }
        }

        public async Task<BasePaginatedList<RestoreToyDetail>> GetPaging(int page, int pageSize, string id)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Invalid page or pageSize value.");
                }

                IQueryable<RestoreToyDetail> query = _unitOfWork.GetRepository<RestoreToyDetail>().Entities
                    .Where(d => !d.DeletedTime.HasValue);

                if (!string.IsNullOrWhiteSpace(id))
                {
                    query = query.Where(d => d.Id.Contains(id));
                }

                var totalItems = await query.CountAsync();
                var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                // Pass items directly into the constructor instead of `paginatedRestore`
                return new BasePaginatedList<RestoreToyDetail>(items, totalItems, page, pageSize);
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


        public async Task Update(string id, UpdateRestoreDetailModel restoreToyDetail)
        {
            try
            {
                var existingRestoreToyDetail = _unitOfWork.GetRepository<RestoreToyDetail>().Entities.AsNoTracking().FirstOrDefault(d => d.Id == id)
                    ?? throw new KeyNotFoundException("RestoreToy detail not found.");

                existingRestoreToyDetail = _mapper.Map<RestoreToyDetail>(restoreToyDetail);
                existingRestoreToyDetail.Id = id;
                existingRestoreToyDetail.LastUpdatedTime = CoreHelper.SystemTimeNow;

                _unitOfWork.GetRepository<RestoreToyDetail>().Update(existingRestoreToyDetail);

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

		public async Task<RestoreToyDetail> GetByRestoreToyId(string restoreToyId)
		{
			try
			{
				var restoreToyDetail = await _unitOfWork.GetRepository<RestoreToyDetail>().Entities.FirstOrDefaultAsync(x => x.Id == restoreToyId && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("RestoreToy not found or has been deleted.");

				return _mapper.Map<RestoreToyDetail>(restoreToyDetail);
			}
			catch (KeyNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to retrieve restoreToy detail. Please try again later.", ex);
			}
		}


	}
}
