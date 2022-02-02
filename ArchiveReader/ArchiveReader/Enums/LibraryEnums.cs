using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiveReader.Enums
{
    public enum SortMethod
    {
        None = 0,
        AddedDate,
        RevisedDate,
        Title,
        Author,
        Fandom,
        Language,
        WordCount,
        ChapterCount,
        Kudos,
        Bookmarks,
        Hits,
        Comments
    }

    public enum GroupMethod
    {
        None = 0
    }
}
