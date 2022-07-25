using OpenFolderTest.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace OpenFolderTest.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}