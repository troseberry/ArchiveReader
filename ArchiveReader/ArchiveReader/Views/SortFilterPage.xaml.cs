using ArchiveReader.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchiveReader.Views
{
    public class SortFilterArgs
    {
        public SortOption sort = SortOption.DateUpdated;
        public CompletionStatus completion = CompletionStatus.None;
        public CrossoverOption crossover = CrossoverOption.None;

        public string otherIncludedTags = "";
        public string otherExcludedTags = "";
        public string searchWithinResults = "";
        public string dateFrom = "";
        public string dateTo = "";
        public string wordsFrom = "";
        public string wordsTo = "";
        public string language = "";

        public RatingsOption ratingsInclude = RatingsOption.None;
        public List<RatingsOption> ratingsExclude = new List<RatingsOption>();
        public List<WarningsOption> warningsInclude = new List<WarningsOption>();
        public List<WarningsOption> warningsExclude = new List<WarningsOption>();
        public List<CategoryOption> categoriesInclude = new List<CategoryOption>();
        public List<CategoryOption> categoriesExclude = new List<CategoryOption>();

        public string fandomsInclude = "";
        public string fandomsExclude = "";
        public string charactersInclude = "";
        public string charactersExclude = "";
        public string relationshipsInclude = "";
        public string relationshipsExclude = "";
        public string additionalTagsInclude = "";
        public string additionalTagsExclude = "";

        public SortFilterArgs()
        {
            
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortFilterPage : ContentPage
    {
        private SortFilterArgs _sortFIlterArgs;

        private RatingsOption _currentRatingIncludeOption;
        private CrossoverOption _currentCrossoverOption;
        private CompletionStatus _currentCompletionOption;
        public SortFilterPage()
        {
            InitializeComponent();
        }

        private void SetSortFilterArgs()
        {
            _sortFIlterArgs = new SortFilterArgs();

            _sortFIlterArgs.sort = SortPickerOptionToEnum(sortByPicker.SelectedItem);
            _sortFIlterArgs.completion = _currentCompletionOption;
            _sortFIlterArgs.crossover = _currentCrossoverOption;

            _sortFIlterArgs.otherIncludedTags = otherTagsIncludeEntry.Text;
            _sortFIlterArgs.otherExcludedTags = otherTagsExcludeEntry.Text;
            _sortFIlterArgs.searchWithinResults = searchWithinResultsEntry.Text;
            _sortFIlterArgs.dateFrom = dateUpdatedFromEntry.Text;
            _sortFIlterArgs.dateTo = dateUpdatedToEntry.Text;
            _sortFIlterArgs.wordsFrom = wordCountFromEntry.Text;
            _sortFIlterArgs.wordsTo = wordCountToEntry.Text;
            //language

            #region Ratings
            _sortFIlterArgs.ratingsInclude = _currentRatingIncludeOption;
            if (generalRatingExcludeCheckbox.IsChecked) _sortFIlterArgs.ratingsExclude.Add(RatingsOption.General);
            if (teenRatingExcludeCheckbox.IsChecked) _sortFIlterArgs.ratingsExclude.Add(RatingsOption.Teen);
            if (matureRatingExcludeCheckbox.IsChecked) _sortFIlterArgs.ratingsExclude.Add(RatingsOption.Mature);
            if (explicitRatingExcludeCheckbox.IsChecked) _sortFIlterArgs.ratingsExclude.Add(RatingsOption.Explicit);
            if (noRatingExcludeCheckbox.IsChecked) _sortFIlterArgs.ratingsExclude.Add(RatingsOption.NotRated);
            #endregion

            #region Warnings
            if (noWarningIncludeCheckbox.IsChecked) _sortFIlterArgs.warningsInclude.Add(WarningsOption.NoArchiveWarningsApply);
            if (choseNotToUseWarningIncludeCheckbox.IsChecked) _sortFIlterArgs.warningsInclude.Add(WarningsOption.CreatorChoseNotToUseArchiveWarnings);
            if (graphicViolenceWarningIncludeCheckbox.IsChecked) _sortFIlterArgs.warningsInclude.Add(WarningsOption.GraphicDepictionsOfViolence);
            if (majorcharacterDeathWarningIncludeCheckbox.IsChecked) _sortFIlterArgs.warningsInclude.Add(WarningsOption.MajorCharacterDeath);
            if (nonConWarningIncludeCheckbox.IsChecked) _sortFIlterArgs.warningsInclude.Add(WarningsOption.RapeNonCon);
            if (underageWarningIncludeCheckbox.IsChecked) _sortFIlterArgs.warningsInclude.Add(WarningsOption.Underage);

            if (noWarningExcludeCheckbox.IsChecked) _sortFIlterArgs.warningsExclude.Add(WarningsOption.NoArchiveWarningsApply);
            if (choseNotToUseWarningExcludeCheckbox.IsChecked) _sortFIlterArgs.warningsExclude.Add(WarningsOption.CreatorChoseNotToUseArchiveWarnings);
            if (graphicViolenceWarningExcludeCheckbox.IsChecked) _sortFIlterArgs.warningsExclude.Add(WarningsOption.GraphicDepictionsOfViolence);
            if (majorcharacterDeathWarningExcludeCheckbox.IsChecked) _sortFIlterArgs.warningsExclude.Add(WarningsOption.MajorCharacterDeath);
            if (nonConWarningExcludeCheckbox.IsChecked) _sortFIlterArgs.warningsExclude.Add(WarningsOption.RapeNonCon);
            if (underageWarningExcludeCheckbox.IsChecked) _sortFIlterArgs.warningsExclude.Add(WarningsOption.Underage);
            #endregion

            #region Categories
            if (fmCategoryIncludeCheckbox.IsChecked) _sortFIlterArgs.categoriesInclude.Add(CategoryOption.FM);
            if (mmCategoryIncludeCheckbox.IsChecked) _sortFIlterArgs.categoriesInclude.Add(CategoryOption.MM);
            if (ffCategoryIncludeCheckbox.IsChecked) _sortFIlterArgs.categoriesInclude.Add(CategoryOption.FF);
            if (genCategoryIncludeCheckbox.IsChecked) _sortFIlterArgs.categoriesInclude.Add(CategoryOption.Gen);
            if (multiCategoryIncludeCheckbox.IsChecked) _sortFIlterArgs.categoriesInclude.Add(CategoryOption.Multi);
            if (otherCategoryIncludeCheckbox.IsChecked) _sortFIlterArgs.categoriesInclude.Add(CategoryOption.Other);

            if (fmCategoryExcludeCheckbox.IsChecked) _sortFIlterArgs.categoriesExclude.Add(CategoryOption.FM);
            if (mmCategoryExcludeCheckbox.IsChecked) _sortFIlterArgs.categoriesExclude.Add(CategoryOption.MM);
            if (ffCategoryExcludeCheckbox.IsChecked) _sortFIlterArgs.categoriesExclude.Add(CategoryOption.FF);
            if (genCategoryExcludeCheckbox.IsChecked) _sortFIlterArgs.categoriesExclude.Add(CategoryOption.Gen);
            if (multiCategoryExcludeCheckbox.IsChecked) _sortFIlterArgs.categoriesExclude.Add(CategoryOption.Multi);
            if (otherCategoryExcludeCheckbox.IsChecked) _sortFIlterArgs.categoriesExclude.Add(CategoryOption.Other);
            #endregion
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SortFilterButton_Clicked(object sender, EventArgs e)
        {
            SetSortFilterArgs();
            MessagingCenter.Send(this, "SortAndFilter", _sortFIlterArgs);
            await Navigation.PopModalAsync();
        }

        private void RatingsIncludeCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((CheckBox)sender == generalRatingIncludeCheckbox && generalRatingIncludeCheckbox.IsChecked)
            {
                _currentRatingIncludeOption = RatingsOption.General;

                teenRatingIncludeCheckbox.IsChecked = false;
                matureRatingIncludeCheckbox.IsChecked = false;
                explicitRatingIncludeCheckbox.IsChecked = false;
                noRatingIncludeCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == teenRatingIncludeCheckbox && teenRatingIncludeCheckbox.IsChecked)
            {
                _currentRatingIncludeOption = RatingsOption.Teen;

                generalRatingIncludeCheckbox.IsChecked = false;
                matureRatingIncludeCheckbox.IsChecked = false;
                explicitRatingIncludeCheckbox.IsChecked = false;
                noRatingIncludeCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == matureRatingIncludeCheckbox && matureRatingIncludeCheckbox.IsChecked)
            {
                _currentRatingIncludeOption = RatingsOption.Mature;

                generalRatingIncludeCheckbox.IsChecked = false;
                teenRatingIncludeCheckbox.IsChecked = false;
                explicitRatingIncludeCheckbox.IsChecked = false;
                noRatingIncludeCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == explicitRatingIncludeCheckbox && explicitRatingIncludeCheckbox.IsChecked)
            {
                _currentRatingIncludeOption = RatingsOption.Explicit;

                generalRatingIncludeCheckbox.IsChecked = false;
                teenRatingIncludeCheckbox.IsChecked = false;
                matureRatingIncludeCheckbox.IsChecked = false;
                noRatingIncludeCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == noRatingIncludeCheckbox && noRatingIncludeCheckbox.IsChecked)
            {
                _currentRatingIncludeOption = RatingsOption.NotRated;

                generalRatingIncludeCheckbox.IsChecked = false;
                teenRatingIncludeCheckbox.IsChecked = false;
                matureRatingIncludeCheckbox.IsChecked = false;
                explicitRatingIncludeCheckbox.IsChecked = false;
            }
            Console.WriteLine("[RatingsIncludeCheckbox_CheckedChanged] Current Rating: " + _currentRatingIncludeOption);
        }

        private void CrossoverCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
           if ((CheckBox)sender == includeCrossoversCheckbox && includeCrossoversCheckbox.IsChecked)
            {
                _currentCrossoverOption = CrossoverOption.IncludeCrossovers;
                excludeCrossoversCheckbox.IsChecked = false;
                onlyCrossoversCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == excludeCrossoversCheckbox && excludeCrossoversCheckbox.IsChecked)
            {
                _currentCrossoverOption = CrossoverOption.ExcludeCrossovers;
                includeCrossoversCheckbox.IsChecked = false;
                onlyCrossoversCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == onlyCrossoversCheckbox && onlyCrossoversCheckbox.IsChecked)
            {
                _currentCrossoverOption = CrossoverOption.OnlyCrossovers;
                includeCrossoversCheckbox.IsChecked = false;
                excludeCrossoversCheckbox.IsChecked = false;
            }
        }

        private void CompletionCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((CheckBox)sender == allWorksCompletionCheckbox && allWorksCompletionCheckbox.IsChecked)
            {
                _currentCompletionOption = CompletionStatus.All;
                completeWorksCompletionCheckbox.IsChecked = false;
                inProgressWorksCompletionCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == completeWorksCompletionCheckbox && completeWorksCompletionCheckbox.IsChecked)
            {
                _currentCompletionOption = CompletionStatus.CompleteOnly;
                allWorksCompletionCheckbox.IsChecked = false;
                inProgressWorksCompletionCheckbox.IsChecked = false;
            }
            else if ((CheckBox)sender == inProgressWorksCompletionCheckbox && inProgressWorksCompletionCheckbox.IsChecked)
            {
                _currentCompletionOption = CompletionStatus.InProgressOnly;
                allWorksCompletionCheckbox.IsChecked = false;
                completeWorksCompletionCheckbox.IsChecked = false;
            }
        }


        private SortOption SortPickerOptionToEnum(object item)
        {
            switch(item.ToString())
            {
                case "Author":
                    return SortOption.Author;
                case "Title":
                    return SortOption.Title;
                case "Date Posted":
                    return SortOption.DatePosted;
                case "Date Updated":
                    return SortOption.DateUpdated;
                case "Word Count":
                    return SortOption.WordCount;
                case "Hits":
                    return SortOption.Hits;
                case "Kudos":
                    return SortOption.Kudos;
                case "Comments":
                    return SortOption.Comments;
                case "Bookmarks":
                    return SortOption.Bookmarks;
                default:
                    return SortOption.None;
            }
        }
    }
}