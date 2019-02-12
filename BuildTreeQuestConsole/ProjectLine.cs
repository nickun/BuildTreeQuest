using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildTreeQuestConsole.Helpers;

namespace BuildTreeQuestConsole
{
    [DebuggerDisplay("Chapter: {Chapter}, Id: {Id}, ParentId: {ParentId}")]
    public class ProjectLine : IComparable
    {
        public int Id { get; set; }

        public string Chapter { get; set; }

        public int ParentId { get; set; }

        public bool HasChildren { get; set; }


        public string ChapterSort1
        {
            get
            {
                var paddedNumbers = Chapter.Split('.').Select(num => num.PadLeft(5, '0'));
                return string.Concat(paddedNumbers);
            }
        }


        #region Compare methods

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var other = obj as ProjectLine;
            if (other != null)
            {
                int res = ChapterComparer.CompareChapters(Chapter, other.Chapter);
                return res;
            }

            return -1;
        }

        #endregion // Compare methods
    }
}