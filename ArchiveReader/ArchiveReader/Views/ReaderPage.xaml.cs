using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace ArchiveReader.Views
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderPage : ContentPage
    {
        private HttpClient _client;
        private WebView _webView;
        private HtmlWebViewSource _htmlSource;

        private Work _currentWork;
        private int _currentChapterNumber;
        private string _apiFunctionPath;
        private ObservableCollection<Chapter> _chapters;

        public ReaderPage()
        {
            InitializeComponent();
        }

        public ReaderPage(Work workToRead)
        {
            InitializeComponent();

            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/"),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _htmlSource = new HtmlWebViewSource();
            _webView = new WebView()
            {
                Source = _htmlSource,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 1000,
                HeightRequest = 1000
            };
            webViewStackLayout.Children.Add(_webView);

            _currentWork = workToRead;
            _apiFunctionPath = "";
            chaptersListView.ItemsSource = _chapters;

            Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
            {
                Command = new Command(async () => {
                    Debug.WriteLine("GoBack");
                    Debug.WriteLine($"Shell Items Null: {Shell.Current.Items == null}");
                    await Navigation.PopAsync();
                })
            });

            BindingContext = _currentWork;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (_currentWork != null) SetUpReader();
        }

        private async void SetUpReader()
        {
            await HandleWorkBody(1);
            await HandleChapters();
            //Debug.WriteLine("SetUpReader");
        }

        //need to remove links for series from body and give alternative navigation method
        private async Task<bool> HandleWorkBody(int chapterNumber)
        {
            _apiFunctionPath = $"GetWorkBodyContentForChapter?workId={_currentWork.id}&lastId={_currentWork.latestChapterId}&chapterNumber={chapterNumber}";
            //Debug.WriteLine("[HandleWorkBody] - Api Func Path: " + _apiFunctionPath);

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    _htmlSource.Html = "";

                    string contentString = await response.Content.ReadAsStringAsync();

                    contentString = contentString.Replace(@"\n", string.Empty);
                    contentString = contentString.Replace(@"\", string.Empty);
                    contentString = contentString.Substring(1, contentString.Length - 2 );

                    _htmlSource.Html = contentString;

                    _currentChapterNumber = chapterNumber;

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
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
                    foreach (Chapter chapter in outputChapters)
                    {
                        //string[] titleSplit = chapter.title.Split(new[] { '.' }, 2);
                        _chapters.Add(chapter);
                    }

                    chaptersListView.ItemsSource = _chapters;
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        private void HamburgerMenuPressed(object sender, EventArgs e)

        {
            ToggleChaptersMenu();
        }

        private void ToggleChaptersMenu()
        {
            Shell.SetTabBarIsVisible(this, !Shell.GetTabBarIsVisible(this));
            Shell.SetNavBarIsVisible(this, !Shell.GetNavBarIsVisible(this));

            ChaptersPanel.IsVisible = !ChaptersPanel.IsVisible;
            TapCatcher.IsVisible = !TapCatcher.IsVisible;
            FakeHeader.IsVisible = !FakeHeader.IsVisible;

            if (TapCatcher.IsVisible)
            {
                LayoutParent.LowerChild(MainContentLayout);
            }
            else
            {
                LayoutParent.RaiseChild(MainContentLayout);
            }
        }

        private void ChaptersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            chaptersListView.SelectedItem = null;
        }

        private async void ChaptersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if ((e.ItemIndex + 1) != _currentChapterNumber)
            {
                ToggleChaptersMenu();
                await HandleWorkBody(e.ItemIndex + 1);
                //eventually implement loading overlay
            }
        }
    }
}