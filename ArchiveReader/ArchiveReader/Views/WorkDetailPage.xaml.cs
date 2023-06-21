using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;
using ArchiveReader.Data;

using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkDetailPage : ContentPage
    {
        private string _apiFunctionPath;

        private HttpClient _client;

        private Work _currentWork;
        private ObservableCollection<Chapter> _chapters;

        public WorkDetailPage()
        {
            InitializeComponent();
        }

        public WorkDetailPage(Work workToDisplay)
        {
            InitializeComponent();

            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/"),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiFunctionPath = "";

            _currentWork = workToDisplay;

            BindingContext = _currentWork;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (_currentWork != null) SetupDetails();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_currentWork != null) SetupDetails();
        }

        //To-Do: figure out why bindable layout won't fully populate all chapters unless
        //SetItemsSource gets called twice (only fixes some instances)
        private async void SetupDetails()
        {
            await HandleChapters();
            BindableLayout.SetItemsSource(ChaptersLayout, _chapters);
        }

        private async Task<bool> HandleChapters()
        {
            _apiFunctionPath = $"GetChaptersForFanfic?workId={_currentWork.id}&lastId={_currentWork.latestChapterId}";
            //Debug.WriteLine("[HandleChapters] - Api Func Path: " + _apiFunctionPath);

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    List<Chapter> outputChapters = JsonSerializer.Deserialize<List<Chapter>>(contentString);

                    _chapters = new ObservableCollection<Chapter>();

                    for (int i = outputChapters.Count - 1; i >= 0; i--)
                    {
                        var chapter = outputChapters[i];

                        //int splitIndex = chapter.title.IndexOf('.');
                        //chapter.title = chapter.title.Substring(splitIndex + 1);

                        _chapters.Add(chapter);
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        private async void ReadFromBeginningButton_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReaderPage(_currentWork));
        }

        private async void SaveButton_Pressed(object sender, EventArgs e)
        {
            LibraryDatabase db = await LibraryDatabase.Instance;

            await db.SaveWorkAsync(_currentWork);
        }

        private void ChaptersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //chaptersListView.SelectedItem = null;
        }

        private void ChaptersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
        }
    }
}