using System;
using System.Collections.Generic;
using System.Text;

namespace Hackaton.UniversalAdapter.Adapter.Parser.Helper.Creator
{
    /// <summary>
    /// Создание узла параграфа 
    /// </summary>
    public class ParNode
    {
        private string _key;
        public int NodeId { get; set; }
        public int? PrevNodeId { get; set; }
        public int NextNodeId { get; set; }
        public int CountArrInNode { get; set; }
        public ParentNode Parent { get; set; }
        public string Number { get; set; }
        public ParNode(string key,ParNode prevNode = null)
        {
            if (prevNode == null)
                InitIfPrevNodeIsNull(key);
            else
            {
                this.PrevNodeId = prevNode.PrevNodeId;
                this.NodeId = prevNode.NextNodeId;
                this.NextNodeId = NodeId + 1;
                this._key = key.Remove(key.LastIndexOf("."));
                this.Number = this._key;
                this.CountArrInNode = key.Split('.').Length;
                this.Parent = new ParentNode(this.CountArrInNode, prevNode);
            }
        }
        private void InitIfPrevNodeIsNull(string key)
        {
            this.PrevNodeId = null;
            this.NodeId = 0;
            this.NextNodeId = NodeId + 1;
            this._key = key.Remove(key.LastIndexOf("."));
            this.Number = this._key;
            this.CountArrInNode = key.Split('.').Length;
            this.Parent = new ParentNode(this.CountArrInNode);
        }
    }
}
