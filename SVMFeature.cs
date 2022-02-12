using ML.WordNetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.SentimentAnalysis
{
    public class SVMFeature
    {
        public int SemanticNodeNumber { get; private set; }
        public string SemanticName
        {
            get
            {
                return WordNet.GetSemanticNode(this.SemanticNodeNumber) != null ? WordNet.GetSemanticNode(this.SemanticNodeNumber).NodeName : null;

            }
        }

        private Dictionary<int, FScore> _featureSegmentScoreValues = new Dictionary<int, FScore>();

        internal SVMFeature(int semanticNodeNumber)
        {
            this.SemanticNodeNumber = semanticNodeNumber;
        }

        public  string ToExportString(int segmentNumber)
        {
            
            return _featureSegmentScoreValues[segmentNumber].ToExportString();
        }
        
        public double GetFeatureScore(int segmentNumber,FeatureScoreType fscoreType)
        {
            if (!_featureSegmentScoreValues.ContainsKey(segmentNumber)) return 0;

            var segmentFeatureScore = _featureSegmentScoreValues[segmentNumber];
            switch (fscoreType)
            {
                case FeatureScoreType.FSCore:
                    return segmentFeatureScore.FScoreValue;
                case FeatureScoreType.TFIDFScore:
                    return segmentFeatureScore.TFIDFScore;
                case FeatureScoreType.Alternative1:
                    return segmentFeatureScore.AlternativeFScore1;
                case FeatureScoreType.Alternative2:
                    return segmentFeatureScore.AlternativeFScore2;
                case FeatureScoreType.Static:
                    return 1;
                default:
                    return 0;
            }

        }
        internal void AddFScore(int segmentNumber,FScore score)
        {
            _featureSegmentScoreValues.Add(segmentNumber, score);
        }

       
    }
}
