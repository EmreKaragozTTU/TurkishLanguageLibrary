using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.SentimentAnalysis
{
    public static class TrainingItemExtensions
    {

        public static List<TrainingItem>  ExtractTrainingTestPair(this IEnumerable<TrainingItem> allTraininingItems,out List<TrainingItem> testData)
        {
            List<TrainingItem> trainData=new List<TrainingItem>();
            testData=new List<TrainingItem>();
            Random rnd = new Random();
            foreach (var item in allTraininingItems)
            {
                if (rnd.Next(5) == 3)
                    testData.Add(item);
                else
                    trainData.Add(item);

            }
            return trainData;


        }
    }
}
