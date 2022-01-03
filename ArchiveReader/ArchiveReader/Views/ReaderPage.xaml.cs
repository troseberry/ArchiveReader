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

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderPage : ContentPage
    {
        string apiFunctionPath;
        private string resultString;
        HttpClient client = new HttpClient();

        public ReaderPage()
        {
            InitializeComponent();
        }

        public ReaderPage(Work workToRead)
        {
            InitializeComponent();

            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string chapterNumber = "1";

            apiFunctionPath = $"ao3api.netlify.app/.netlify/functions/GetWorkBodyContentForChapter?workId={workToRead.id}&lastId={workToRead.latestChapterId}&chapterNumber={chapterNumber}";
            apiFunctionPath = "https://" + apiFunctionPath;

            Debug.WriteLine("Api Func Path: " + apiFunctionPath);

            RunAPICall();
        }

        private async void RunAPICall()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiFunctionPath);
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();

                    resultString = resultString.Replace(@"\n", string.Empty);
                    resultString = resultString.Replace(@"\", string.Empty);

                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = resultString;

                    WebView webpage = new WebView
                    {
                        Source = htmlSource,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        WidthRequest = 1000,
                        HeightRequest = 1000
                    };

                    webViewStackLayout.Children.Add(webpage);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}