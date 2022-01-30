using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;
using ArchiveReader.Enums;
using ArchiveReader.Util;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private HttpClient _client = new HttpClient();
        private string _apiFunctionPath;
        private string _resultString;

        private string _currentSearchQuery;
        private int _currentPageNumber;

        private SortFilterArgs _sortFiterArgs;
        private bool _isSorted = false;

        public SearchPage()
        {
            InitializeComponent();

            _client.BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            MessagingCenter.Subscribe<SortFilterPage,SortFilterArgs>(this, "SortAndFilter", async (sender, args) =>
            {
                _sortFiterArgs = args;
                _isSorted = true;
                SortAndFilterResults();
            });
        }

        private async void SortAndFilterResults()
        {
            loadingActivityIndicator.IsRunning = true;
            UpdatedApiFunctionPathForSort(_sortFiterArgs);

            await RunAPICall();

            loadingActivityIndicator.IsRunning = false;
        }
        private void UpdatedApiFunctionPathForSort(SortFilterArgs newSortArgs)
        {
            _apiFunctionPath = $"GetAllFanficsOnPage?tagName={_currentSearchQuery}&pageNumber={_currentPageNumber}" +
                $"&completion={SortFilterHelper.SFEnumToUrlString(newSortArgs.completion)}" +
                $"&crossover={SortFilterHelper.SFEnumToUrlString(newSortArgs.crossover)}" +
                $"&sort={SortFilterHelper.SFEnumToUrlString(newSortArgs.sort)}" +
                $"&dateFrom={newSortArgs.dateFrom}&dateTo={newSortArgs.dateTo}" +
                $"&wordsFrom={newSortArgs.wordsFrom}&wordsTo={newSortArgs.wordsTo}" +
                $"&otherExcludedTags={newSortArgs.otherExcludedTags}" +
                $"&otherIncludedTags={newSortArgs.otherIncludedTags}" +
                $"&language={newSortArgs.language}" +
                $"&searchWithinResults={newSortArgs.searchWithinResults}";

            if (newSortArgs.ratingsInclude != RatingsOption.None) _apiFunctionPath += $"&ratingsInclude={SortFilterHelper.SFEnumToUrlString(newSortArgs.ratingsInclude)}";
            if (newSortArgs.ratingsExclude.Count > 0)
            {
                foreach(RatingsOption arg in newSortArgs.ratingsExclude)
                {
                    if (arg != RatingsOption.None) _apiFunctionPath += $"&ratingsExclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                }
            }

            if (newSortArgs.warningsInclude.Count > 0)
            {
                foreach (WarningsOption arg in newSortArgs.warningsInclude)
                {
                    if (arg != WarningsOption.None) _apiFunctionPath += $"&warningsInclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                }
            }

            if (newSortArgs.warningsExclude.Count > 0)
            {
                foreach (WarningsOption arg in newSortArgs.warningsExclude)
                {
                    if (arg != WarningsOption.None) _apiFunctionPath += $"&warningsExclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                }
            }

            if (newSortArgs.categoriesInclude.Count > 0)
            {
                foreach (CategoryOption arg in newSortArgs.categoriesInclude)
                {
                    if (arg != CategoryOption.None) _apiFunctionPath += $"&categoriesInclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                }
            }

            if (newSortArgs.categoriesExclude.Count > 0)
            {
                foreach (CategoryOption arg in newSortArgs.categoriesExclude)
                {
                    if (arg != CategoryOption.None) _apiFunctionPath += $"&categoriesExclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                }
            }
        }

        private async Task<bool> RunAPICall()
        {
            resultsListView.ItemsSource = new List<Work>();
            try
            {
                Debug.WriteLine("API Path: " + _apiFunctionPath);

                HttpResponseMessage response = await _client.GetAsync(_apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    _resultString = await response.Content.ReadAsStringAsync();
                    List<Work> outputWork = JsonSerializer.Deserialize<List<Work>>(_resultString);
                    foreach(Work w in outputWork)
                    {
                        w.GenerateExtraInfo();
                    }

                    resultsListView.ItemsSource = outputWork;
                    currentPageNumberLabel.Text = _currentPageNumber.ToString();
                    return true;
                }
            } 
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Execute search");

            loadingActivityIndicator.IsRunning = true;

            _currentSearchQuery = searchBar.Text;
            _currentPageNumber = 1;
            _sortFiterArgs = new SortFilterArgs();

            _apiFunctionPath = $"GetAllFanficsOnPage?tagName={_currentSearchQuery}&pageNumber={_currentPageNumber}";

            await RunAPICall();

            loadingActivityIndicator.IsRunning = false;
            if (!sortFilterButton.IsEnabled) sortFilterButton.IsEnabled = true;
        }

        private void ResultsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            resultsListView.SelectedItem = null;
        }

        private async void ResultsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Work selectedWork = e.Item as Work;
            await Navigation.PushAsync(new WorkDetailPage(selectedWork));
        }

        private async void SortFilterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SortFilterPage());
        }

        private async void PrevButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPageNumber > 1)
            {
                loadingActivityIndicator.IsRunning = true;
                
                _currentPageNumber--;
                _apiFunctionPath = $"GetAllFanficsOnPage?tagName={_currentSearchQuery}&pageNumber={_currentPageNumber}";

                await RunAPICall();

                if (_currentPageNumber == 1) previousPageButton.IsEnabled = false;

                loadingActivityIndicator.IsRunning = false;
            }
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            //need to cap based on result

            loadingActivityIndicator.IsRunning = true;

            _currentPageNumber++;
            _apiFunctionPath = $"GetAllFanficsOnPage?tagName={_currentSearchQuery}&pageNumber={_currentPageNumber}";

            await RunAPICall();

            if (_currentPageNumber > 1) previousPageButton.IsEnabled = true;

            loadingActivityIndicator.IsRunning = false;
        }
    }
}