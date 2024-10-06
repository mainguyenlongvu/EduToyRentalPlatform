using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;

namespace ToyShop.Services.Service
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeedBackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponeFeedBackModel> CreateFeedBackAsync(CreateFeedBackModel model)
        {
            // Kiểm tra nội dung phản hồi không được để trống
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new Exception("Please enter feedback content!");
            }

            // Lưu phản hồi vào DB
            FeedBack newFeedBack = _mapper.Map<FeedBack>(model);
            newFeedBack.CreatedTime = CoreHelper.SystemTimeNow;
            newFeedBack.DeletedTime = null;
            await _unitOfWork.GetRepository<FeedBack>().InsertAsync(newFeedBack);
            await _unitOfWork.SaveAsync();

            FeedBack? feedback = await _unitOfWork.GetRepository<FeedBack>().Entities.FirstOrDefaultAsync(p => p.Content == model.Content);
            return _mapper.Map<ResponeFeedBackModel>(feedback);
        }

        public async Task<ResponeFeedBackModel> DeleteFeedBackAsync(string id)
        {
            // Lấy phản hồi - kiểm tra sự tồn tại
            FeedBack feedback = await _unitOfWork.GetRepository<FeedBack>().Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("The feedback cannot be found!");

            // Xóa mềm
            feedback.DeletedTime = CoreHelper.SystemTimeNow;
            feedback.DeletedTime = CoreHelper.SystemTimeNow;
            _unitOfWork.GetRepository<FeedBack>().Update(feedback);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ResponeFeedBackModel>(feedback);
        }

        public async Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacksAsync(int pageNumber, int pageSize, bool? sortByDate)
        {
            IQueryable<FeedBack> feedbacksQuery = _unitOfWork.GetRepository<FeedBack>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            // Sắp xếp theo CreatedTime
            if (sortByDate.HasValue)
            {
                feedbacksQuery = sortByDate.Value ? feedbacksQuery.OrderBy(p => p.CreatedTime) : feedbacksQuery.OrderByDescending(p => p.CreatedTime);
            }

            // Tổng số phần tử
            int totalCount = await feedbacksQuery.CountAsync();

            // Áp dụng pagination
            List<FeedBack> paginatedFeedbacks = await feedbacksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            IReadOnlyCollection<ResponeFeedBackModel> responseItems = _mapper.Map<IReadOnlyCollection<ResponeFeedBackModel>>(paginatedFeedbacks);
            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponeFeedBackModel> GetFeedBackAsync(string id)
        {
            FeedBack feedback = await _unitOfWork.GetRepository<FeedBack>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("The feedback cannot be found!");
            return _mapper.Map<ResponeFeedBackModel>(feedback);
        }

        public async Task<BasePaginatedList<ResponeFeedBackModel>> SearchFeedBacksAsync(int pageNumber, int pageSize, string? content, string? userId)
        {
            IQueryable<FeedBack> feedbacksQuery = _unitOfWork.GetRepository<FeedBack>().Entities
                .Where(p => !p.DeletedTime.HasValue);

            // Tìm theo nội dung phản hồi
            if (!string.IsNullOrWhiteSpace(content))
            {
                feedbacksQuery = feedbacksQuery.Where(fb => fb.Content.Contains(content));
            }

            // Tìm theo UserID
            if (!string.IsNullOrWhiteSpace(userId))
            {
                string userGuid = userId;
                feedbacksQuery = feedbacksQuery.Where(fb => fb.CreatedBy == userGuid);
            }

            // Tổng số phần tử
            int totalCount = await feedbacksQuery.CountAsync();

            // Áp dụng pagination
            List<FeedBack> paginatedFeedbacks = await feedbacksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            IReadOnlyCollection<ResponeFeedBackModel> responseItems = _mapper.Map<IReadOnlyCollection<ResponeFeedBackModel>>(paginatedFeedbacks);
            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponeFeedBackModel> UpdateFeedBackAsync(string id, CreateFeedBackModel model)
        {
            // Lấy phản hồi - kiểm tra sự tồn tại
            FeedBack feedback = await _unitOfWork.GetRepository<FeedBack>().Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new Exception("The feedback cannot be found!");

            // Kiểm tra nội dung không được để trống
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new Exception("Please enter feedback content!");
            }

            // Cập nhật và lưu phản hồi vào db
            _mapper.Map(model, feedback);
            feedback.LastUpdatedTime = CoreHelper.SystemTimeNow;
            _unitOfWork.GetRepository<FeedBack>().Update(feedback);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ResponeFeedBackModel>(feedback);
        }
    }
}
