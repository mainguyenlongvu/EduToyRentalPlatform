using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.RestoreToyModelViews;

namespace ToyShop.Services.Service
{
    public interface IRestoreToyService
    {
        public Task Update(string id, UpdateRestoreModel restoreToy);
        public Task<BasePaginatedList<RestoreToy>> GetPaging(int page, int pageSize, string id, string status);
        public Task<RestoreToy> GetById(string id);
        public Task<bool> Delete(string id);
        public Task<RestoreToy> Insert(RestoreToy restoreToy);
		public Task<RestoreToy> GetByContractId(string contractId);


	}
}
