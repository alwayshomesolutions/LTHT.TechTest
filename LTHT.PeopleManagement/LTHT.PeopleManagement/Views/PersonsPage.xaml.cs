using System;

using LTHT.PeopleManagement.Models;
using LTHT.PeopleManagement.ViewModels;

using Xamarin.Forms;

namespace LTHT.PeopleManagement.Views
{
    /// <summary>
    /// Persons page code behind
    /// </summary>
    public partial class PersonsPage : ContentPage
    {
        private PersonsViewModel viewModel = new PersonsViewModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PersonsPage()
        {
            InitializeComponent();
            this.NameFilter.TextChanged += NameFilter_TextChanged;
            BindingContext = viewModel;
        }

        /// <summary>
        /// Handle the filter text being changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This mechanism should incorporate some throttling and a delay mechanism (e.g. only update 1s after last keystroke etc.)
        /// so the service isn't overloaded. For now the brute force isBusy flag helps but this should have more sophistication</remarks>
        private void NameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Tell the viewmodel to refresh the persons accounting for the filter value
            viewModel.LoadPersonsCommand.Execute(e.NewTextValue);
        }

        /// <summary>
        /// Handle a person being selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void OnPersonSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Grab the selected person
            var person = args.SelectedItem as Person;
            if (person == null)
            {
                return;
            }

            // Go to the details page with the person's details
            await Navigation.PushAsync(new PersonPage(new PersonViewModel(person)));

            // Manually deselect item
            PersonsListView.SelectedItem = null;
        }

        /// <summary>
        /// Start the New Person flow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void AddPerson_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonPage(new PersonViewModel()));
        }

        /// <summary>
        /// Initialise the data on load
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // NOTE: This is a little inefficient as it will always re-load, including returning from cancelling on the Add New page when there is no need to refresh.
            // But for the purpose of the test it ensures the list is updated when new items are added.
            // Alternatives are to use Xamarin's messagecenter but that is not without issue so I tend to avoid it.
            // A more robust solution would be to use a MVVM framework that allows navigation to be driven from view models rather than pages.
            // For the test, I've avoided all 3rd party libraries and decided not to write my own as it wouldn't demonstrate much but would take a little more time.
            viewModel.LoadPersonsCommand.Execute(this.NameFilter.Text);
        }
    }
}