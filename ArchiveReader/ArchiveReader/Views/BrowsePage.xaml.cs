using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowsePage : ContentPage
    {
        private HttpClient _client = new HttpClient();
        private string _apiFunctionPath;
        private string _resultString;

        public BrowsePage()
        {
            InitializeComponent();

            _client.BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        private void OnFandomSelected(object sender, EventArgs e)
        {
            StackLayout selected = (StackLayout)sender;
            
            switch(selected.Id.ToString())
            {
                case "animeMangaFandom":
                    break;
                case "booksLitFandom":
                    break;
                case "cartoonsFandom":
                    break;
                case "celebritiesFandom":
                    break;
                case "moviesFandom":
                    break;
                case "musicBandsFandom":
                    break;
                case "otherMediaFandom":
                    break;
                case "theaterFandom":
                    break;
                case "tvShowsFandom":
                    break;
                case "videoGamesFandom":
                    break;
                case "uncategorizedFandom":
                    break;
                default:
                    break;
            }
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var resultsPage = new WorkResultsPage();

            Shell.SetNavBarIsVisible(resultsPage, false);
            Shell.SetTabBarIsVisible(resultsPage, false);

            await Navigation.PushAsync(resultsPage);
        }
    }
}