using System;
using System.Collections.Generic;
using System.Text;

using ArchiveReader.Enums;

namespace ArchiveReader.Util
{
    public static class SortFilterHelper
    {
        public static string SFEnumToUrlString(Enum e)
        {
            if (e is CompletionStatus)
            {
                switch (e)
                {
                    case CompletionStatus.CompleteOnly:
                        return "T";
                    case CompletionStatus.InProgressOnly:
                        return "F";
                    default:
                        return "";
                }
            }

            if (e is CrossoverOption)
            {
                switch (e)
                {
                    case CrossoverOption.IncludeCrossovers:
                        return "T";
                    case CrossoverOption.ExcludeCrossovers:
                        return "F";
                    default:
                        return "";
                }
            }

            if (e is SortOption)
            {
                switch (e)
                {
                    case SortOption.Author:
                        return "author";
                    case SortOption.Title:
                        return "title";
                    case SortOption.DatePosted:
                        return "datePosted";
                    case SortOption.DateUpdated:
                        return "dateUpdated";
                    case SortOption.WordCount:
                        return "wordCount";
                    case SortOption.Hits:
                        return "hits";
                    case SortOption.Kudos:
                        return "kudos";
                    case SortOption.Comments:
                        return "comments";
                    case SortOption.Bookmarks:
                        return "bookmarks";
                    default:
                        return "";
                }
            }

            if (e is RatingsOption)
            {
                switch (e)
                {
                    case RatingsOption.General:
                        return "general audiences";
                    case RatingsOption.Teen:
                        return "teen and up audiences";
                    case RatingsOption.Mature:
                        return "mature";
                    case RatingsOption.Explicit:
                        return "explicit";
                    case RatingsOption.NotRated:
                        return "not rated";
                    default:
                        return "";
                }
            }

            if (e is WarningsOption)
            {
                switch (e)
                {
                    case WarningsOption.NoArchiveWarningsApply:
                        return "no archive warnings apply";
                    case WarningsOption.CreatorChoseNotToUseArchiveWarnings:
                        return "creator chose not to use archive warnings";
                    case WarningsOption.GraphicDepictionsOfViolence:
                        return "graphic depictions of violence";
                    case WarningsOption.MajorCharacterDeath:
                        return "major character death";
                    case WarningsOption.RapeNonCon:
                        return "rape/non-con";
                    case WarningsOption.Underage:
                        return "underage";
                    default:
                        return "";
                }
            }

            if (e is CategoryOption)
            {
                switch (e)
                {
                    case CategoryOption.MM:
                        return "m/m";
                    case CategoryOption.FM:
                        return "f/m";
                    case CategoryOption.FF:
                        return "f/f";
                    case CategoryOption.Gen:
                        return "gen";
                    case CategoryOption.Multi:
                        return "multi";
                    case CategoryOption.Other:
                        return "other";
                    default:
                        return "";
                }
            }

            return "";
        }
    }
}
