using ArchiveReader.Enums;
using ArchiveReader.Models;
using ArchiveReader.Util;
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

        private bool _canSearch;
        private bool _canSort;

        private SearchType _currentSearchType;
        private string _currentSearchQuery;
        private string _currentSortQuery;
        private int _currentPageNumber;
        private bool _isSorted;

        private SortFilterArgs _sortFiterArgs;

        private Color GrayedOutColor;

        public WorkResultsPage(string searchQuery = "", bool canSearch = true, bool canSort = true,
            SearchType pageSourceSearchType = SearchType.Works)
        {
            InitializeComponent();

            _client.BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //searchBar.Focus();
            _currentSearchType = pageSourceSearchType;
            _canSearch = canSearch;
            _canSort = canSort;

            if (!canSort)
            {
                //To-Do: gray out button
                sortFilterButton.IsEnabled = false;
            }
            else
            {
                MessagingCenter.Subscribe<SortFilterPage, SortFilterArgs>(this, "SortAndFilter", async (sender, args) =>
                {
                    _isSorted = true;
                    SortAndFilterResults(args);
                });
            }

        }

        #region Xaml Event Listeners
        public async void OnSearchBarSearchButtonPressed(object sender, EventArgs e)
        {
            _isSorted = false;
            _sortFiterArgs = new SortFilterArgs();

            await ExecuteSearch(searchBar.Text, string.Empty, 1);
        }

        public async void OnSortFilterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SortFilterPage());
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

        private async Task ExecuteSearch(string searchQuery, string sortQuery, int pageNumber)
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

        private async void SortAndFilterResults(SortFilterArgs sortArgs)
        {
            if (!_canSort)
            {
                return;
            }

            _sortFiterArgs = sortArgs;
            string sortQuery = GetSortQueryFromArgs(_sortFiterArgs);

            await ExecuteSearch(_currentSearchQuery, sortQuery, 1);
        }

        private string GetSortQueryFromArgs(SortFilterArgs newSortArgs)
        {
            string newSortQuery = $"&completion={SortFilterHelper.SFEnumToUrlString(newSortArgs.completion)}" +
                $"&crossover={SortFilterHelper.SFEnumToUrlString(newSortArgs.crossover)}" +
                $"&sort={SortFilterHelper.SFEnumToUrlString(newSortArgs.sort)}" +
                $"&dateFrom={newSortArgs.dateFrom}&dateTo={newSortArgs.dateTo}" +
                $"&wordsFrom={newSortArgs.wordsFrom}&wordsTo={newSortArgs.wordsTo}" +
                $"&otherExcludedTags={newSortArgs.otherExcludedTags}" +
                $"&otherIncludedTags={newSortArgs.otherIncludedTags}" +
                $"&language={newSortArgs.language}" +
                $"&searchWithinResults={newSortArgs.searchWithinResults}";

            if (newSortArgs.ratingsInclude != RatingsOption.None)
            {
                newSortQuery += $"&ratingsInclude={SortFilterHelper.SFEnumToUrlString(newSortArgs.ratingsInclude)}";
            }

            if (newSortArgs.ratingsExclude.Count > 0)
            {
                foreach (RatingsOption arg in newSortArgs.ratingsExclude)
                {
                    if (arg != RatingsOption.None)
                    {
                        newSortQuery += $"&ratingsExclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                    }
                }
            }

            if (newSortArgs.warningsInclude.Count > 0)
            {
                foreach (WarningsOption arg in newSortArgs.warningsInclude)
                {
                    if (arg != WarningsOption.None)
                    {
                        newSortQuery += $"&warningsInclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                    }
                }
            }

            if (newSortArgs.warningsExclude.Count > 0)
            {
                foreach (WarningsOption arg in newSortArgs.warningsExclude)
                {
                    if (arg != WarningsOption.None)
                    {
                        newSortQuery += $"&warningsExclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                    }
                }
            }

            if (newSortArgs.categoriesInclude.Count > 0)
            {
                foreach (CategoryOption arg in newSortArgs.categoriesInclude)
                {
                    if (arg != CategoryOption.None)
                    {
                        newSortQuery += $"&categoriesInclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                    }
                }
            }

            if (newSortArgs.categoriesExclude.Count > 0)
            {
                foreach (CategoryOption arg in newSortArgs.categoriesExclude)
                {
                    if (arg != CategoryOption.None)
                    {
                        newSortQuery += $"&categoriesExclude={SortFilterHelper.SFEnumToUrlString(arg)}";
                    }
                }
            }

            return newSortQuery;
        }

        // Keep synced with strings in  ArchiveOfOurOwnAPI.GetAllFanficsOnPage.getBaseUrlForPageType
        private string GetPageTypeForSearchType(SearchType searchType)
        {
            switch(searchType)
            {
                case SearchType.Tags:
                    return "tagWorksPage";
                default:
                    return "workSearchResultsPage";
            }
        }

        private async Task<List<Work>> RunAPICall(string searchQuery, string sortQuery, int pageNumber)
        {
            var pageType = GetPageTypeForSearchType(_currentSearchType);

            _currentSearchQuery = searchQuery;
            _currentSortQuery = sortQuery;
            _currentPageNumber = pageNumber;

            _apiFunctionPath = $"GetAllFanficsOnPage?pageType={pageType}&searchQuery={_currentSearchQuery}&pageNumber={_currentPageNumber}";

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