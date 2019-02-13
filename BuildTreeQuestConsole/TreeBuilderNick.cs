using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BuildTreeQuestConsole.Helpers;

namespace BuildTreeQuestConsole
{
    
    public class TreeBuilderNick
    {
        public static IList<ProjectLine> BuildTree(IList<ProjectLine> testData)
        {
            var rootZeroNode = new Node("-");

            foreach (var item in testData)
            {
                Node itemNode = rootZeroNode;

                foreach (string chapterNum in item.Chapter.Split('.'))
                    itemNode = itemNode.GetOrCreateChildNode(chapterNum);

                itemNode.NodeItem = item;
            }

            return testData;
        }

#if DEBUG
        [DebuggerDisplay("Id: {ID}, Children: {ChildrenCount}")]
#endif
        public class Node
        {
#if DEBUG
            public string ID;
            public int ChildrenCount => _children?.Count ?? 0;
#endif
            private ProjectLine _nodeItem;
            private IDictionary<string, Node> _children;

            public ProjectLine NodeItem
            {
                get { return _nodeItem; }
                set
                {
                    // take parent from the node
                    var parentItem = ParentNode?.NodeItem;
                    if (parentItem != null && value != null)
                    {
                        value.ParentId = parentItem.Id;
                        parentItem.HasChildren = true;
                    }

                    // set me as parent for all my children
                    if (_nodeItem == null && value != null)
                    {
                        _children?.Values.ForEachExt(c => c.SetParentNodeItemId(value.Id));
                        value.HasChildren = _children?.Count > 0;
                    }
                    _nodeItem = value;
                }
            }

            public Node ParentNode { get; }

            public Node(string id)
            {
#if DEBUG
                ID = id;
#endif
            }

            private Node(string id, Node parentNode)
            {
#if DEBUG
                ID = id;
#endif
                ParentNode = parentNode;
            }

            public Node GetOrCreateChildNode(string childNum)
            {
                Node childNode;
                var children = _children;
                if (children == null)
                {
                    childNode = new Node(childNum, this);
                    _children = new Dictionary<string, Node> { { childNum, childNode } };
                    return childNode;
                }

                if (!children.TryGetValue(childNum, out childNode))
                    children[childNum] = childNode = new Node(childNum, this);

                return childNode;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void SetParentNodeItemId(int parentId)
            {
                if (_nodeItem != null)
                    _nodeItem.ParentId = parentId;
            }
        }
    }
}