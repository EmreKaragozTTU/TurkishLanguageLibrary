using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.WordNetLibrary;
namespace ML.SentimentAnalysis
{
   [Serializable]
    public class TrainingItem
    {
       private HashSet<int> _containedSemanticList;
        private List<int> _semanticWordList = new List<int>();
        private HashSet<int> _relatedSegmentNumbers;

        public IEnumerable<int> DistinctSemanticNodes
        {
            get { return _containedSemanticList; }
        }
        public IEnumerable<int> RelatedSegments
        {
            get { return _relatedSegmentNumbers; }
        }
        public bool ContainsSegment(int segmentNumber)
        {
            return _relatedSegmentNumbers.Contains(segmentNumber);
        }
        public bool ContainsFeature(int semanticNumber)
        {
            return _containedSemanticList.Contains(semanticNumber);
        }
       public Guid TrainingId { get; set; }
       public bool IsFound { get; set; }
       public string FoundSegmentName { get; set; }
        public IEnumerable<int> SentenceSemanticNodes
        {
            get
            {
                return _semanticWordList.Select(p => p);
            }

        }
        public TrainingItem(IEnumerable<SemanticNode> sentenceSemanticNodes,IEnumerable<SemanticNode> relatedSegmentSemanticNodes)
        {
            this._semanticWordList.AddRange(sentenceSemanticNodes.Select(p=>p.SemanticNumber));
            this._containedSemanticList = new HashSet<int>(this._semanticWordList);
            this._relatedSegmentNumbers=new HashSet<int>(relatedSegmentSemanticNodes.Select(s=>s.SemanticNumber));

        }
       
    }
}
