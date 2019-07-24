using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoadState.BusinessLayer.Shared.Interfaces;
using RoadState.BusinessLayer.Shared.TransportModels;

namespace RoadState.BusinessLayer.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        #region explicit interfaces

        /// <inheritdoc />
        public async Task<int?> AddUserAsnyc(UserTransportModel model, string firstRoleName)
        {
            var newUser = model.ToEntity();
            newUser.EmailConfirmed = true;
            newUser.AccessFailedCount = 0;
            newUser.LockoutEnabled = false;
            newUser.LockoutEndDateUtc = null;
            newUser.SecurityStamp = Guid.NewGuid().ToString("N");
            DbContext.Users.Add(newUser);
            try
            {
                await DbContext.SaveChangesAsync();
                if (newUser.Id == 0)
                {
                    await DbContext.Entry(newUser).ReloadAsync();
                }
                return newUser.Id;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<UserTransportModel> GetUserByIdAsync(int id)
        {
            var user = await DbContext.Users.FindAsync(id).ConfigureAwait(false);
            return user?.ToTransportModel();
        }
        /// <inheritdoc />
        public async Task<IEnumerable<UserTransportModel>> GetUsersAsync()
        {
            var users = await DbContext.Users.ToListAsync().ConfigureAwait(false);
            return users.Select(u => u.ToTransportModel()).ToList();
        }

        /// <inheritdoc />
        public async Task<string> IsPassCorrectAsync(string userName, string passHash)
        {
            var user = await GetUserByUserNameAsync(userName).ConfigureAwait(false);
            if (user == null)
            {
                return PasswordCheckResult.UserNotFound;
            }
            if (!user.EmailConfirmed)
            {
                return PasswordCheckResult.UserNotConfirmed;
            }
            if (user.LockoutEnabled)
            {
                return PasswordCheckResult.UserIsLocked;
            }
            if (!user.PasswordHash.Equals(passHash, StringComparison.Ordinal))
            {
                return PasswordCheckResult.PasswordIncorrect;
            }
            return PasswordCheckResult.Success;
        }

        /// <inheritdoc />
        public async Task<bool> UserExistsAsync(string userName)
        {
            return await DbContext.Users.AnyAsync(u => u.UserName.Equals(userName, StringComparison.Ordinal)).ConfigureAwait(false);
        }

        #endregion
    }
}
