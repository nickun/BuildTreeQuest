using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildTreeQuestConsole.Helpers
{
    /// <summary>
    /// Compare two chapter strings like: 1.1.1 and 1.2.1
    /// </summary>
    public class ChapterComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null)
                return y != null ? -1 : 0;
            if (y == null)
                return 1;
            return CompareChapters(x, y);
        }

        public static int CompareChapters(string chapter1, string chapter2)
        {
            var intComparer = Comparer<int>.Default;

            var splitChapter1 = chapter1.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var splitChapter2 = chapter2.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            int minLength = Math.Min(splitChapter1.Length, splitChapter2.Length);

            for (int index = 0; index < minLength; index++)
            {
                var strChapter1 = splitChapter1[index];
                int intChapt1Part = 0;
                if (!int.TryParse(strChapter1, out intChapt1Part))
                {
                    var notDigitCh = strChapter1.First(ch => !char.IsDigit(ch));
                    int.TryParse(strChapter1.Substring(0, strChapter1.IndexOf(notDigitCh)), out intChapt1Part);
                }

                var strChapter2 = splitChapter2[index];
                int intChapt2Part = 0;
                if (!int.TryParse(strChapter2, out intChapt2Part))
                {
                    var notDigitCh = strChapter2.First(ch => !char.IsDigit(ch));
                    int.TryParse(strChapter2.Substring(0, strChapter2.IndexOf(notDigitCh)), out intChapt2Part);
                }

                int res = intComparer.Compare(intChapt1Part, intChapt2Part);
                if (res != 0)
                    return res;
            }

            return intComparer.Compare(splitChapter1.Length, splitChapter2.Length);
        }
    }
}