using ArchiveReader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkResultsPage : ContentPage
    {
        private HttpClient _client = new HttpClient();
        private string _apiFunctionPath;
        private string _resultString;

        private string _currentSearchQuery;
        private string _currentSortQuery;
        private int _currentPageNumber;
        private bool _isSorted;

        private SortFilterArgs _sortFiterArgs;

        public WorkResultsPage()
        {
            InitializeComponent();

            _client.BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            searchBar.Focus();

        }

        #region Xaml Event Listeners
        public void OnSearchBarSearchButtonPressed(object sender, EventArgs e)
        {
            _isSorted = false;
            _sortFiterArgs = new SortFilterArgs();

            ExecuteSearch(searchBar.Text, string.Empty, 1);
        }

        public void OnSortFilterButtonClicked(object sender, EventArgs e)
        {

        }

        public void OnPrevButtonClicked(object sender, EventArgs e)
        {

        }

        public void OnNextButtonClicked(object sender, EventArgs e)
        {

        }

        public void OnResultsListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            resultsListView.SelectedItem = null;
        }

        public async void OnResultsListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Work selectedWork = e.Item as Work;

            var detailPage = new WorkDetailPage(selectedWork);
            Shell.SetTabBarIsVisible(detailPage, false);

            await Navigation.PushAsync(detailPage);
        }

        public void OnResultsListViewRefreshing(object sender, EventArgs e)
        {

        }
        #endregion

        private async void ExecuteSearch(string searchQuery, string sortQuery, int pageNumber)
        {
            Debug.WriteLine($"[WorkResultsPage][OnSearchBarSearchButtonPressed] - Execute search -" +
                $" Search Query {searchQuery}" +
                $" Sort Query {sortQuery}" +
                $" Page Number {pageNumber}");

            loadingActivityIndicator.IsRunning = true;
            noResultsLabel.IsVisible = false;

            resultsListView.ItemsSource = new List<Work>();

            var results = await RunAPICall(searchQuery, sortQuery, pageNumber);
            if (results != null)
            {
                resultsListView.ItemsSource = results;
                sortFilterButton.IsEnabled = true;
                currentPageNumberLabel.Text = _currentPageNumber.ToString();
            }
            else
            {
                noResultsLabel.IsVisible = true;
                sortFilterButton.IsEnabled = false;
            }

            loadingActivityIndicator.IsRunning = false;
        }

        private async Task<List<Work>> RunAPICall(string searchQuery, string sortQuery, int pageNumber)
        {
            _currentSearchQuery = searchQuery;
            _currentSortQuery = sortQuery;
            _currentPageNumber = pageNumber;

            _apiFunctionPath = $"GetAllFanficsOnPage?tagName={_currentSearchQuery}&pageNumber={_currentPageNumber}{_currentSortQuery}";

            try
            {
                Debug.WriteLine($"[WorkResultsPage][RunAPICall] - API Path: {_apiFunctionPath}");
                HttpResponseMessage response = await _client.GetAsync(_apiFunctionPath);

                if (response.IsSuccessStatusCode)
                {
                    _resultString = await response.Content.ReadAsStringAsync();
                    List<Work> outputWork = JsonSerializer.Deserialize<List<Work>>(_resultString);

                    if (outputWork.Count <= 0)
                    {
                        return null;
                    }

                    return outputWork;
                }
            }
            catch (Exception _)
            {
                return null;
            }

            return null;
        }

    }
}