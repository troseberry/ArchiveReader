using System;
using System.Collections.Generic;
using System.Text;

namespace ArchiveReader.Models
{
    public class Work
    {
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


        public string AllStats { get; set; }
        public string[] RequiredTagsArray { get; set; }

        
         public Work()
        {
            if (!string.IsNullOrWhiteSpace(language))
            {
                AllStats += $"Language: {language}";
            }

            if (!string.IsNullOrWhiteSpace(wordCount))
            {
                AllStats += $" Words: {wordCount}";
            }

            if (!string.IsNullOrWhiteSpace(chapterCount))
            {
                AllStats += $" Chapters: {chapterCount}";
            }

            if (!string.IsNullOrWhiteSpace(collections))
            {
                AllStats += $" Collections: {collections}";
            }

            if (!string.IsNullOrWhiteSpace(comments))
            {
                AllStats += $" Comments: {comments}";
            }

            if (!string.IsNullOrWhiteSpace(kudos))
            {
                AllStats += $" Kudos: {kudos}";
            }

            if (!string.IsNullOrWhiteSpace(bookmarks))
            {
                AllStats += $" Bookmarks: {bookmarks}";
            }

            if (!string.IsNullOrWhiteSpace(hits))
            {
                AllStats += $" Hits: {hits}";
            }
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

        public override string ToString()
        {
            return $"Work ID: {id} | Title: {title}";
        }
    }
}
