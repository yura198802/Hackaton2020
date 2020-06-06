using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.ModelCrm.Core;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.UniversalAdapter.Adapter.User
{
    public interface IUserAdapter
    {
        Task<IEnumerable<CrmDbModel.Model.LoadDocument.User>> GetUsers();
        Task<ResultCrmDb> AddUser(CrmDbModel.Model.LoadDocument.User user);
        Task<ResultCrmDb> EditUser(CrmDbModel.Model.LoadDocument.User user);
        Task<ResultCrmDb> DeleteUser(int userId);
        Task<IEnumerable<UserDocumentDto>> GetUserDocument(int userId);
        Task<ResultCrmDb> DeleteUserDocument(int userDocId);
        Task<ResultCrmDb> AddUserDocument(int docId, int userId);


    }
}
