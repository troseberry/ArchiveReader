using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;
using System.Text.Json;
using System.Diagnostics;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkDetailPage : ContentPage
    {
        private Work currentWork;
        public WorkDetailPage()
        {
            InitializeComponent();
            Debug.WriteLine("Init WorkDetailPage");
        }

        public WorkDetailPage(Work workToDisplay)
        {
            InitializeComponent();

            currentWork = workToDisplay;
            BindingContext = workToDisplay;
        }

        private async void ReadButton_Pressed(object sender, EventArgs e)

        {
            await Navigation.PushAsync(new ReaderPage(currentWork));
            //await Navigation.PushAsync(new ReaderPage());
            //GoToReader();
        }

        private async void SaveButton_Pressed(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new WorkDetailPage());
            await Navigation.PushAsync(new ReaderPage());
        }
    }
}