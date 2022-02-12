using LibSVMsharp;
using LibSVMsharp.Helpers;
using ML.WordNetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.SentimentAnalysis
{
    public class TrainingSet:IEnumerable<TrainingItem>
    {
        private List<TrainingItem> trainingSentences = new List<TrainingItem>();
        private Dictionary<int, SVMFeature> featureScoreValues = new Dictionary<int, SVMFeature>();
        
        private TrainingSet() { }
        public static TrainingSet Create(IEnumerable<TrainingItem> sentences)
        {
            TrainingSet trainSet = new TrainingSet();
            trainSet.trainingSentences.AddRange(sentences);
            trainSet.prepareFeatureScores();
            return trainSet;

        }

        public string ExportFeatureScoreString(FeatureScoreType fscoreType)
        {
            StringBuilder sb = new StringBuilder();
            var segmentList = this.trainingSentences.SelectMany(s => s.RelatedSegments).Distinct().ToArray();
            sb.Append("Segment/Feature;");
            sb.Append(segmentList.Select(p => WordNet.GetSemanticNode(p).NodeName).Aggregate((current, next) => current + ";" + next));
            var featureList = this.trainingSentences.SelectMany(ts => ts.SentenceSemanticNodes).Distinct().ToArray();
            foreach(var featureNumber in featureList)
            {
                sb.Append(Environment.NewLine);
                sb.Append(WordNet.GetSemanticNode(featureNumber).NodeName + ";");
                foreach(var segmentNumber in segmentList)
                {
                    var value = featureScoreValues[featureNumber].GetFeatureScore(segmentNumber, fscoreType).ToString() +";";
                    sb.Append(value);
                }
            }
            return sb.ToString();

        }

        public IEnumerable<SVMFeature> SVMFeatures
        {
            get
            {
                return this.featureScoreValues.Select(f => f.Value);
            }
        }
        public TrainedModel Train(SVMSetting svmSettings)
        {
            TrainedModel model = new TrainedModel();
            SVMProblem problem = this.ToSVMProblem(svmSettings);
            model.SVMModel = SVM.Train(problem, svmSettings.SVMParameter);
           
            model.Settings = svmSettings;
            bool isCheckProbability=SVM.CheckProbabilityModel(model.SVMModel);
            return model;
        }

      
       
        public SVMProblem ToSVMProblem(SVMSetting settings)
        {

            SVMProblem problem = new SVMProblem();
            foreach (var sentece in this.trainingSentences)
            {
                var semanticNodesGrouped = sentece.SentenceSemanticNodes
                    .GroupBy(snodeNumber => snodeNumber).OrderBy(p=>p.Key);
               foreach(var segment in sentece.RelatedSegments)
               {
                   var svmNodes = semanticNodesGrouped.Select(snodeNumber =>
                         new SVMNode(snodeNumber.Key, (double) snodeNumber.Count() * featureScoreValues[snodeNumber.Key].GetFeatureScore(segment, settings.FeatureScoreType))).ToArray();

                   problem.Add(svmNodes, (double)segment);

               }
              
                    
               

            }
            problem = SVMProblemHelper.Normalize(problem, settings.SVMNormType);
            double[] targets;
            SVM.CrossValidation(problem, settings.SVMParameter, 10, out targets);
            double crossValidationAccuracy = SVMHelper.EvaluateClassificationProblem(problem, targets);
            return problem;

        }
        
        
      
        
        private void prepareFeatureScores()
        {
            var sentenceCount= this.trainingSentences.Count;
            var segmentList = new HashSet<int>(this.trainingSentences.SelectMany(s => s.RelatedSegments).Distinct());
            var featureList = this.trainingSentences.SelectMany(ts => ts.SentenceSemanticNodes).Distinct().ToArray();
         
            Dictionary<int, Dictionary<int, int>> featureSegmentMap = new Dictionary<int, Dictionary<int, int>>();
            foreach (var segmentNumber in segmentList)
                featureSegmentMap.Add(segmentNumber, new Dictionary<int, int>());
            foreach(var item in featureSegmentMap)
            {
              
                foreach(var featureNumber in featureList)
                {
                    item.Value.Add(featureNumber, 0);
                }
            }
            foreach(var trainingItem in this.trainingSentences)
            {
                foreach(var segment in trainingItem.RelatedSegments)
                {
                    var segmentDic=featureSegmentMap[segment];
                    foreach (var feature in trainingItem.DistinctSemanticNodes)
                        segmentDic[feature]++;
                }
            }
            foreach (var segmentNumber in segmentList)
            {
             
                foreach (var featureNumber in featureList)
                {
                    if (!this.featureScoreValues.ContainsKey(featureNumber))
                        this.featureScoreValues.Add(featureNumber, new SVMFeature(featureNumber));
                    var svmFeature = this.featureScoreValues[featureNumber];
                    var posizitifInstanceNumber = featureSegmentMap[segmentNumber][featureNumber];
                    var negatifInstanceNumber = featureSegmentMap.Where(p => p.Key != segmentNumber).Sum(p => p.Value[featureNumber]);
                    var featureInSegmentCount =featureSegmentMap.Count(p=>featureSegmentMap[p.Key][featureNumber]>0);       // this.trainingSentences
                   //.Where(sentence => sentence.ContainsFeature(featureNumber))
                   //.Select(sentence => sentence.RelatedSegments)
                   //.Distinct().Count();
                    FScore fscore=new FScore(posizitifInstanceNumber,negatifInstanceNumber,sentenceCount,featureInSegmentCount,segmentList.Count);
                    svmFeature.AddFScore(segmentNumber, fscore);
                }

            }


        }

        public IEnumerator<TrainingItem> GetEnumerator()
        {
            return trainingSentences.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return trainingSentences.GetEnumerator();
        }
    }
}
