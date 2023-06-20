using ArchiveReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            recentlyReadListView.ItemsSource = new List<Work>()
            {
                new Work()
            };
        }


        private async void NavToReader()
        {
            await Navigation.PushAsync(new WorkDetailPage());
        }

        private async void PushFilterModal()
        {
            await Navigation.PushModalAsync(new SortFilterPage());
        }

        private void RecentlyReadListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            recentlyReadListView.SelectedItem = null;
        }

        private async void RecentlyReadListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Work selectedWork = e.Item as Work;
            await Navigation.PushAsync(new WorkDetailPage(selectedWork));
        }
    }
}