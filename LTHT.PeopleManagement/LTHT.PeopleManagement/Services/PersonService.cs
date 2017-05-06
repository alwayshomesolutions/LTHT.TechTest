using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LTHT.PeopleManagement.Models;

using Xamarin.Forms;
using LTHT.PeopleManagement.Helpers;
using System.Threading;

[assembly: Dependency(typeof(LTHT.PeopleManagement.Services.PersonService))]
namespace LTHT.PeopleManagement.Services
{
    /// <summary>
    /// Basic implementation of PersonService.
    /// </summary>
    /// <remarks>
    /// 1. Robust error handling needed
    /// 2. Connectivity checks for network coverage required
    /// 3. IoC registration required (elsewhere) using an appropriate IoC framework
    /// 4. IMPORTANT: Unlikely to work using stock Android emulator with host using WiFi.
    /// </remarks>
    public class PersonService : IPersonService
    {
        /// <summary>
        /// Base Url for LTHT Test service
        /// </summary>
        private const string BaseUrl = "http://ltht-tech-test.azurewebsites.net/api/people";

        public async Task<Person> AddPersonAsync(Person person)
        {
            return await WebQuery.PostJson<Person>(new Uri($"{BaseUrl}"), new CancellationTokenSource().Token, person);
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            return await WebQuery.PutJson<Person>(new Uri($"{BaseUrl}/{person.Id}"), new CancellationTokenSource().Token, person);
        }

        public async Task DeletePersonAsync(Person person)
        {
            await WebQuery.DeleteJson<Person>(new Uri($"{BaseUrl}/{person.Id}"), new CancellationTokenSource().Token);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            // Not needed for the test
            // NOTE: The corresponding API is returning a HTTP 500 when the id is not found.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of <see cref="Person"/> from the service
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of <see cref="Person"/></returns>
        /// <remarks>
        /// NOTE: In reality, this API should be filtering and paging on the server to cater for realistic data sets
        /// </remarks>
        public async Task<IEnumerable<Person>> GetPersonsAsync(string filter = null)
        {
            // Get the list of people from the service
            IEnumerable<Person> persons = await WebQuery.GetJson<IEnumerable<Person>>(new Uri($"{BaseUrl}?includeColours=true"), new CancellationTokenSource().Token);

            // Optionally filter it
            if (!string.IsNullOrEmpty(filter))
            {
                persons = persons.Where(p => p.FullName.ToLower().Contains(filter.ToLower())).ToList<Person>();
            }

            return persons;
        }
    }
}
