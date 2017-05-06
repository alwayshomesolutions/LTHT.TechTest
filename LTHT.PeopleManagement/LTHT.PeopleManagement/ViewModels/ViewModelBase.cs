using LTHT.PeopleManagement.Helpers;
using LTHT.PeopleManagement.Models;
using LTHT.PeopleManagement.Services;

using Xamarin.Forms;

namespace LTHT.PeopleManagement.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// Reference to person service.
        /// </summary>
        /// <remarks>In reality, this would use an IoC implementation (rather than re-purposing Xamarin's basic service locator which is more for
        /// platform dependent code) to load the correct implementation, reduce coupling and facilitate testing.</remarks>
        public IPersonService PersonService => DependencyService.Get<IPersonService>();

        /// <summary>
        /// Backing field for <see cref="IsBusy"/>
        /// </summary>
        private bool isBusy = false;

        /// <summary>
        /// Gets or sets a value indicating if the view model is currently busy with a long running operation
        /// </summary>
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { SetProperty(ref this.isBusy, value); }
        }

        /// <summary>
        /// Backing field for <see cref="Title"/>
        /// </summary>
        private string title = string.Empty;
        
        /// <summary>
        /// Gets or sets a value representing the title of the item
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { SetProperty(ref this.title, value); }
        }
    }
}