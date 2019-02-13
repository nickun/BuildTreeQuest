using System.Collections.Generic;
using System.Diagnostics;
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

        //[DebuggerDisplay("Id: {ID}, Children: {ChildrenCount}")]
        public class Node
        {
            public string ID;

            private ProjectLine _nodeItem;
            private IDictionary<string, Node> _children;

            public int ChildrenCount => _children?.Count ?? 0;

            public ProjectLine NodeItem
            {
                get { return _nodeItem; }
                set
                {
                    // take parent from the node
                    var parentItem = ParentNode?.NodeItem;
                    if (parentItem != null && value != null)
                        value.ParentId = parentItem.Id;

                    // set me as parent for all my children
                    if (_nodeItem == null && value != null)
                        _children?.Values.ForEachExt(c => c.SetParentNodeItemId(value.Id));
                    _nodeItem = value;
                }
            }

            public Node ParentNode { get; }

            public Node(string id)
            {
                ID = id;
            }

            private Node(string id, Node parentNode)
            {
                ID = id;
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

            private void SetParentNodeItemId(int parentId)
            {
                if (_nodeItem != null)
                    _nodeItem.ParentId = parentId;
            }
        }
    }
}