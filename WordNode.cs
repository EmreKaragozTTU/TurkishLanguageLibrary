using ML.WordNetLibrary.Dictionary;
using net.zemberek.erisim;
using net.zemberek.tr.yapi;
using net.zemberek.yapi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    [Serializable]
    public class WordNode:IComparable<WordNode>
    {
     
        private HashSet<int> _semanticConnections = new HashSet<int>();
        private int _mainSemanticConnectionNumber;
        public SemanticNode MainSemanticNode
        {
            get { return WordNet.GetSemanticNode(this._mainSemanticConnectionNumber); }
            internal set
            {
                if (value == null)
                    throw new WordNetInvalidOperationException("MainSemanticNode can not be null");
                _mainSemanticConnectionNumber = value.SemanticNumber;
            }
        }
        
        
        internal bool AddSemanticNode(SemanticNode snode)
        {
            if (_mainSemanticConnectionNumber == snode.SemanticNumber) return false;
            if (_semanticConnections.Contains(snode.SemanticNumber)) return false;
            if (snode == SemanticNode.Empty) return false;
            _semanticConnections.Add(snode.SemanticNumber);
            return true;
        }

        public static WordNode Empty { get; private set; }
        public  IEnumerable<SemanticNode> ToSemanticNodes()
        {
            if (_semanticConnections.Count == 0)
                yield return MainSemanticNode;

            foreach (var snodeNumber in _semanticConnections)
                yield return WordNet.GetSemanticNode(snodeNumber);
            
        }
        static WordNode()
        {
            Empty = new WordNode(string.Empty, SemanticNode.Empty);
        }

        internal WordNode(string word,SemanticNode mainSemanticNode)
        {
            this.Word = word;
            this._mainSemanticConnectionNumber = mainSemanticNode.SemanticNumber;
        }
        public string Word { get; private set; }

        public override int GetHashCode()
        {
            return this.Word.GetHashCode();
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if(!(obj is WordNode)) return false;
            return this.Word == ((WordNode)obj).Word;
        }
        public static bool operator ==(WordNode node1,WordNode node2)
        {
            if ((object)node1 == null && (object)node2 == null) return true;
            return (object)node2 == null ? node1.Equals(node2) : node2.Equals(node1);
        }
        public static bool operator !=(WordNode node1, WordNode node2)
        {

            return !(node1 == node2);

        }
        public override string ToString()
        {
            return this.Word;
        }

        public int CompareTo(WordNode other)
        {
            return this.Word.CompareTo(other.Word);
        
        }

        public string ToSummaryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Kelime: ");
            sb.Append(this.Word);
            foreach (var connection in this._semanticConnections)
                sb.Append(connection);
            return sb.ToString();
        }

       
       

    }
}
