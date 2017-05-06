using LTHT.PeopleManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LTHT.PeopleManagement.Services
{
    /// <summary>
    /// Interface for the Person Service
    /// </summary>
    /// <remarks>Facilitates IoC and therefore simplifes testing</remarks>
    public interface IPersonService
    {
        /// <summary>
        /// Add a <see cref="Person"/>
        /// </summary>
        /// <param name="person">The person to add</param>
        /// <returns>Updated object</returns>
        Task<Person> AddPersonAsync(Person person);

        /// <summary>
        /// Update a <see cref="Person"/>
        /// </summary>
        /// <param name="person">The person to update</param>
        /// <returns>Updated object</returns>
        Task<Person> UpdatePersonAsync(Person person);

        /// <summary>
        /// Delete a <see cref="Person"/>
        /// </summary>
        /// <param name="person">The person to delete</param>
        Task DeletePersonAsync(Person person);

        /// <summary>
        /// Get a specific <see cref="Person"/>
        /// </summary>
        /// <param name="id">Identifier of the <see cref="Person"/> to fetch</param>
        /// <returns>The requested <see cref="Person"/></returns>
        Task<Person> GetPersonAsync(int id);

        /// <summary>
        /// Get all <see cref="Person"/>
        /// </summary>
        /// <param name="filter">Optional filter applied to the name</param>
        /// <returns>A list of Person</returns>
        /// <remarks>Paging required for realistic data</remarks>
        Task<IEnumerable<Person>> GetPersonsAsync(string filter = null);
    }
}