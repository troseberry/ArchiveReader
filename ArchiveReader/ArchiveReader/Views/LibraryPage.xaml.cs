using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ArchiveReader.Enums;
using ArchiveReader.Data;
using ArchiveReader.Models;
using ArchiveReader.Util;

namespace ArchiveReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryPage : ContentPage
    {
        private LibraryDatabase _libraryDatabase;

        private List<Work> _databaseWorks;
        private ObservableCollection<Work> _worksObservableCollection;

        public LibraryPage()
        {
            InitializeComponent();

            SetupDatabaseConnection();
        }
        private async void SetupDatabaseConnection()
        {
            _libraryDatabase = await LibraryDatabase.Instance;
            _libraryDatabase.WorkAddedEvent += LibraryDatabaseAddedTo;

            _databaseWorks = await _libraryDatabase.GetWorksAsync();

            _worksObservableCollection = new ObservableCollection<Work>(_databaseWorks);
            worksListView.ItemsSource = _worksObservableCollection;
        }

        private async void LibraryDatabaseAddedTo(object sender, Work arg)
        {
            _databaseWorks = await _libraryDatabase.GetWorksAsync();
            _worksObservableCollection.Add(arg);                        // don't need to update itemssource for change to take affect
        }

        private void WorksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            worksListView.SelectedItem = null;
        }

        private async void WorksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Work selectedWork = e.Item as Work;

            string response = await DisplayActionSheet(selectedWork.title, "Cancel", "Delete", new string[] { "Read" });

            switch(response)
            {
                case "Read":
                    await Navigation.PushAsync(new ReaderPage(selectedWork));
                    break;
                case "Delete":
                    await _libraryDatabase.DeleteWorkAsync(selectedWork);
                    _worksObservableCollection.Remove(selectedWork);            // don't need to update itemssource for change to take affect
                    break;
            }
        }
    
        private async void SortFilterButtonTapped(object sender, EventArgs e)
        {
            string[] sortOptions = 
            {
                "Author",
                "Title",
                "Fandom",
                "Date Posted",
                "Date Updated",
                "Word Count",
                "Chapter Count",
                "Hits",
                "Kudos",
                "Comments",
                "Bookmarks",
            };

            string response = await DisplayActionSheet("Sort Library", "Cancel", null, sortOptions);

            SortMethod toSortBy = LibraryHelper.StringToSortMethod(response);

            if (toSortBy != SortMethod.None)
            {
                _worksObservableCollection = new ObservableCollection<Work>(LibraryHelper.SortWorksBySortMethod(_worksObservableCollection.ToList(), toSortBy));
                worksListView.ItemsSource = _worksObservableCollection;
            }
        }
    }
}