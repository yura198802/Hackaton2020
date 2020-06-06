using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelCrm.Core;
using Hackaton.CrmDbModel.ModelDto;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.UniversalAdapter.Adapter.User
{
    public class UserAdapter : IUserAdapter
    {
        private WordDbContext _wordDbContext;

        public UserAdapter(WordDbContext wordDbContext)
        {
            _wordDbContext = wordDbContext;
        }

        public async Task<IEnumerable<CrmDbModel.Model.LoadDocument.User>> GetUsers()
        {
            return await _wordDbContext.User.ToListAsync();
        }

        public async Task<ResultCrmDb> AddUser(CrmDbModel.Model.LoadDocument.User user)
        {
            var result = new ResultCrmDb();
            try
            {
                _wordDbContext.Add(user);
                await _wordDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.AddError("-",e.Message);
            }

            return result;
        }

        public async Task<ResultCrmDb> EditUser(CrmDbModel.Model.LoadDocument.User user)
        {
            var result = new ResultCrmDb();
            try
            {
                _wordDbContext.Update(user);
                await _wordDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.AddError("-", e.Message);
            }

            return result;
        }



        public async Task<ResultCrmDb> DeleteUser(int userId)
        {
            var result = new ResultCrmDb();
            try
            {
                var user = await _wordDbContext.User.FirstOrDefaultAsync(f => f.Id == userId);
                _wordDbContext.Remove(user);
                await _wordDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.AddError("-", e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<UserDocumentDto>> GetUserDocument(int userId)
        {
            await _wordDbContext.UserDocument.LoadAsync();
            var query = await _wordDbContext.UserDocument.Where(f => f.UserId == userId).ToListAsync();
            return query.Select(s => new UserDocumentDto
            {
                NameDoc = s.DocumentLoader?.Name,
                Category = s.DocumentLoader?.Category,
                VidDoc = s.DocumentLoader?.VidDoc,
                Id = s.Id
            });
        }

        public async Task<ResultCrmDb> DeleteUserDocument(int userDocId)
        {
            var result = new ResultCrmDb();
            try
            {
                var model = await _wordDbContext.UserDocument.FirstOrDefaultAsync(f => f.Id == userDocId);
                _wordDbContext.Remove(model);
                await _wordDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.AddError("-", e.Message);
            }

            return result;
        }
        public async Task<ResultCrmDb> AddUserDocument(int docId, int userId)
        {
            var result = new ResultCrmDb();
            try
            {
                var model = new UserDocument() { DocumentLoaderId = docId, UserId = userId};
                _wordDbContext.Add(model);
                await _wordDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.AddError("-", e.Message);
            }

            return result;
        }

    }
}
