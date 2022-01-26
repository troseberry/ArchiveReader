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
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            //Shell.Current.FlyoutIsPresented = true;
            //NavToReader();
            PushFilterModal();
        }

        private async void NavToReader()
        {
            await Navigation.PushAsync(new WorkDetailPage());
        }

        private async void PushFilterModal()
        {
            await Navigation.PushModalAsync(new SortFilterPage());
        }
    }
}