
using LTHT.PeopleManagement.ViewModels;

using Xamarin.Forms;

namespace LTHT.PeopleManagement.Views
{
    /// <summary>
    /// Person page code behind
    /// </summary>
    public partial class PersonPage : ContentPage
    {
        private PersonViewModel viewModel;

        /// <summary>
        /// Default Xamarin constructor
        /// </summary>
        public PersonPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Construct with a prepared viewmodel (the norm)
        /// </summary>
        /// <param name="viewModel">A populated viewmodel</param>
        public PersonPage(PersonViewModel viewModel) : this()
        {
            BindingContext = this.viewModel = viewModel;
            this.viewModel.NavigateBack += ViewModel_NavigateBack;
        }

        /// <summary>
        /// If instructed by the viewmodel, navigate back
        /// </summary>
        /// <param name="sender">The view model</param>
        /// <param name="e">Not used</param>
        private void ViewModel_NavigateBack(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
