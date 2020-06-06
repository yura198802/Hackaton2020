using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Monica.Core.DataBaseUtils;

namespace Hackaton.CrmDbModel.IdentityModel
{
    /// <summary>
    /// Конекст данных для работы с БД IS4
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private IDataBaseIs4 _dataBaseMain;

        public ApplicationDbContext(IDataBaseIs4 dataBaseMain)
        {
            _dataBaseMain = dataBaseMain;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_dataBaseMain.ConntectionString);
        }

    }


}
