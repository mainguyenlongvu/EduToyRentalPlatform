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

        public async Task<ResponeFeedBackModel> GetFeedBackAsync(string id)
        {
            // Retrieve feedback with related User and Toy details
            var feedback = await _unitOfWork.GetRepository<FeedBack>().Entities
                .Include(f => f.User) // Include User details
                .Include(f => f.Toy)  // Include Toy details
                .FirstOrDefaultAsync(p => p.Id == id);

            // Check if feedback is null
            if (feedback == null)
            {
                throw new KeyNotFoundException($"Feedback with ID '{id}' cannot be found."); // More specific exception
            }

            // Map feedback data including UserName and ToyName
            var response = new ResponeFeedBackModel
            {
                Id = feedback.Id,
                Content = feedback.Content,
                CreatedTime = feedback.CreatedTime,
                DeletedTime = feedback.DeletedTime,
                LastUpdatedTime = feedback.LastUpdatedTime,
                UserId = feedback.User?.UserName ?? "Unknown", // Safely access UserName
                ToyId = feedback.Toy?.ToyName ?? "Unknown"     // Safely access ToyName
            };

            return response;
        }


        public async Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacks_AdminAsync(int pageNumber, int pageSize, bool? sortByDate)
        {
            IQueryable<FeedBack> feedbacksQuery = _unitOfWork.GetRepository<FeedBack>().Entities
                .Include(f => f.User) // Include User details
                .Include(f => f.Toy)  // Include Toy details
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            // Sort by CreatedTime if specified
            if (sortByDate.HasValue)
            {
                feedbacksQuery = sortByDate.Value ? feedbacksQuery.OrderBy(p => p.CreatedTime) : feedbacksQuery.OrderByDescending(p => p.CreatedTime);
            }

            // Count total items
            int totalCount = await feedbacksQuery.CountAsync();

            // Apply pagination
            List<FeedBack> paginatedFeedbacks = await feedbacksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to response model
            // Map to response model
            var responseItems = paginatedFeedbacks.Select(f => new ResponeFeedBackModel
            {
                Id = f.Id,
                Content = f.Content,
                CreatedTime = f.CreatedTime,
                DeletedTime = f.DeletedTime,
                LastUpdatedTime = f.LastUpdatedTime,
                UserId = f.User != null ? f.User.FullName : "Unknown", // Null check for User
                ToyId = f.Toy != null ? f.Toy.ToyName : "Unknown"     // Null check for Toy
            }).ToList();


            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacksAsync(int pageNumber, int pageSize, bool? sortByDate)
        {
            // Validate pagination parameters
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be greater than or equal to 1.", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than or equal to 1.", nameof(pageSize));

            IQueryable<FeedBack> feedbacksQuery = _unitOfWork.GetRepository<FeedBack>().Entities
                .Include(f => f.User) // Include User details
                .Include(f => f.Toy)  // Include Toy details
                .OrderByDescending(p => p.CreatedTime);

            // Sort by CreatedTime if specified
            if (sortByDate.HasValue)
            {
                feedbacksQuery = sortByDate.Value ? feedbacksQuery.OrderBy(p => p.CreatedTime) : feedbacksQuery.OrderByDescending(p => p.CreatedTime);
            }

            // Count total items
            int totalCount = await feedbacksQuery.CountAsync();

            // Apply pagination
            List<FeedBack> paginatedFeedbacks = await feedbacksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to response model
            var responseItems = paginatedFeedbacks.Select(f => new ResponeFeedBackModel
            {
                Id = f.Id,
                Content = f.Content,
                CreatedTime = f.CreatedTime,
                DeletedTime = f.DeletedTime,
                LastUpdatedTime = f.LastUpdatedTime,
                UserId = f.User?.FullName ?? "Unknown", // Use null-conditional operator
                ToyId = f.Toy?.ToyName ?? "Unknown"     // Use null-conditional operator
            }).ToList();

            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
        }


        public async Task<BasePaginatedList<ResponeFeedBackModel>> SearchFeedBacksAsync(int pageNumber, int pageSize, string? content, Guid? userId)
        {
            IQueryable<FeedBack> feedbacksQuery = _unitOfWork.GetRepository<FeedBack>().Entities
                .Where(p => !p.DeletedTime.HasValue);

            // Tìm theo nội dung phản hồi
            if (!string.IsNullOrWhiteSpace(content))
            {
                feedbacksQuery = feedbacksQuery.Where(fb => fb.Content.Contains(content));
            }

            // Tìm theo UserID
            if (userId.HasValue)
            {
                Guid? userGuid = userId;
                feedbacksQuery = feedbacksQuery.Where(fb => fb.UserId == userGuid);
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

        public async Task<bool> CreateFeedBackAsync(CreateFeedBackModel model)
        {
            // Kiểm tra nội dung phản hồi không được để trống
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new Exception("Please enter feedback content!");
            }

            FeedBack newFeedBack = _mapper.Map<FeedBack>(model);


            newFeedBack.Id = Guid.NewGuid().ToString("N");
            newFeedBack.UserId = Guid.Parse(model.UserId);
            newFeedBack.ToyId = model.ToyId;
            newFeedBack.CreatedTime = CoreHelper.SystemTimeNow;
            newFeedBack.LastUpdatedTime = CoreHelper.SystemTimeNow;
          
            

            await _unitOfWork.GetRepository<FeedBack>().InsertAsync(newFeedBack);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateFeedBackAsync(string id, ResponeFeedBackModel model)
        {
            // Lấy phản hồi - kiểm tra sự tồn tại
            FeedBack feedback = await _unitOfWork.GetRepository<FeedBack>().Entities.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("The feedback cannot be found!");

            // Kiểm tra nội dung không được để trống
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                throw new Exception("Please enter feedback content!");
            }

            // Cập nhật các thuộc tính
            feedback.LastUpdatedTime = CoreHelper.SystemTimeNow;
            feedback.Content = model.Content;

            // Thực hiện cập nhật
            _unitOfWork.GetRepository<FeedBack>().Update(feedback);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteFeedBackAsync(string id)
        {
            // get feedback
            FeedBack? feedback = await _unitOfWork.GetRepository<FeedBack>().Entities.FirstOrDefaultAsync(p => p.Id == id);

            // Check if feedback does not exist
            if (feedback == null)
            {
                throw new Exception("The feedback cannot be found!");
            }

            // Check if feedback has been deleted
            if (feedback.DeletedTime.HasValue)
            {
                throw new Exception("The feedback has already been deleted!");
            }

            // Soft delete
            feedback.DeletedTime = CoreHelper.SystemTimeNow;
            feedback.DeletedTime = CoreHelper.SystemTimeNow;
            _unitOfWork.GetRepository<FeedBack>().Update(feedback);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacksByToyIdAsync(string toyId, int pageNumber, int pageSize, bool? sortByDate)
        {
            IQueryable<FeedBack> feedbacksQuery = _unitOfWork.GetRepository<FeedBack>().Entities
                .Include(f => f.User)
                .Include(f => f.Toy)
                .Where(p => !p.DeletedTime.HasValue && p.ToyId == toyId) // Only include feedbacks that are not deleted
                .OrderByDescending(p => p.CreatedTime);

            // Sort by CreatedTime if specified
            if (sortByDate.HasValue)
            {
                feedbacksQuery = sortByDate.Value ? feedbacksQuery.OrderBy(p => p.CreatedTime) : feedbacksQuery.OrderByDescending(p => p.CreatedTime);
            }

            // Count total items
            int totalCount = await feedbacksQuery.CountAsync();

            // Apply pagination
            List<FeedBack> paginatedFeedbacks = await feedbacksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to response model
            var responseItems = paginatedFeedbacks.Select(f => new ResponeFeedBackModel
            {
                Id = f.Id,
                Content = f.Content,
                CreatedTime = f.CreatedTime,
                LastUpdatedTime = f.LastUpdatedTime,
                FullName = f.User?.FullName ?? "Unknown",
                ToyId = f.Toy?.ToyName ?? "Unknown"
            }).ToList();

            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
        }

    }
}
