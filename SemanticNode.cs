using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    [Serializable]
    public class SemanticNode
    {
       
        public int SemanticNumber { get; private set; }

        public static SemanticNode Empty { get; private set; }
        static SemanticNode()
        {
            Empty = new SemanticNode(0,string.Empty);
        }

        private HashSet<string> _connectedNodes = new HashSet<string>();


        public string NodeName
        {
            get;
            private set;

        }
        public IEnumerable<WordNode> ConnectedNodes
        {
            get { return _connectedNodes.Select(word=>word.ToWordNode()); }
        }
        public int ConnectedNodesCount
        {
            get { return _connectedNodes.Count; }
        }
        internal bool AddNode(WordNode node)
        {
            if (this == SemanticNode.Empty) return false;
            if (this._connectedNodes.Contains(node.Word)) return false;
            this._connectedNodes.Add(node.Word);
            return true;
        }
        internal SemanticNode(int semanticNumber,string name)
        {
            this.SemanticNumber = semanticNumber;
            this.NodeName = name;
        }

        public override string ToString()
        {
            return  string.Format("[#{0}# - {1}]",
                this.SemanticNumber,
                this._connectedNodes.Count>3?this.NodeName:
                this._connectedNodes.Aggregate((current, next) => current + " / " + next)
                );
        }

        internal bool DropNode(WordNode node)
        {
            if (node == WordNode.Empty) return false;
            if (!_connectedNodes.Contains(node.Word)) return false;
            _connectedNodes.Remove(node.Word);
            return true;
        }
      
    }
}
