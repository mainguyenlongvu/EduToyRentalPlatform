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
                throw new Exception("Please enter toy name!");
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
                throw new Exception("Entered name already exists!");
            }

            // Lưu toy vào DB
            Toy newToy = _mapper.Map<Toy>(model);
            newToy.CreatedTime = CoreHelper.SystemTimeNows;
            newToy.DeletedTime = null;
            newToy.LastUpdatedBy = null;

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
                ?? throw new Exception("The Toy cannot be found!");

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


        public async Task<BasePaginatedList<ResponeToyModel>> GetToysAsync(int pageNumber, int pageSize, bool? sortByName)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            IQueryable<Toy> toysQuery = _unitOfWork.GetRepository<Toy>().Entities
                .Where(p => !p.DeletedTime.HasValue) // Những đồ chơi chưa có thời gian xoá trong DB (tức chưa được xoá) thì xuất hiện ngược lại thì không
                .OrderByDescending(p => p.CreatedTime);

            // Sort by name if specified
            if (sortByName.HasValue)
            {
                toysQuery = sortByName.Value
                    ? toysQuery.OrderBy(p => p.ToyName)
                    : toysQuery.OrderByDescending(p => p.ToyName);
            }

            // Total count of items
            int totalCount = await toysQuery.CountAsync();

            // Apply pagination
            List<Toy> paginatedProducts = await toysQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to response models
            IReadOnlyCollection<ResponeToyModel> responseItems = _mapper.Map<IReadOnlyCollection<ResponeToyModel>>(paginatedProducts);

            return new BasePaginatedList<ResponeToyModel>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponeToyModel> GetToyAsync(string id)
        {
            Toy toy = await _unitOfWork.GetRepository<Toy>()
                .Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("The Prodcut can not found!");
            return _mapper.Map<ResponeToyModel>(toy);

        }

        public async Task<bool> UpdateToyAsync(string id, UpdateToyModel model)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("The Toy cannot be found!");

            // Kiểm tra tên không được để trống
            if (string.IsNullOrWhiteSpace(model.ToyName))
            {
                throw new Exception("Please enter toy name!");
            }

            // Cập nhật và lưu sản phẩm vào db
            toy.ToyName = model.ToyName;
            toy.ToyDescription = model.ToyDescription;
            toy.ToyPriceSale = model.ToyPriceSale;
            toy.ToyPriceRent = model.ToyPriceRent;
            toy.ToyRemainingQuantity = model.ToyRemainingQuantity;
            toy.ToyImg = model.ToyImg == null?toy.ToyImg:model.ToyImg;
            toy.ToyQuantitySold = model.ToyQuantitySold;
            toy.Option = model.option;
            toy.LastUpdatedTime = CoreHelper.SystemTimeNows;
            //Cập nhật bởi, bạn làm phần login chưa xong
            //toy.LastUpdatedBy = UserId;
             _unitOfWork.GetRepository<Toy>().Update(toy);
            await _unitOfWork.SaveAsync();

            // Return success as true
            return true;
        }

    }
}
