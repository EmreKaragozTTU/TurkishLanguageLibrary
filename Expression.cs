using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    internal class Expression
    {
        internal HashSet<string> SemanticMeanings { get; private set; }
        private LinkedWord _rootLinkedWord;
        internal void AddSemanticMeaning(string meaning)
        {
            if(this.SemanticMeanings.Contains(meaning))
                return;
            this.SemanticMeanings.Add(meaning);
        }
        internal Expression(IEnumerable<WordNode> wordList,string meaning)
        {
            this.SemanticMeanings = new HashSet<string>();
            this.SemanticMeanings.Add(meaning);
            
            LinkedWord currentLinkedWord = null;
            foreach(var subWord in wordList)
            {
                if(currentLinkedWord==null)
                {
                    _rootLinkedWord = new LinkedWord(subWord.Word);
                    currentLinkedWord = _rootLinkedWord;
                }
                else
                {
                    currentLinkedWord.NextLinkedWord = new LinkedWord(subWord.Word);
                    currentLinkedWord = currentLinkedWord.NextLinkedWord;
                }

            }


        }
        
        internal  int IndexOf(WordNode[] wordList,int startIndex)
        {
            var currentNode = _rootLinkedWord;
            while(startIndex<wordList.Length)
            {

                if (currentNode.Word != wordList[startIndex].Word)
                    return -1;
                currentNode = currentNode.NextLinkedWord;
                startIndex++;
                if (currentNode == null) return startIndex;

            }
            return -1;

        }

    }
}
