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
using Models.Class.UserFollow;

namespace Service.UserArea.Interface
{
    public interface IUserFollowService
    {

        ToggleUserFollowResult ToggleUserFollow(int UserId, int UserToFollowId);

        List<UserFollowItem> GetUsersFollowedByUser(int UserId);

        List<UserFollowItem> GetUsersFollowingUser(int UserId);

    }
}
