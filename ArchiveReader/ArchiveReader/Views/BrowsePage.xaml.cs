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
    public partial class BrowsePage : ContentPage
    {
        public BrowsePage()
        {
            InitializeComponent();
        }

        private void FandomSelected(object sender, EventArgs e)
        {
            
            StackLayout selected = (StackLayout)sender;
            
            switch(selected.Id.ToString())
            {
                case "animeMangaFandom":
                    break;
                case "booksLitFandom":
                    break;
                case "cartoonsFandom":
                    break;
                case "celebritiesFandom":
                    break;
                case "moviesFandom":
                    break;
                case "musicBandsFandom":
                    break;
                case "otherMediaFandom":
                    break;
                case "theaterFandom":
                    break;
                case "tvShowsFandom":
                    break;
                case "videoGamesFandom":
                    break;
                case "uncategorizedFandom":
                    break;
                default:
                    break;
            }
        }
    }
}