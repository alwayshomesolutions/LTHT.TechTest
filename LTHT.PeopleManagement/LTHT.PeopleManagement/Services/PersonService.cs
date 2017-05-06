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
        bool isInitialized;
        string lastFilter = null;
        List<Person> persons;

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

        private async Task initialiseAsync(string filter = null)
        {
            if (isInitialized && filter == this.lastFilter)
            {
                return;
            }

            this.lastFilter = filter;

            this.persons = new List<Person>();
            var _persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "Bob", LastName = "Bob"},
                new Person { Id = 2, FirstName = "Fred", LastName = "Blogs", Authorised = true },
                new Person { Id = 3, FirstName = "Jim", LastName = "Bob", Enabled = true },
                new Person { Id = 4, FirstName = "No", LastName = "One", Valid = true },
                new Person { Id = 5, FirstName = "Some", LastName = "Body", Colours = { new Colour() { Id = 1, Name = "Red" }, new Colour() { Id = 2, Name = "Green" }} },
                new Person { Id = 6, FirstName = "Any", LastName = "One", Authorised = true, Enabled = true },
            };

            if (!string.IsNullOrEmpty(filter))
            {
                _persons = _persons.Where(p => p.FullName.ToLower().Contains(filter.ToLower())).ToList<Person>();
            }

            foreach (Person person in _persons)
            {
                this.persons.Add(person);
            }

            isInitialized = true;
        }
    }
}
