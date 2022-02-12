using ML.WordNetLibrary.Dictionary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    public  static class WordNet
    {

        #region Private Static Members Of WordNet


        private static Dictionary<string, WordNode> _allWordNodes = new Dictionary<string, WordNode>();
        private static Dictionary<string, WordDictionaryType> _wordDictionaries = new Dictionary<string, WordDictionaryType>();
        private static Dictionary<int, SemanticNode> _semanticNodesNumberDictionary = new Dictionary<int, SemanticNode>();
        private static Dictionary<string, SemanticNode> _semanticNodeNamesDictionary = new Dictionary<string, SemanticNode>();

        private static Guid _version = Guid.NewGuid();
        private static int _semanticCounter = 1;
      
        private static WordNetMode _wordNetMode=WordNetMode.Extension;
        
        private static void onSemanticConnected(WordNode wordNode,SemanticNode semanticNode)
        {
            if (SemanticConnected == null) return;
            var eventArgs = new SemanticConnectedEventArgs(wordNode, semanticNode);
            SemanticConnected(eventArgs);
        }
        private static void onWordNetExtended(WordNode newNode)
        {
            if (WordNetExtended == null) return;
            var eventArgs = new WordNetExtendedEventArgs(newNode);
            WordNetExtended(eventArgs);
        }
        private static WordNode createNewWordNode(string word)
        {
           
            EnsureInExtensionMode();
            var semanticNode = word.ToSemanticNode();
            var newWordNode = new WordNode(word, semanticNode);
            _allWordNodes.Add(newWordNode.Word, newWordNode);
            onWordNetExtended(newWordNode);
            semanticNode.AddNode(newWordNode);
            return newWordNode;
        }
        #endregion

        #region Internal Operations Of WordNet
        internal static void EnsureInExtensionMode()
        {
            if (WordNet.Mode != WordNetMode.Extension)
                throw new WordNetInvalidOperationException("You are not allowed to operate this method when WordNet is in Static mode");
        }
        internal static SemanticNode ToSemanticNode(this string semanticNodeName)
        {
            EnsureInExtensionMode();
            if (string.IsNullOrEmpty(semanticNodeName))
                throw new WordNetInvalidOperationException("Semantic Node Name can not be empty");
            semanticNodeName = semanticNodeName.Trim().ToTurkishLower();
            if (_semanticNodeNamesDictionary.ContainsKey(semanticNodeName))
                return _semanticNodeNamesDictionary[semanticNodeName];
            var node = new SemanticNode(_semanticCounter++,semanticNodeName);
            _semanticNodesNumberDictionary.Add(node.SemanticNumber, node);
           _semanticNodeNamesDictionary.Add(semanticNodeName,node);
            return node;
        }
      
        internal static void ConnectToMainSemanticNode(this WordNode wnode, SemanticNode snode)
        {
            
            if (wnode.MainSemanticNode == snode) return;
            var tempMainSemanticNode = wnode.MainSemanticNode;
            tempMainSemanticNode.DropNode(wnode);
            wnode.MainSemanticNode = snode;
            snode.AddNode(wnode);
            foreach(var connectedNode in tempMainSemanticNode.ConnectedNodes.ToArray())
            {
                if(connectedNode.MainSemanticNode==tempMainSemanticNode)
                {
                    connectedNode.MainSemanticNode = snode;
                    snode.AddNode(connectedNode);
                    tempMainSemanticNode.DropNode(connectedNode);
                }

            }
            if (tempMainSemanticNode.ConnectedNodesCount == 0)
            {
                _semanticNodeNamesDictionary.Remove(tempMainSemanticNode.NodeName);
                _semanticNodesNumberDictionary.Remove(tempMainSemanticNode.SemanticNumber);
            }
            
           
           
        }


      
        #endregion

        #region Public Static Members
        public static WordNetMode Mode
        {
            get { return _wordNetMode; }
          
        }
        public static Guid Version
        {
            get { return _version; }
        }
        public static event SemanticConnectedEventHandler SemanticConnected;
        public static event WordNetExtendedEventHandler WordNetExtended;
        public static SemanticNode GetSemanticNode(int semanticNodeNumber)
        {
            return _semanticNodesNumberDictionary[semanticNodeNumber];
        }
        
        public static WordNode ToWordNode(this string word)
        {
           
            if (string.IsNullOrEmpty(word)) return WordNode.Empty;
            word = word.Trim().ToTurkishLower().ToASCIISolvedWord();
            if (string.IsNullOrEmpty(word)) return WordNode.Empty;
            if (_allWordNodes.ContainsKey(word))
                 return _allWordNodes[word];
            word = word.ToRootWord();
            if (_allWordNodes.ContainsKey(word))
                return _allWordNodes[word];
            if (word.IsMorphologicalValidWord())
              return  createNewWordNode(word);

            var foundWordFromPatterns = word.FindWordFromWordPatterns();
            if (string.IsNullOrEmpty(foundWordFromPatterns))
                return createNewWordNode(word);
            var correspondingNode=_allWordNodes.ContainsKey(foundWordFromPatterns)?_allWordNodes[foundWordFromPatterns]:createNewWordNode(foundWordFromPatterns);
            _allWordNodes.Add(word, correspondingNode);
            return correspondingNode;
        }
        public static IEnumerable<WordNode> ToWordNodes(this  IEnumerable<string> words)
        {
            return words.Where(w => !string.IsNullOrEmpty(w)).Select(word => word.ToWordNode());

        }
        public static void IncludeToWordDictionary(this string word,WordDictionaryType dicTypes)
        {
            EnsureInExtensionMode();

            word = word.ToTurkishLower().ToASCIISolvedWord().ToRootWord();
            if(!_wordDictionaries.ContainsKey(word))
             {
                        _wordDictionaries.Add(word, dicTypes);
                        if (!_allWordNodes.ContainsKey(word))
                            createNewWordNode(word);
                        return;
             }
              _wordDictionaries[word] |= dicTypes;
            
        }
        public static bool IsInWordsDictionary(this string word)
        {
            return _wordDictionaries.ContainsKey(word.ToWordNode().Word);
        }
        public static bool IsInWordsDictionary(this string word,WordDictionaryType dicType)
        {
          
            if (!_wordDictionaries.ContainsKey(word.ToWordNode().Word)) return false;
           
            return (_wordDictionaries[word] & dicType) != WordDictionaryType.None;
        }
        public static IEnumerable<string> GetWordDictionary(WordDictionaryType dicType)
        {
            return _wordDictionaries.Where(item => (item.Value & dicType) != WordDictionaryType.None).Select(p => p.Key);
        }
        

        public static IEnumerable<SemanticNode> SemanticNodes
        {
            get
            {
                return _semanticNodesNumberDictionary.Select(p => p.Value);
            }
        }

       
        public static WordNode ConnectToSemanticNode(this WordNode wordNode, SemanticNode semanticNode)
        {
            if (semanticNode == null) return wordNode;
            EnsureInExtensionMode();
            if(wordNode.AddSemanticNode(semanticNode))
            {
                semanticNode.AddNode(wordNode);
                onSemanticConnected(wordNode, semanticNode);

            }
            return wordNode;

        }
        public static void ConnectToSemanticNodes(this string word,string semanticWord)
        {
            if (word == null || semanticWord == null) return ;
            var wordNodeArray = word.SplitWithSpace().ToWordNodes().ToArray();
            if (wordNodeArray.Length > 1)
                word.AddExpression(semanticWord);
            else
                wordNodeArray.First().ConnectToSemanticNode(semanticWord.ToSemanticNode());
           
         
        }

      
       
        public static string ToSentenceString(this IEnumerable<WordNode> wordNodeList)
        {

            return wordNodeList.Select(w => w.Word).Aggregate((current, next) => current + " " + next);

        }

        public static void ConnectAsSynonym(this string word, string synonymWords)
        {
           EnsureInExtensionMode();
          
           var wordNode = word.ToWordNode();
         
           foreach(var subWord in synonymWords.SplitWithComma())
           {
               subWord.ToWordNode().ConnectToMainSemanticNode(wordNode.MainSemanticNode);

           }


        }
      

        public static WordNetSpace ExportSpace(string description)
        {
            WordNetSpace space = new WordNetSpace(_version,description);
            space.WordsDictionary = _wordDictionaries;
            space.SemanticCounter = _semanticCounter;
            space.SemanticNodes = new List<SemanticNode>(_semanticNodesNumberDictionary.Select(s => s.Value));
            space.WordNodes = new List<WordNode>(_allWordNodes.Select(w => w.Value));
         
            return space;
        }

        public static void ImportSpace(WordNetSpace space)
        {
            EnsureInExtensionMode();
            _wordNetMode = WordNetMode.Backup;
            _wordDictionaries.Clear();
            _semanticNodesNumberDictionary.Clear();
            _allWordNodes.Clear();
            _semanticCounter = space.SemanticCounter;
            space.SemanticNodes.ForEach(semanticNode =>
            {
                _semanticNodesNumberDictionary.Add(semanticNode.SemanticNumber, semanticNode);
                _semanticNodeNamesDictionary.Add(semanticNode.NodeName, semanticNode);

            
            });
            space.WordNodes.ForEach(wordnode => _allWordNodes.Add(wordnode.Word, wordnode));
            foreach (var dicItem in space.WordsDictionary)
                _wordDictionaries.Add(dicItem.Key, dicItem.Value);
            _version = space.Version;
            _wordNetMode = WordNetMode.Extension;

        }

      
        #endregion




    }
}
