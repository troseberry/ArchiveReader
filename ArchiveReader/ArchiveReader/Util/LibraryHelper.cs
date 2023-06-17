using System;
using System.Collections.Generic;
using System.Text;

using ArchiveReader.Models;
using ArchiveReader.Enums;

namespace ArchiveReader.Util
{
    public class LibraryHelper
    {

        public static List<Work> SortWorksBySortMethod(List<Work> worksToSort, SortMethod sortMethod)
        {
            List<Work> sortedWorks = worksToSort;

            switch(sortMethod)
            {
                case SortMethod.WordCount:
                case SortMethod.Kudos:
                case SortMethod.Hits:
                case SortMethod.Bookmarks:
                case SortMethod.Comments:
                    sortedWorks.Sort((Work work01, Work work02) =>
                    {
                        // Ascending Sort
                        //return work01.GetParameterForSort(sortMethod).CompareTo(work02.GetParameterForSort(sortMethod));

                        int workOneStat = !String.IsNullOrWhiteSpace(work01.GetParameterForSort(sortMethod)) ? int.Parse(work01.GetParameterForSort(sortMethod)) : 0;
                        int workTwoStat = !String.IsNullOrWhiteSpace(work02.GetParameterForSort(sortMethod)) ? int.Parse(work02.GetParameterForSort(sortMethod)) : 0;

                        int result = workOneStat == workTwoStat ? 0 :
                        workOneStat > workTwoStat ? -1 : 1;
                        return result;
                    });
                    break;

                case SortMethod.Author:
                case SortMethod.Fandom:
                case SortMethod.Language:
                case SortMethod.Title:
                    sortedWorks.Sort((Work work01, Work work02) =>
                    {
                        var workOneParam = work01.GetParameterForSort(sortMethod);
                        var workTwoParam = work02.GetParameterForSort(sortMethod);

                        return workOneParam.CompareTo(workTwoParam);

                    });
                    break;
            }

            return sortedWorks;
        }

        public static SortMethod StringToSortMethod(string sortString)
        {
            switch(sortString)
            {
                case "Updated Date":
                    return SortMethod.RevisedDate;
                case "Title":
                    return SortMethod.Title;
                case "Author":
                    return SortMethod.Author;
                case "Fandom":
                    return SortMethod.Fandom;
                case "Word Count":
                    return SortMethod.WordCount;
                case "Chapter Count":
                    return SortMethod.ChapterCount;
                case "Kudos":
                    return SortMethod.Kudos;
                case "Bookmarks":
                    return SortMethod.Bookmarks;
                case "Hits":
                    return SortMethod.Hits;
                case "Comments":
                    return SortMethod.Comments;
                default:
                    return SortMethod.None;
            }
        }
    }
}
