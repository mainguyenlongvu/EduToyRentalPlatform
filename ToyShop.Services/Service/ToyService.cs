using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.ToyModelViews;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;


namespace ToyShop.Services.Service
{
    public class ToyService : IToyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ToyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateToyAsync(CreateToyModel model)
        {
            // Kiểm tra tên không được để trống
            if (string.IsNullOrWhiteSpace(model.ToyName))
            {
                throw new Exception("Vui lòng nhập tên đồ chơi!");
            }

            // Kiểm tra sản phẩm đã tồn tại hay chưa
            var toys = await _unitOfWork.GetRepository<Toy>().Entities
                .Where(t => t.DeletedTime == null)
                .ToListAsync();

            bool isExistProduct = toys.Any(t =>
                model.ToyName.Equals(t.ToyName, StringComparison.OrdinalIgnoreCase) &&
                !t.DeletedTime.HasValue);

            if (isExistProduct)
            {
                throw new Exception("Tên đã tồn tại!");
            }

            // Lưu toy vào DB
            Toy newToy = _mapper.Map<Toy>(model);
            newToy.CreatedTime = CoreHelper.SystemTimeNows;
            newToy.DeletedTime = null;
            newToy.LastUpdatedBy = null;
            //newToy.ToyImg = await FileUploadHelper.UploadFile(model.ImageFile);

            await _unitOfWork.GetRepository<Toy>().InsertAsync(newToy);
            await _unitOfWork.SaveAsync();

            // Return success as true
            return true;
        }


        public async Task<bool> DeleteToyAsync(string id)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("Không tìm thấy đồ chơi!");

            // Xóa mềm
            toy.DeletedTime = CoreHelper.SystemTimeNow;
            toy.DeletedTime = CoreHelper.SystemTimeNows;
            //Xoá bởi, bạn làm phần login chưa xong
            //toy.DeletedBy = UserId;
            _unitOfWork.GetRepository<Toy>().Update(toy);
            await _unitOfWork.SaveAsync();

            // Return success as true
            return true;
        }


        public async Task<BasePaginatedList<ResponeToyModel>> GetToysAsync(int pageNumber, int pageSize, bool? sortByName, string searchTerm)
        {
            // Ensure page number and page size are valid
            pageNumber = Math.Max(1, pageNumber); // Use Math.Max for cleaner code
            pageSize = Math.Max(10, pageSize);

            // Build the base query
            IQueryable<Toy> toysQuery = _unitOfWork.GetRepository<Toy>().Entities
                .Where(p => !p.DeletedTime.HasValue); // Exclude soft-deleted toys

            // Apply search filter if a search term is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                toysQuery = toysQuery.Where(p => p.ToyName.Contains(searchTerm)); // Filter by ToyName
            }

            // Apply sorting if specified
            if (sortByName.HasValue)
            {
                toysQuery = sortByName.Value
                    ? toysQuery.OrderBy(p => p.ToyName) // Sort ascending
                    : toysQuery.OrderByDescending(p => p.ToyName); // Sort descending
            }
            else
            {
                // If no sorting option is provided, keep the default sort
                toysQuery = toysQuery.OrderByDescending(p => p.CreatedTime);
            }

            // Get total count of items after filtering
            int totalCount = await toysQuery.CountAsync();

            // Apply pagination
            List<Toy> paginatedToys = await toysQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to response models
            IReadOnlyCollection<ResponeToyModel> responseItems = _mapper.Map<IReadOnlyCollection<ResponeToyModel>>(paginatedToys);

            // Create and return paginated response
            return new BasePaginatedList<ResponeToyModel>(responseItems, totalCount, pageNumber, pageSize);
        }


        public async Task<ResponeToyModel> GetToyAsync(string id)
        {
            Toy toy = await _unitOfWork.GetRepository<Toy>()
                .Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("Không tìm thấy đồ chơi!");
            return _mapper.Map<ResponeToyModel>(toy);

        }

        public async Task<bool> UpdateToyAsync(string id, UpdateToyModel model)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("Không tìm thấy đồ chơi!");

            // Kiểm tra tên không được để trống
            if (string.IsNullOrWhiteSpace(model.ToyName))
            {
                throw new Exception("Vui lòng nhập tên đồ chơi!");
            }

            // Cập nhật và lưu sản phẩm vào db
            toy.ToyName = model.ToyName;
            toy.ToyDescription = model.ToyDescription;
            toy.ToyPriceSale = model.ToyPriceSale;
            toy.ToyPriceRent = model.ToyPriceRent;
            toy.ToyRemainingQuantity = model.ToyRemainingQuantity;
            toy.ToyImg = model.ToyImg == null?toy.ToyImg:model.ToyImg;
            toy.ToyQuantitySold = model.ToyQuantitySold;
            toy.Option = model.Option;
            toy.LastUpdatedTime = CoreHelper.SystemTimeNows;
            //toy.ToyImg = await FileUploadHelper.UploadFile(model.Image);
            //Cập nhật bởi, bạn làm phần login chưa xong
            //toy.LastUpdatedBy = UserId;
             _unitOfWork.GetRepository<Toy>().Update(toy);
            await _unitOfWork.SaveAsync();

            // Return success as true
            return true;
        }

    }
}
