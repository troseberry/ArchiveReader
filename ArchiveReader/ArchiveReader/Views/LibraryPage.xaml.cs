using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Models;
using ArchiveReader.Data;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryPage : ContentPage
    {
        private LibraryDatabase _libraryDatabase;

        // this should be an ObservableCollection so it can be updated while viewing library (like if a delete happens)
        private List<Work> _libraryWorks;

        public LibraryPage()
        {
            InitializeComponent();

            SetupDatabaseConnection();
        }

        private async void SetupDatabaseConnection()
        {
            _libraryDatabase = await LibraryDatabase.Instance;

            _libraryWorks = await _libraryDatabase.GetWorksAsync();

            worksListView.ItemsSource = _libraryWorks;
        }

        private void WorksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            worksListView.SelectedItem = null;
        }

        private async void WorksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Work selectedWork = e.Item as Work;
            //await Navigation.PushAsync(new ReaderPage(selectedWork));

            string response = await DisplayActionSheet(selectedWork.title, "Cancel", "Delete", new string[] { "Read" });

            switch(response)
            {
                case "Read":
                    await Navigation.PushAsync(new ReaderPage(selectedWork));
                    break;
                case "Delete":
                    await _libraryDatabase.DeleteWorkAsync(selectedWork);
                    break;
            }
        }
    }
}