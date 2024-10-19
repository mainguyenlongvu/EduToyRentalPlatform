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
            FeedBack feedback = await _unitOfWork.GetRepository<FeedBack>().Entities
                .Include(f => f.User) // Include User details
                .Include(f => f.Toy)  // Include Toy details
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("The feedback cannot be found!");

            // Map feedback data including UserName and ToyName
            var response = new ResponeFeedBackModel
            {
                Id = feedback.Id,
                Content = feedback.Content,
                CreatedTime = feedback.CreatedTime,
                DeletedTime = feedback.DeletedTime,
                LastUpdatedTime = feedback.LastUpdatedTime,
                UserId = feedback.User.UserName, // Assuming User has UserName property
                ToyId = feedback.Toy.ToyName      // Assuming Toy has ToyName property
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
            var responseItems = paginatedFeedbacks.Select(f => new ResponeFeedBackModel
            {
                Id = f.Id,
                Content = f.Content,
                CreatedTime = f.CreatedTime,
                DeletedTime = f.DeletedTime,
                LastUpdatedTime = f.LastUpdatedTime,
                UserId = f.User.UserName, // Assuming the User entity has a Name property
                ToyId = f.Toy.ToyName // Assuming the Toy entity has a ToyName property
            }).ToList();

            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
        }    

        public async Task<BasePaginatedList<ResponeFeedBackModel>> GetFeedBacksAsync(int pageNumber, int pageSize, bool? sortByDate)
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
            var responseItems = paginatedFeedbacks.Select(f => new ResponeFeedBackModel
            {
                Id = f.Id,
                Content = f.Content,
                CreatedTime = f.CreatedTime,
                DeletedTime = f.DeletedTime,
                LastUpdatedTime = f.LastUpdatedTime,
                UserId = f.User.UserName, // Assuming the User entity has a Name property
                ToyId = f.Toy.ToyName // Assuming the Toy entity has a ToyName property
            }).ToList();

            return new BasePaginatedList<ResponeFeedBackModel>(responseItems, totalCount, pageNumber, pageSize);
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
            newFeedBack.UserId = model.UserId;
            newFeedBack.ToyId = model.ToyId;
            newFeedBack.DeletedTime = CoreHelper.SystemTimeNow;
            newFeedBack.CreatedTime = CoreHelper.SystemTimeNow;
            newFeedBack.LastUpdatedTime = CoreHelper.SystemTimeNow;
          
            // Lưu phản hồi vào DB
            //FeedBack newFeedBack = _mapper.Map<FeedBack>(model);
            ////newFeedBack.Id = Guid.NewGuid().ToString("N");
            //newFeedBack.CreatedTime = CoreHelper.SystemTimeNow;
            //newFeedBack.LastUpdatedTime = CoreHelper.SystemTimeNow; ;
            //newFeedBack.IsDeleted = false;
            //newFeedBack.DeletedTime = null;
            //newFeedBack.LastUpdatedBy = null;

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
    }
}
