using System.Collections.Generic;
using System.Threading.Tasks;
using RoadState.BusinessLayer.Shared.TransportModels;

namespace RoadState.BusinessLayer.Shared.Interfaces
{
    public interface IUserRepository
    {
     /// <summary>
        /// Retrieves a single user by searching for a given <paramref name="id" />.
        /// </summary>
        /// <param name="id">The database id of the user.</param>
        /// <returns>The user information or <c>null</c> if the user wasn't found.</returns>
        Task<UserTransportModel> GetUserByIdAsync(int id);

        /// <summary>
        /// Retrieves a list of all users currently stored in the database.
        /// </summary>
        /// <returns>The list of users.</returns>
        Task<IEnumerable<UserTransportModel>> GetUsersAsync();

        /// <summary>
        /// Checks if a given <paramref name="userName" />-<paramref name="passHash" />-combination is valid.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <param name="passHash">The password hash to use for the check.</param>
        /// <returns>An enum showing the result of the operation.</returns>
        Task<string> IsPassCorrectAsync(string userName, string passHash);

        /// <summary>
        /// Checks if a given <paramref name="userName" /> exists in the store.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns><c>true</c> if the user was found otherwise <c>false</c>.</returns>
        Task<bool> UserExistsAsync(string userName);
    }
}
