using LibSVMsharp;
using LibSVMsharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.SentimentAnalysis
{
   [Serializable]
    public class SVMSetting
    {
        public SVMNormType SVMNormType { get; set; }
        public FeatureScoreType FeatureScoreType { get; set; }
        private SVMType _svmType=SVMType.C_SVC;
        private SVMKernelType _svmKernelType=SVMKernelType.LINEAR;
        private double _svmC=1;
        private double _svmGamma=1;

       [NonSerialized]
       private SVMParameter _svmParameter;
       public SVMParameter SVMParameter
        {
            get
            {
                if (_svmParameter != null) return _svmParameter;
                _svmParameter = new SVMParameter();
                _svmParameter = new SVMParameter();
                _svmParameter.Type = this._svmType;
                _svmParameter.Kernel = this._svmKernelType;
                _svmParameter.C = this._svmC;
                _svmParameter.Gamma = this._svmGamma;
                return _svmParameter;
            }
            set
            {
                if (_svmParameter == null)
                    _svmParameter = new SVMParameter();
                _svmParameter.Type = value.Type;
                _svmParameter.Kernel = value.Kernel;
                _svmParameter.C = value.C;
                _svmParameter.Gamma = value.Gamma;

            }
        }
          
        public SVMSetting()
            :base()
        {
            this.SVMNormType = LibSVMsharp.Helpers.SVMNormType.L2;

            this.FeatureScoreType = FeatureScoreType.Static;
        }
    }
}
