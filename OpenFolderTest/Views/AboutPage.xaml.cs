using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OpenFolderTest.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IMyService>().OpenFolder();
        }

        private async void Button_Clicked2(object sender, EventArgs e)
        {

            try
            {
                string u = (img1.Source as UriImageSource).Uri.AbsoluteUri;
                string fileName = "";
                int i = u.LastIndexOf("/");
                if (i != -1)
                    fileName = u.Substring(i + 1);
                else
                    fileName = $"pic{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss)")}.jpg";

                HttpClient client = new HttpClient(new HttpClientHandler(), true);
                Stream s = await client.GetStreamAsync(u);
                MemoryStream ms = new MemoryStream();
                s.CopyTo(ms);
                byte[] b = ms.ToArray();
                s.Close();
                ms.Close();
                bool success = await DependencyService.Get<IMyService>().SaveToGallery(b, null, fileName);
                if (success)
                    await DisplayAlert("Info", "Save with no error, goto gallery to check it out!", "OK");
                else
                    await DisplayAlert("Error", "Save has error", "OK");
            }
            catch(Exception error)
            {
                Console.WriteLine("----------------------Error " + error.Message);
            }
        }

        private async void Button_Clicked3(object sender, EventArgs e)
        {
            try
            {
                string u = (img2.Source as UriImageSource).Uri.AbsoluteUri;
                string fileName = "";
                int i = u.LastIndexOf("/");
                if (i != -1)
                    fileName = u.Substring(i + 1);
                else
                    fileName = $"pic{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss)")}.jpg";

                HttpClient client = new HttpClient(new HttpClientHandler(), true);
                Stream s = await client.GetStreamAsync(u);
                MemoryStream ms = new MemoryStream();
                s.CopyTo(ms);
                byte[] b = ms.ToArray();
                s.Close();
                ms.Close();
                bool success = await DependencyService.Get<IMyService>().SaveToGallery(b, null, fileName);
                if (success)
                    await DisplayAlert("Info", "Save with no error, goto gallery to check it out!", "OK");
                else
                    await DisplayAlert("Error", "Save has error", "OK");
            }
            catch (Exception error)
            {
                Console.WriteLine("----------------------Error " + error.Message);
            }
        }
    }
}