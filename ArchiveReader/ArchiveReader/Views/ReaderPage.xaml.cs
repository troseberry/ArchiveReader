using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderPage : ContentPage
    {
        public ReaderPage()
        {
            InitializeComponent();
        }

        public ReaderPage(Work workToRead)
        {
            InitializeComponent();

            string url = $"archiveofourown.org/works/{workToRead.id}";

            WebView webpage = new WebView
            {
                Source = $"https://" + url,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 1000,
                HeightRequest = 1000
            };

            webViewStackLayout.Children.Add(webpage);
        }
    }
}