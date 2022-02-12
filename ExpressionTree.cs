using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    internal static class ExpressionTree
    {
        private static Dictionary<string, Dictionary<string,Expression>> _expressions = new Dictionary<string,Dictionary<string, Expression>>();
        
       
        internal static void AddExpression(this string sentence,string meaning)
        {
            if (sentence == null || string.IsNullOrEmpty(sentence.Trim())) return;
            var wordArray = sentence.SplitWithSpace().ToWordNodes().ToArray();
            var cleanSentence=wordArray.ToSentenceString();
            var firstNode = wordArray.First();
            if (!_expressions.ContainsKey(firstNode.Word))
                _expressions.Add(firstNode.Word, new Dictionary<string, Expression>());
            var theExpressionList = _expressions[firstNode.Word];
            if (theExpressionList.ContainsKey(cleanSentence))
            {
                theExpressionList[cleanSentence].AddSemanticMeaning(meaning.ToSemanticNode().NodeName);
                return;
            }
            var theExpression = new Expression(wordArray, meaning.ToSemanticNode().NodeName);
            theExpressionList.Add(cleanSentence, theExpression);

        }

        internal static IEnumerable<string> ReplaceWithDefinedExpressions(this IEnumerable<string> wordList)
        {
            var wordArray = wordList.ToWordNodes().ToArray();
            List<string> newStringList=new List<string>();
            for(int i=0;i<wordArray.Length;i++)
            {
                if(!_expressions.ContainsKey(wordArray[i].Word))
                {
                    newStringList.Add(wordArray[i].Word);
                    continue;
                }
                var isFound = false;
                foreach(var exp in _expressions[wordArray[i].Word])
                {
                    var foundIndex=exp.Value.IndexOf(wordArray, i);
                    if (foundIndex < 0)
                        continue;
                    foreach (var semanticMeaning in exp.Value.SemanticMeanings)
                        newStringList.Add(semanticMeaning);
                    i = foundIndex-1;
                    isFound = true;
                    break;
                    
                }
                if(!isFound)
                    newStringList.Add(wordArray[i].Word);

            }
            return newStringList;
        }



    }
}
