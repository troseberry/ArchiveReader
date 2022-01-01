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
    public partial class WorkDetailPage : ContentPage
    {
        private Work currentWork;
        public WorkDetailPage()
        {
            InitializeComponent();
        }

        public WorkDetailPage(Work workToDisplay)
        {
            InitializeComponent();

            currentWork = workToDisplay;
            BindingContext = workToDisplay;
        }

        private void ReadButton_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReaderPage(currentWork));
        }

        private void SaveButton_Pressed(object sender, EventArgs e)
        {

        }
    }
}