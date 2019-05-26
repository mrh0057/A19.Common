using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mrh.Database
{
    /// <summary>
    ///     The id of the user id.
    /// </summary>
    /// <typeparam name="TUserId">The type of the user.</typeparam>
    /// <typeparam name="TKey">The  type for the key in the database.</typeparam>
    public interface IDiffRepository<TUserId, TKey> 
    {
        
        /// <summary>
        /// Used to insert the new record in the database.
        /// </summary>
        /// <param name="records">The records to update in the database.</param>
        /// <param name="userId">The id of the user who is making the request.</param>
        /// <returns>The task that does the database.</returns>
        Task Add(IEnumerable<AbstractDatabaseRecord<TKey>> records, TUserId userId);

        /// <summary>
        /// Used to update the database with the update record.
        /// </summary>
        /// <param name="records">The records for the database.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The task that does the database.</returns>
        Task Update(IEnumerable<AbstractDatabaseRecord<TKey>> records, TUserId userId);
        
        /// <summary>
        ///  Used to delete the records.
        /// </summary>
        /// <param name="records">The records to update in the database.</param>
        /// <param name="userId">The id of the user who is updating the database.</param>
        /// <returns>The task that updates the database.</returns>
        Task Delete(IEnumerable<AbstractDatabaseRecord<TKey>> records, TUserId userId);
    }
}