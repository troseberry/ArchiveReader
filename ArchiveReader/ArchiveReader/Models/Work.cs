using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

using SQLite;

using ArchiveReader.Enums;

namespace ArchiveReader.Models
{
    public class Work
    {
        [PrimaryKey, AutoIncrement]
        public int dbID { get; set; }

        public string id { get; set; }
        public string title { get; set; }
        public string author { get; set; }

        public string recipientUsers { get; set; }
        public string fandoms { get; set; }
        public string requiredTags { get; set; }

        public string lastUpdated { get; set; }
        public string tags { get; set; }
        public string summary { get; set; }
        public string series { get; set; }
        public string seriesIds { get; set; }

        public string language { get; set; }
        public string wordCount { get; set; }
        public string chapterCount { get; set; }
        public string collections { get; set; }
        public string comments { get; set; }
        public string kudos { get; set; }
        public string bookmarks { get; set; }
        public string hits { get; set; }

        public string latestChapterId { get; set; }

        [Ignore]
        public string AllStats 
        {
            get 
            {
                string output = "";
                #region Func
                if (!string.IsNullOrWhiteSpace(language))
                {
                    output += $"Language: {language}";
                }

                if (!string.IsNullOrWhiteSpace(wordCount))
                {
                    output += $" | Words: {wordCount}";
                }

                if (!string.IsNullOrWhiteSpace(chapterCount))
                {
                    output += $" | Chapters: {chapterCount}";
                }

                if (!string.IsNullOrWhiteSpace(collections))
                {
                    output += $" | Collections: {collections}";
                }

                if (!string.IsNullOrWhiteSpace(comments))
                {
                    output += $" | Comments: {comments}";
                }

                if (!string.IsNullOrWhiteSpace(kudos))
                {
                    output += $" | Kudos: {kudos}";
                }

                if (!string.IsNullOrWhiteSpace(bookmarks))
                {
                    output += $" | Bookmarks: {bookmarks}";
                }

                if (!string.IsNullOrWhiteSpace(hits))
                {
                    output += $" | Hits: {hits}";
                }
                #endregion
                return output; 
            }
        }

        [Ignore]
        public string ReadingStats { get; set; }

        [Ignore]
        public string[] RequiredTagsArray
        {
            get
            {
                string[] output = new string[4];

                string[] reqStrings = requiredTags.Split('[');
                reqStrings = reqStrings.Skip(1).ToArray();

                for (int i = 0; i < reqStrings.Length; i++)
                {
                    string result = reqStrings[i];
                    if (result.Contains("],"))
                    {
                        result = result.Replace("],", "");
                    }
                    else
                    {
                        result = result.Replace("]", "");
                    }

                    result = result.Trim();
                    output[i] = GetReqTagImageForString(result, i);
                }

                return output;
            }
        }

        
         public Work()
        {

        }

        public void GenerateExtraInfo()
        {

        }

        string GetReqTagImageForString(string tagStr, int index)
        {
            if (index == 1 & !tagStr.Equals("No Archive Warnings Apply"))
            {
                return "RequiredTag_Warning_OneOrMore.png";
            }

            if (index == 2 & tagStr.Contains(","))
            {
                return "RequiredTag_Pairing_Multi.png";
            }

            

            switch(tagStr)
            {
                case "No category":
                    return "RequiredTag_None.png";

                case "General Audiences":
                    return "RequiredTag_Rating_General.png";
                case "Teen And Up Audiences":
                    return "RequiredTag_Rating_Teen.png";
                case "Mature":
                    return "RequiredTag_Rating_Mature.png";
                case "Explicit":
                    return "RequiredTag_Rating_Explicit.png";
                case "Not Rated":
                    return "RequiredTag_None.png";

                case "F/M":
                    return "RequiredTag_Pairing_FM.png";
                case "F/F":
                    return "RequiredTag_Pairing_FF.png";
                case "M/M":
                    return "RequiredTag_Pairing_MM.png";
                case "Gen":
                    return "RequiredTag_Pairing_Gen.png";
                case "Multi":
                    return "RequiredTag_Pairing_Multi.png";
                case "Other":
                    return "RequiredTag_Pairing_Other.png";

                case "Work in Progress":
                    return "RequiredTag_Progress_Incomplete.png";
                case "Complete Work":
                    return "RequiredTag_Progress_Complete.png";

                case "Choose Not To Use Archive Warnings":
                    return "RequiredTag_Warning_AuthorChoseNot.png";
                case "No Archive Warnings Apply":
                    return "RequiredTag_None.png";
                case "External Work":
                    return "RequiredTag_Warning_ExternalWork.png";
            }
            
            return "NO VALID TAG FOUND for " + tagStr;
        }
        /*
         public Work(string id, string title, string author, string recipientUsers = "", string fandoms = "", string requiredTags = "",
                 string lastUpdated = "", string tags = "", string summary = "", string series = "", string seriesIds = "",
                 string language = "", string wordCount = "", string chapterCount = "", string collections = "", 
                 string comments = "", string kudos = "", string bookmarks = "", string hits = "")
         {
             this.id = id;
             this.title = title;
             this.author = author;

             this.recipientUsers = recipientUsers;
             this.fandoms = fandoms;
             this.requiredTags = requiredTags;

             this.lastUpdated = lastUpdated;
             this.tags = tags;
             this.summary = summary;
             this.series = series;
             this.seriesIds = seriesIds;

             this.language = language;
             this.wordCount = wordCount;
             this.chapterCount = chapterCount;
             this.collections = collections;
             this.comments = comments;
             this.kudos = kudos;
             this.bookmarks = bookmarks;
             this.hits = hits;
         }
         */

        public string GetParameterForSort(SortMethod method)
        {
            switch(method)
            {
                //case SortMethod.AddedDate:
                    //return "";
                case SortMethod.Author:
                    return author;
                case SortMethod.Bookmarks:
                    return bookmarks;
                case SortMethod.ChapterCount:
                    return chapterCount;
                case SortMethod.Comments:
                    return comments;
                case SortMethod.Fandom:
                    return fandoms;
                case SortMethod.Hits:
                    return hits;
                case SortMethod.Kudos:
                    return kudos;
                case SortMethod.Language:
                    return language;
                case SortMethod.RevisedDate:
                    return lastUpdated;
                case SortMethod.Title:
                    return title;
                case SortMethod.WordCount:
                    return wordCount;
                default:
                    return "";
            }
        }

        public override string ToString()
        {
            return $"Work ID: {id} | Title: {title}";
        }
    }
}
