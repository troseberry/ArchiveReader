using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;
using System.Diagnostics;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private string apiFunctionPath;
        private string resultString;
        HttpClient client = new HttpClient();

        public SearchPage()
        {
            InitializeComponent();

            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            searchBar.SearchButtonPressed += SearchBar_SearchButtonPressed;
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            //https://thirsty-kilby-a79641.netlify.app/.netlify/functions/GetAllFanficsOnPage?tagName=dcu&pageNumber=1
            //https://ao3api.netlify.app/.netlify/functions/GetAllFanficsOnPage?tagName=dcu

            apiFunctionPath = "https://ao3api.netlify.app/.netlify/functions/GetAllFanficsOnPage?tagName=" + searchBar.Text;
            //resultsLabel.Text = searchBar.Text;
            RunAPICall();

            Debug.WriteLine("Execute search");
        }

        private async void RunAPICall()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                    List<Work> outputWork = JsonSerializer.Deserialize<List<Work>>(resultString);
                    foreach(Work w in outputWork)
                    {
                        w.GenerateExtraInfo();
                    }

                    resultsListView.ItemsSource = outputWork;
                }
            } 
            catch (Exception e)
            {

            }
        }

        private void ResultsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            resultsListView.SelectedItem = null;
        }

        private void ResultsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Work selectedWork = e.Item as Work;

            Navigation.PushAsync(new WorkDetailPage(selectedWork));
        }
    }
}