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
        Work currentWork;
        HttpClient client;
        WebView webView;
        HtmlWebViewSource htmlSource;

        private string apiFunctionPath;
        private string resultString;

        List<MenuItem> chapters;

        public ReaderPage()
        {
            InitializeComponent();

            /*
            client = new HttpClient();
            apiFunctionPath = "";

            Debug.WriteLine("Init Component");

            Work testBind = new Work();
            testBind.title = "Liminal";
            testBind.latestChapterId = "90934045";
            testBind.id = "36130966";

            client = new HttpClient();
            client.BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            currentWork = testBind;
            BindingContext = currentWork;

            htmlSource = new HtmlWebViewSource();
            webView = new WebView();

            webView.Source = htmlSource;
            webView.VerticalOptions = LayoutOptions.FillAndExpand;
            webView.WidthRequest = 1000;
            webView.HeightRequest = 1000;

            webViewStackLayout.Children.Add(webView);

            Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
            {
                Command = new Command(async () => {

                    Debug.WriteLine("AppShell Instance: " + AppShell.Instance == null);
                    await Shell.Current.Navigation.PopAsync();
                })
            });
            */
        }

        public ReaderPage(Work workToRead)
        {
            InitializeComponent();
            Debug.WriteLine("Init Component");

            currentWork = workToRead;
            apiFunctionPath = "";

            client = new HttpClient();
            client.BaseAddress = new Uri("https://ao3api.netlify.app/.netlify/functions/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            BindingContext = currentWork;

            htmlSource = new HtmlWebViewSource();
            webView = new WebView();

            webView.Source = htmlSource;
            webView.VerticalOptions = LayoutOptions.FillAndExpand;
            webView.WidthRequest = 1000;
            webView.HeightRequest = 1000;

            webViewStackLayout.Children.Add(webView);

            chaptersListView.BindingContext = chapters;

            Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
            {
                Command = new Command(async () => {
                    Debug.WriteLine("GoBack");
                    Debug.WriteLine($"Shell Items Null: {Shell.Current.Items == null}");
                    await Navigation.PopAsync();
                })
            });
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (currentWork != null) SetUpReader();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("OnAppearing");
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
            Debug.WriteLine("OnDisappearing");
        }

        private void SetUpReader()
        {
            HandleWorkBody("1");
            HandleChapters();
            //Debug.WriteLine("SetUpReader");
        }

        //change task type to bool to return success?
        private async void HandleWorkBody(string chapterNumber)
        {
            //Debug.WriteLine(Environment.StackTrace);
            //string chapterNumber = "1";
            apiFunctionPath = $"GetWorkBodyContentForChapter?workId={currentWork.id}&lastId={currentWork.latestChapterId}&chapterNumber={chapterNumber}";

            Debug.WriteLine("Api Func Path: " + apiFunctionPath);

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    htmlSource.Html = "";

                    resultString = await response.Content.ReadAsStringAsync();

                    resultString = resultString.Replace(@"\n", string.Empty);
                    resultString = resultString.Replace(@"\", string.Empty);

                    htmlSource.Html = resultString;

                    /*
                    if (Shell.Current.FlyoutIsPresented)
                    {
                        Shell.Current.FlyoutIsPresented = false;
                    }
                    */
                }
            }
            catch (Exception e)
            {

            }
        }

        private async void HandleChapters()
        {
            Debug.WriteLine("HandleChapters");
            apiFunctionPath = $"GetChaptersForFanfic?workId={currentWork.id}&lastId={currentWork.latestChapterId}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                    List<Chapter> outputChapters = JsonSerializer.Deserialize<List<Chapter>>(resultString);

                    chapters = new List<MenuItem>();
                    for (int i = 0; i < outputChapters.Count; i++)
                    {
                        string[] titleSplit = outputChapters[i].title.Split(new[] { '.' }, 2);

                        MenuItem newChap = new MenuItem
                        {
                            Text = outputChapters[i].title,
                            IconImageSource = "icon_feed.png",
                            Command = new Command(() => HandleWorkBody(titleSplit[0])),
                        };
                        chapters.Add(newChap);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void HamburgerMenuPressed(object sender, EventArgs e)
        {
            Shell.SetTabBarIsVisible(this, !Shell.GetTabBarIsVisible(this));
            Shell.SetNavBarIsVisible(this, !Shell.GetNavBarIsVisible(this));

            ChaptersPanel.IsVisible = !ChaptersPanel.IsVisible;
            TapCatcher.IsVisible = !TapCatcher.IsVisible;

            if (TapCatcher.IsVisible)
            {
                LayoutParent.RaiseChild(TapCatcher);
                LayoutParent.RaiseChild(ChaptersPanel);
            }
            else
            {
                LayoutParent.RaiseChild(MainContentLayout);
            }
        }
    }
}