﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Annytab.Stemmer;
using Fastenshtein;
using TestIt.Utils.Extend;

namespace TestIt.CorrectionAlgorithms
{
    public static class Core
    {
        public static IEnumerable<string> SeparateIntoSentences(string text)
        {
           var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
            
           return sentences;
        }
        
        public static int TextDif(string text1, string text2)
        {
            return Levenshtein.Distance(text1, text2);
        }

        private static double TextDifPercent(string text1, string text2)
        {
            var maxLength = text1.Length;
            var result = maxLength - Levenshtein.Distance(text1, text2);

            return (double)result / maxLength;
        }

        public static double BestTextPercent(string text1, List<string> texts)
        {
            var bestPercent = 0.0;

            texts.ForEach(x =>
            {
                var percent = TextDifPercent(text1, x);
                if (percent > bestPercent)
                    bestPercent = percent;
            });

            return bestPercent;
        }

        public static string NormalizeText(string text)
        {
            var stemmer = new PortugueseStemmer();

            var steamWord = stemmer.GetSteamWord(text);

            return steamWord;
        }

        public static int KeyWordMatcher(string baseText, IEnumerable<string> keywords)
        {
            var lowerKeywords = (from s in keywords select s.ToLower()).ToList();
            return baseText.ToLower().Count(lowerKeywords);
        }
        
        public static IEnumerable<string> NormalizeWords(string[] words)
        {
            var stemmer = new PortugueseStemmer();

            var steamWords = stemmer.GetSteamWords(words);

            return steamWords;
        }
    }
}
