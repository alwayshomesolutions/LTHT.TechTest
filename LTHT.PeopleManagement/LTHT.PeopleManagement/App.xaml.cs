using LTHT.PeopleManagement.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LTHT.PeopleManagement
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.MainPage = new NavigationPage(new PersonsPage());
        }
    }
}
