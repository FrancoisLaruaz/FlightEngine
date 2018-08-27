using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.Class.UserFollow;
using System.Configuration;

namespace Service.UserArea
{
    public  class UserFollowService : IUserFollowService
    {

        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<UserFollow> _userFollowRepo;

        private static string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        public UserFollowService(IGenericRepository<User> userRepo, IGenericRepository<UserFollow> userFollowRepo)
        {
            _userRepo = userRepo;
            _userFollowRepo = userFollowRepo;
        }


        public ToggleUserFollowResult ToggleUserFollow(int UserId, int UserToFollowId)
        {
            ToggleUserFollowResult result = new ToggleUserFollowResult();
            try
            {
                var existingData = _userFollowRepo.FindAllBy(u => u.UserId == UserId && u.FollowedUserId == UserToFollowId).FirstOrDefault();
                if (existingData == null)
                {
                    UserFollow follow = new UserFollow();
                    follow.CreationDate = DateTime.UtcNow;
                    follow.UserId = UserId;
                    follow.FollowedUserId = UserToFollowId;
                    result.Followed = true;
                    _userFollowRepo.Add(follow);
                }
                else
                {
                    result.Followed = false;
                    _userFollowRepo.Delete(existingData);
                }
                result.Result = _userFollowRepo.Save();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId + " and UserToFollowId = " + UserToFollowId);
            }
            return result;
        }


        public UserFollowItem TransformUserFollowIntoUserFollowItem(User user)
        {
            UserFollowItem result = new UserFollowItem();
            try
            {
                if (user != null)
                {
                    result.UserId = user.Id;
                    result.FirstNameUser = user.FirstName;
                    result.LastNameUser =user.LastName;
                    result.PictureSrcUser = user.PictureSrc;
                    result.Url = WebsiteURL + "/MyProfile/" + user.Id;
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " Id = " + user.Id);
            }
            return result;
        }

        public List<UserFollowItem> GetUsersFollowedByUser(int UserId)
        {
            List<UserFollowItem> result = new List<UserFollowItem>();
            try
            {
                List<UserFollow> UsersFollow = _userFollowRepo.FindAllBy(u => u.UserId == UserId).ToList();
                foreach (UserFollow elem in UsersFollow)
                {
                    UserFollowItem item = TransformUserFollowIntoUserFollowItem(elem.User);
                    if (item != null)
                    {
                        result.Add(item);
                    }
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }


        public List<UserFollowItem> GetUsersFollowingUser(int UserId)
        {
            List<UserFollowItem> result = new List<UserFollowItem>();
            try
            {
                List<UserFollow> UsersFollow = _userFollowRepo.FindAllBy(u => u.FollowedUserId == UserId).ToList();
                foreach (UserFollow elem in UsersFollow)
                {
                    UserFollowItem item = TransformUserFollowIntoUserFollowItem(elem.User1);
                    if (item != null)
                    {
                        result.Add(item);
                    }
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }
    }
}
