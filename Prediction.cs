using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.SentimentAnalysis
{
    public class Prediction
    {
        public int SegmentNumber { get; set; }
        public string SegmentName { get; set; }
        public double Probability { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - Probability:{1}", this.SegmentName, this.Probability);
        }

    }
}
