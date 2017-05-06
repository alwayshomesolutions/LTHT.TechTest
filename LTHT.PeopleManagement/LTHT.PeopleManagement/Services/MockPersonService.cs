using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LTHT.PeopleManagement.Models;

using Xamarin.Forms;

//[assembly: Dependency(typeof(LTHT.PeopleManagement.Services.MockPersonService))]
namespace LTHT.PeopleManagement.Services
{
    public class MockPersonService : IPersonService
    {
        bool isInitialized;
        string lastFilter = null;
        List<Person> persons;

        public async Task<Person> AddPersonAsync(Person person)
        {
            await initialiseAsync();

            this.persons.Add(person);

            return await Task.FromResult(person);
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            await initialiseAsync();

            var _person = persons.Where((Person arg) => arg.Id == person.Id).FirstOrDefault();
            persons.Remove(_person);
            persons.Add(person);

            return await Task.FromResult(person);
        }

        public async Task DeletePersonAsync(Person person)
        {
            await initialiseAsync();

            var _person = persons.Where((Person arg) => arg.Id == person.Id).FirstOrDefault();
            persons.Remove(_person);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            await initialiseAsync();

            return await Task.FromResult(persons.FirstOrDefault(person => person.Id == id));
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync(string filter = null)
        {
            await initialiseAsync(filter);

            return await Task.FromResult(persons);
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
