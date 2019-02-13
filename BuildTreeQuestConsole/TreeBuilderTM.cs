﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BuildTreeQuestConsole
{
    public class TreeBuilderTM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string chap(string chapter)
        {
            int dot = chapter.LastIndexOf('.');
            if (dot < 0) return null;
            return chapter.Substring(0, dot);
        }

        public static IList<ProjectLine> BuildTree(IList<ProjectLine> testData)
        {
            var nodeCollector = testData.ToDictionary(i => i.Chapter);
            var chapters = testData.OrderBy(c => c.Chapter.Split('.').Length);
            return chapters.Select(c => {
                var k = chap(c.Chapter);
                if (k == null) return c;
                if (!nodeCollector.TryGetValue(k, out var val) || !val.HasChildren) return null;
                val.HasChildren = true;
                c.ParentId = val.Id;
                return c;
            }).Where(c => c != null).ToList();
        }
    }
}