using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiveReader.Enums
{
    public enum CompletionStatus
    {
        None = 0,
        All,
        CompleteOnly,
        InProgressOnly
    }

    public enum CrossoverOption
    {
        None = 0,
        IncludeCrossovers,
        ExcludeCrossovers,
        OnlyCrossovers
    }

    public enum SortOption
    {
        None = 0,
        Author,
        Title,
        DatePosted,
        DateUpdated,
        WordCount,
        Hits,
        Kudos,
        Comments,
        Bookmarks
    }

    public enum RatingsOption
    {
        None = 0,
        General,
        Teen,
        Mature,
        Explicit,
        NotRated
    }

    public enum WarningsOption
    {
        None = 0,
        NoArchiveWarningsApply,
        CreatorChoseNotToUseArchiveWarnings,
        GraphicDepictionsOfViolence,
        MajorCharacterDeath,
        RapeNonCon,
        Underage
    }

    public enum CategoryOption
    {
        None = 0,
        MM,
        FM,
        FF,
        Gen,
        Multi,
        Other
    }
}
