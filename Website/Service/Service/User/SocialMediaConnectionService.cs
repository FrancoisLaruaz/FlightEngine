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


namespace Service.UserArea
{
    public  class SocialMediaConnectionService : ISocialMediaConnectionService
    {

        private readonly IGenericRepository<SocialMediaConnection> _socialMediaRepo;

        public SocialMediaConnectionService(IGenericRepository<SocialMediaConnection> socialMediaRepo)
        {
            _socialMediaRepo = socialMediaRepo;
        }

        /// <summary>
        /// Insert connections between users of the website
        /// </summary>
        /// <param name="FriendsList"></param>
        /// <param name="ProviderKey"></param>
        /// <param name="LoginProvider"></param>
        /// <returns></returns>
        public  bool InsertSocialMediaConnections(List<string> FriendsList, string ProviderKey, string LoginProvider)
        {
            bool Result = true;
            try
            {
                var SocialMediaConnections = _socialMediaRepo.FindAllBy(s => s.ProviderKeyUserSignedUp == ProviderKey).ToList();
                foreach (var connection in SocialMediaConnections)
                {
                    _socialMediaRepo.Delete(connection);
                }

                if (FriendsList != null && FriendsList.Count > 0)
                {
                    foreach (string Friend in FriendsList)
                    {
                        if(!String.IsNullOrWhiteSpace(ProviderKey) && !String.IsNullOrWhiteSpace(LoginProvider) && !String.IsNullOrWhiteSpace(Friend))
                        {
                            SocialMediaConnection connection = new SocialMediaConnection();
                            connection.ProviderKeyUserFriend = Friend;
                            connection.ProviderKeyUserSignedUp = ProviderKey;
                            connection.LoginProvider = LoginProvider;
                            _socialMediaRepo.Edit(connection);
                        }
                    }
                    Result = _socialMediaRepo.Save();
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProviderKey = " + ProviderKey + " and LoginProvider =" + LoginProvider);
            }
            return Result;
        }

    }
}
