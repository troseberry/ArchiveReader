using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArchiveReader.ViewModels
{
    class ReaderViewModel : BaseViewModel
    {
        public string workId;
        public string latestChapterId;
        public string chapterNumber;

        public ReaderViewModel()
        {
            Title = "Reader";
        }
        
    }
}
