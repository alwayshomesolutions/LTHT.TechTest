using System;
using System.Diagnostics;
using System.Threading.Tasks;

using LTHT.PeopleManagement.Helpers;
using LTHT.PeopleManagement.Models;

using Xamarin.Forms;

namespace LTHT.PeopleManagement.ViewModels
{
    /// <summary>
    /// The viewmodel for the Persons page
    /// </summary>
    public class PersonsViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets a collection of <see cref="Person"/>
        /// </summary>
        public ObservableRangeCollection<Person> Persons { get; set; }

        /// <summary>
        /// Command to load persons
        /// </summary>
        public Command<string> LoadPersonsCommand { get; set; }

        /// <summary>
        /// Command to delete a person
        /// </summary>
        public Command DeletePersonCommand { get; set; }

        /// <summary>
        /// Constructor for the view model
        /// </summary>
        public PersonsViewModel()
        {
            // Set up some defaults
            this.Title = "Tech Test";
            this.Persons = new ObservableRangeCollection<Person>();

            // Initialise the commands
            this.LoadPersonsCommand = new Command<string>(async (filter) => await this.LoadPersons(filter));
            this.DeletePersonCommand = new Command<Person>(async (person) => await this.DeletePerson(person));
        }

        /// <summary>
        /// Load a collection of <see cref="Person"/> from the service for the UI to bind to
        /// </summary>
        /// <param name="filter">An optional filter</param>
        /// <returns>Async task</returns>
        private async Task LoadPersons(string filter)
        {
            // For simplicity, don't make multiple requests
            if (this.IsBusy)
            {
                return;
            }

            // Indicate we are busy
            this.IsBusy = true;

            try
            {
                // Reset and fetch the list of persons from the service and store for binding
                this.Persons.Clear();
                var persons = await PersonService.GetPersonsAsync(filter);
                this.Persons.ReplaceRange(persons);
            }
            catch (Exception ex)
            {
                // Placeholder error handling
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Delete a <see cref="Person"/> from the service
        /// </summary>
        /// <param name="person">The person to delete</param>
        /// <returns>Async task</returns>
        private async Task DeletePerson(Person person)
        {
            // For simplicity, don't make multiple requests
            if (this.IsBusy)
            {
                return;
            }

            // Indicate we are busy
            this.IsBusy = true;

            try
            {
                // Remove the person
                await PersonService.DeletePersonAsync(person);
                this.Persons.Remove(person);
            }
            catch (Exception ex)
            {
                // Placeholder error handling
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}