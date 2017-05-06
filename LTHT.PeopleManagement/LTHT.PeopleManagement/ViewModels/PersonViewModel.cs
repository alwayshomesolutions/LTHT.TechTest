using LTHT.PeopleManagement.Helpers;
using LTHT.PeopleManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LTHT.PeopleManagement.ViewModels
{
    /// <summary>
    /// View model representing a Person and the Person detail page
    /// </summary>
    public class PersonViewModel : ViewModelBase
    {
        /// <summary>
        /// The Person represented by this view model
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Save a person
        /// </summary>
        public Command SavePersonCommand { get; set; }

        /// <summary>
        /// An event to indicate the view needs to navigate back
        /// </summary>
        public event EventHandler NavigateBack;

        /// <summary>
        /// Backing field for <see cref="Red"/>
        /// </summary>
        private bool red;

        /// <summary>
        /// Gets or sets a flag indicating if the colour red is selected
        /// </summary>
        public bool Red
        {
            get { return this.red; }
            set { this.ToggleColour(ref this.red, value, ColourEnum.Red); }
        }

        /// <summary>
        /// Backing field for <see cref="Green"/>
        /// </summary>
        private bool green;

        /// <summary>
        /// Gets or sets a flag indicating if the colour green is selected
        /// </summary>
        public bool Green
        {
            get { return this.green; }
            set { this.ToggleColour(ref this.green, value, ColourEnum.Green); }
        }

        /// <summary>
        /// Backing field for <see cref="Blue"/>
        /// </summary>
        private bool blue;

        /// <summary>
        /// Gets or sets a flag indicating if the colour blue is selected
        /// </summary>
        public bool Blue
        {
            get { return this.blue; }
            set { this.ToggleColour(ref this.blue, value, ColourEnum.Blue); }
        }

        /// <summary>
        /// Backing field for <see cref="NotValid"/>
        /// </summary>
        private bool notValid;

        /// <summary>
        /// Gets or sets a flag indicating if the person can be saved
        /// </summary>
        public bool NotValid
        {
            get { return this.notValid; }
            set { SetProperty(ref this.notValid, value); }
        }

        /// <summary>
        /// View model constructor optionally taking in an existing <see cref="Person"/>
        /// </summary>
        /// <param name="person">Optional initialisation</param>
        public PersonViewModel(Person person = null)
        {
            // Set up defaults
            this.Title = person?.FullName ?? "New Person";
            this.Person = person ?? new Person();

            // Init command for Save
            this.SavePersonCommand = new Command(async () => await this.SavePerson());
            
            // Initialise the colours
            // TODO: All this colour code is a time saving exercise for the purpose of the test. With more time, a call should be made to
            // the service for the available colours and the UI built dynamically based on the available colours.
            if (this.Person.Colours.Where(c => c.Id == 1).Any())
            {
                this.Red = true;
            }

            if (this.Person.Colours.Where(c => c.Id == 2).Any())
            {
                this.Green = true;
            }

            if (this.Person.Colours.Where(c => c.Id == 3).Any())
            {
                this.Blue = true;
            }
        }

        /// <summary>
        /// Temp colour code. As stated above should be driven from service.
        /// </summary>
        /// <param name="backingStore">Value to update</param>
        /// <param name="value">The new value</param>
        /// <param name="colour">Which colour is this</param>
        private void ToggleColour(ref bool backingStore, bool value, ColourEnum colour)
        {
            // Update the property on this VM
            SetProperty(ref backingStore, value);

            // Copy the existing colours to a new object so we can force an update (and trigger the binding to update)
            var colours = new ObservableRangeCollection<Colour>();
            colours.AddRange(this.Person.Colours);

            // Also ensure the colours in the person's colour collection is correct
            if (value)
            {
                // The colour must be present
                if (!colours.Where(c => c.Id == (int)colour).Any())
                {
                    colours.Add(new Colour() { Id = (int)colour, Name = colour.ToString() });
                }
            }
            else
            {
                // Make sure the colour is not there
                var existingColour = colours.FirstOrDefault(c => c.Id == (int)colour);
                if (existingColour != null)
                {
                    colours.Remove(existingColour);
                }
            }

            // Update the colour collection and trigger the refresh
            this.Person.Colours = colours;
        }

        /// <summary>
        /// Update the person with the service
        /// </summary>
        /// <returns></returns>
        private async Task SavePerson()
        {
            // Simplistic avoidance of duplicate calls
            if (this.IsBusy)
            {
                return;
            }

            // Validation
            if (string.IsNullOrEmpty(this.Person.FirstName) || string.IsNullOrEmpty(this.Person.LastName))
            {
                this.NotValid = true;
                return;
            }

            this.IsBusy = true;

            try
            {
                // Save the person and navigate back
                if (this.Person.Id == 0)
                {
                    await PersonService.AddPersonAsync(this.Person);
                }
                else
                {
                    await PersonService.UpdatePersonAsync(this.Person);
                }

                NavigateBack?.Invoke(this, new EventArgs());
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
        /// Temp in lieu of retrieving colours from service
        /// </summary>
        private enum ColourEnum
        {
            Red = 1,
            Green = 2,
            Blue = 3,
        }
    }
}