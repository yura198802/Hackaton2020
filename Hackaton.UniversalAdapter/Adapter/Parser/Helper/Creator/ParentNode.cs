using System.Collections.Generic;

namespace Hackaton.UniversalAdapter.Adapter.Parser.Helper.Creator
{
    /// <summary>
    /// Создание родительского узла для узла параграфа(при чтении структурированных док-ов)
    /// </summary>
    public class ParentNode
    {
        private ParNode _prevParNode;
        private int _countArrInNode;
        public int Seek { get; set; }
        public int CurrentParentNode { get; set; }
        public List<int> CollectionParents { get; private set; }
        public ParentNode(int countArrInNode, ParNode prevNode = null)
        {
            this._prevParNode = prevNode;
            this._countArrInNode = countArrInNode;
            if (prevNode != null)
                InitParent(countArrInNode, prevNode);
            else
                InitIfPrevNodeIsNull(countArrInNode);
        }
        private void InitIfPrevNodeIsNull(int countArrInNode)
        {
            this.CurrentParentNode = -1;
            this.CollectionParents = null;
        }
        private void InitParent(int countArrInNode, ParNode prevNode)
        {
            CollectionParents = _prevParNode.Parent.CollectionParents ?? new List<int> { Seek };
            if (_prevParNode.CountArrInNode < _countArrInNode)
            {
                CollectionParents.Add(_prevParNode.NodeId);
            }
            if (_prevParNode.CountArrInNode > _countArrInNode)
            {
                var range = GetRange();
                CollectionParents.RemoveRange(range.Item1, range.Item2);
            }
            this.CurrentParentNode = CollectionParents[CollectionParents.Count - 1];
        }
        private (int, int) GetRange()
        {
            var count = _prevParNode.CountArrInNode - _countArrInNode;
            var index = CollectionParents.Count - count;
            return (index, count);
        }
    }
}
