using Chenyuan.Data.Base.Entity;
using Chenyuan.Lottery.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Lottery.Services
{
    public class AccountService : IAccountService
    {
        //AccountInfoRepository
        public AccountInfo GetAccountInfo(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Login(string account, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
