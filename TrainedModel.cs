using LibSVMsharp;
using LibSVMsharp.Helpers;
using ML.WordNetLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TurkishLanguageLibrary.Conversions;

namespace ML.SentimentAnalysis
{
    [Serializable]
    public class TrainedModel
    {
       // public SVMModel SVMModel { get;  internal set; }
        [NonSerialized]
        private SVMModel _svmModel;
     
        public SVMModel SVMModel
        {
            get
            {
                if (_svmModel != null) return _svmModel;
                _svmModel = xmlSerializedSVMModel.XmlDeSerialize<SVMModel>();
                return _svmModel;

            }
            internal set
            {
                _svmModel = value;
                xmlSerializedSVMModel = value.XmlSerialize();
            }
        }
     
        internal SVMSetting Settings { get; set; }

        internal byte[] xmlSerializedSVMModel = null;
       
        
      
        //public double CrossValidationAccuracy { get; internal set; }
        internal TrainedModel()
        {
            this.WordNetVersion = WordNet.Version;
        }

        public Guid WordNetVersion { get; private set; }
      
        private void ensureWornetVersions()
        {
            if (this.WordNetVersion != WordNet.Version)
                throw new Exception("Model şu andaki WordNet uzayında oluşturulmamış.");
        }
        public List<Prediction> Predict(IEnumerable<SemanticNode>sentenceSemanticNodes)
        {
            ensureWornetVersions();
           
            var svmNodes = sentenceSemanticNodes.GroupBy(p => p.SemanticNumber)
                .OrderBy(p => p.Key)
                .Select(p => new SVMNode()
                { 
                    Index = p.Key,
                    Value = p.Count()

                }).ToArray();
            SVMProblem problem = new SVMProblem();
          
            svmNodes = SVMNodeHelper.Normalize(svmNodes, this.Settings.SVMNormType);
           
            var prediction = Convert.ToInt32(SVM.Predict(this.SVMModel, svmNodes));
            var result=new List<Prediction>();
            result.Add(new Prediction()
            {
                SegmentName= WordNetLibrary.WordNet.GetSemanticNode(prediction).NodeName,
                Probability=1
            });
            return result;
           

        }

        public double CheckAccuracy(IEnumerable<TrainingItem> trainingItems)
        {
            ensureWornetVersions();
            int correct = 0;
            foreach (var item in trainingItems)
            {
                var semanticName=this.Predict(item.SentenceSemanticNodes.Select(p=>WordNetLibrary.WordNet.GetSemanticNode(p)));
                var found=item.FoundSegmentName=semanticName.First().SegmentName;
                if (item.RelatedSegments.Any(p => WordNet.GetSemanticNode(p).NodeName == found))
                {
                    item.IsFound = true;
                    correct++;
                }
            }
            return (double)correct / (double)trainingItems.Count();
        }


        
    }
}
