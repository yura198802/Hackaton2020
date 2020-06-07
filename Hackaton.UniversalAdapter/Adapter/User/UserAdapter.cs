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
                if (string.IsNullOrWhiteSpace(user.Account))
                    return result;


                _wordDbContext.Add(user);
                await _wordDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.AddError("-", e.Message);
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

        public async Task<CrmDbModel.Model.LoadDocument.User> GetUserModel(int userId)
        {
            return await _wordDbContext.User.FirstOrDefaultAsync(f => f.Id == userId);
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
            return await _wordDbContext.UserDocument.Where(f => f.UserId == userId).Join(_wordDbContext.DocumentLoader, document => document.DocumentLoaderId,
                loader => loader.Id, (document, loader) => new UserDocumentDto()
                {
                    ProfId = loader.Id,
                    ProfName = loader.ProfName,
                    ProfDoc = loader.Name,
                    Id = document.Id
                }).ToListAsync();
        }

        public async Task<IEnumerable<DocumentLoader>> GetDocumentLoader(int? userId)
        {
            if (userId != null)
            {
                return _wordDbContext.DocumentLoader.GroupJoin(
                    _wordDbContext.UserDocument,
                    document => document.Id,
                    userDoc => userDoc.DocumentLoaderId, (doc, uDoc) => new
                    {
                        doc,
                        uDoc

                    }).SelectMany
                (
                    args => args.uDoc.DefaultIfEmpty(),
                    (doc, udoc) => new
                    {
                        doc,
                        IsVisible = udoc == null
                    }
                ).Where(f => f.IsVisible).Select(s => s.doc.doc);
            }
            return await _wordDbContext.DocumentLoader.ToListAsync();
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
                var model = await _wordDbContext.UserDocument.FirstOrDefaultAsync(f => f.UserId == userId && f.DocumentLoaderId == docId);
                if (model != null)
                    return result;
                model = new UserDocument() { DocumentLoaderId = docId, UserId = userId };
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
