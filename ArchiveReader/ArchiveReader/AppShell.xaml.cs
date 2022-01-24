using ArchiveReader.ViewModels;
using ArchiveReader.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace ArchiveReader
{
    public class TestItem
    {
        public string Title { get; set; }
    }

    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ObservableCollection<TestItem> BindableFlyoutItems { get; set; }
        //public ObservableCollection<dynamic> FlyoutItems;
        public static AppShell Instance;

        public AppShell()
        {
            InitializeComponent();
            Debug.WriteLine("Init AppShell");

            Instance = this;
            /*
            MenuItem test = new MenuItem
            {
                Text = "Chapter 1",
                IconImageSource = "icon_feed.png",
                Command = new Command( () => Debug.WriteLine("Command executed")),
            };

            MainShell.Items.Add(test);
            */
        }
    }
}
