using Commons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.ViewModels.Account;
using Models.ViewModels.Home;

namespace Service.UserArea.Interface
{
    public interface ISocialMediaConnectionService
    {
        bool InsertSocialMediaConnections(List<string> FriendsList, string ProviderKey, string LoginProvider);

    }
}
