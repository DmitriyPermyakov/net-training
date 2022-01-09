﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EnumerableTask {
    public class StringComparer: IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (s1.Length == s2.Length)
                return s1.CompareTo(s2);
            else if (s1.Length > s2.Length)
                return 1;
            else
                return -1;
        }
    }
    public class Task {

        /// <summary> Transforms all strings to uppercase</summary>
        /// <param name="data">source string sequence</param>
        /// <returns>
        ///   Returns sequence of source strings in uppercase
        /// </returns>
        /// <example>
        ///    {"a","b","c"} => { "A", "B", "C" }
        ///    { "A", "B", "C" } => { "A", "B", "C" }
        ///    { "a", "A", "", null } => { "A", "A", "", null }
        /// </example>
        public IEnumerable<string> GetUppercaseStrings(IEnumerable<string> data) {
            List<string> modifiedList = new List<string>();
            foreach (string s in data)
            {
                if(s == string.Empty)
                {
                    modifiedList.Add(string.Empty);
                } else if(s == null)
                {
                    modifiedList.Add(null);
                } else
                {
                    modifiedList.Add(s.ToUpper());
                }
            }
            return (IEnumerable<string>)modifiedList;
        }

        /// <summary> Transforms an each string from sequence to its length</summary>
        /// <param name="data">source strings sequence</param>
        /// <returns>
        ///   Returns sequence of strings length
        /// </returns>
        /// <example>
        ///   { } => { }
        ///   {"a","aa","aaa" } => { 1, 2, 3 }
        ///   {"aa","bb","cc", "", "  ", null } => { 2, 2, 2, 0, 2, 0 }
        /// </example>
        public IEnumerable<int> GetStringsLength(IEnumerable<string> data) {
            List<int> result = new List<int>();

            foreach(string s in data)
            {
                if(s == string.Empty || s == null)
                {
                    result.Add(0);
                } 
                else
                {
                    result.Add(s.Length);
                }
            }

            return (IEnumerable<int>)result;
        }

        /// <summary>Transforms int sequence to its square sequence, f(x) = x * x </summary>
        /// <param name="data">source int sequence</param>
        /// <returns>
        ///   Returns sequence of squared items
        /// </returns>
        /// <example>
        ///   { } => { }
        ///   { 1, 2, 3, 4, 5 } => { 1, 4, 9, 16, 25 }
        ///   { -1, -2, -3, -4, -5 } => { 1, 4, 9, 16, 25 }
        /// </example>
        public IEnumerable<long> GetSquareSequence(IEnumerable<int> data) {
            List<long> result = new List<long>();

            if(data != null)
            {
                foreach(int n in data)
                {
                    result.Add((long)n * (long)n);
                }
            }
            return (IEnumerable<long>)result;
        }

        /// <summary>Transforms int sequence to its moving sum sequence, 
        ///          f[n] = x[0] + x[1] + x[2] +...+ x[n] 
        ///       or f[n] = f[n-1] + x[n]   
        /// </summary>
        /// <param name="data">source int sequence</param>
        /// <returns>
        ///   Returns sequence of sum of all source items with less or equals index
        /// </returns>
        /// <example>
        ///   { } => { }
        ///   { 1, 1, 1, 1 } => { 1, 2, 3, 4 }
        ///   { 5, 5, 5, 5 } => { 5, 10, 15, 20 }
        ///   { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } => { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 }
        ///   { 1, -1, 1, -1, -1 } => { 1, 0, 1, 0, 1 }
        /// </example>
        public IEnumerable<long> GetMovingSumSequence(IEnumerable<int> data) {
            List<long> result = new List<long>();

            if(data != null)
            {
                for(int elementIndex = 0; elementIndex < data.Count(); elementIndex++)
                {
                    long sum = 0;
                    int subElementIndex = -1;
                    foreach(int elementValue in data)
                    {
                        sum += (long)elementValue;
                        subElementIndex++;
                        if (subElementIndex >= elementIndex)
                            break;
                    }                    
                    result.Add(sum);
                }
            }
            return (IEnumerable<long>)result;
        }


        /// <summary> Filters a string sequence by a prefix value (case insensitive)</summary>
        /// <param name="data">source string sequence</param>
        /// <param name="prefix">prefix value to filter</param>
        /// <returns>
        ///  Returns items from data that started with required prefix (case insensitive)
        /// </returns>
        /// <exception cref="System.ArgumentNullException">prefix is null</exception>
        /// <example>
        ///  { "aaa", "bbbb", "ccc", null }, prefix="b"  =>  { "bbbb" }
        ///  { "aaa", "bbbb", "ccc", null }, prefix="B"  =>  { "bbbb" }
        ///  { "a","b","c" }, prefix="D"  => { }
        ///  { "a","b","c" }, prefix=""   => { "a","b","c" }
        ///  { "a","b","c", null }, prefix=""   => { "a","b","c" }
        ///  { "a","b","c" }, prefix=null => exception
        /// </example>
        public IEnumerable<string> GetPrefixItems(IEnumerable<string> data, string prefix) {
            if(prefix == null)
            {
                throw new ArgumentNullException();
            }

            if(data == null)
            {
                throw new ArgumentNullException();
            }


            List<string> result = new List<string>();
            if (prefix == string.Empty)
            {                
                foreach (string substring in data)
                {                    
                    if(substring != null)
                    {                        
                        result.Add(substring);
                    }
                }
                return (IEnumerable<string>)result;
            } 
            else 
            {                
                foreach (string substring in data)
                {                    
                    if (substring == null)
                        continue;
                    if (substring.Trim().StartsWith(prefix, true, CultureInfo.CurrentCulture))
                    {                        
                        result.Add(substring);
                    }
                }                
            }

            return (IEnumerable<string>)result;
        }

        /// <summary> Returns every second item from source sequence</summary>
        /// <typeparam name="T">the type of the elements of data</typeparam>
        /// <param name="data">source sequence</param>
        /// <returns>Returns a subsequence that consists of every second item from source sequence</returns>
        /// <example>
        ///  { 1,2,3,4,5,6,7,8,9,10 } => { 2,4,6,8,10 }
        ///  { "a","b","c" , null } => { "b", null }
        ///  { "a" } => { }
        /// </example>
        public IEnumerable<T> GetEvenItems<T>(IEnumerable<T> data) {
            if (data == null)
                throw new ArgumentNullException();

            int index = 1;
            List<T> result = new List<T>();
            foreach(var item in data)
            {
                if(index % 2 == 0)
                {
                    result.Add(item);
                }
                index++;
            }

            return (IEnumerable<T>)result;
        }

        /// <summary> Propagate every item in sequence its position times</summary>
        /// <typeparam name="T">the type of the elements of data</typeparam>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns a sequence that consists of: one first item, two second items, tree third items etc. 
        /// </returns>
        /// <example>
        ///   { } => { }
        ///   { 1 } => { 1 }
        ///   { "a","b" } => { "a", "b","b" }
        ///   { "a", "b", "c", null } => { "a", "b","b", "c","c","c", null,null,null,null }
        ///   { 1,2,3,4,5} => { 1, 2,2, 3,3,3, 4,4,4,4, 5,5,5,5,5 }
        /// </example>
        public IEnumerable<T> PropagateItemsByPositionIndex<T>(IEnumerable<T> data) {
            List<T> result = new List<T>();
            int elementIndex = 1;

            foreach(var item in data)
            {
                for(int cycleCount = 1; cycleCount <= elementIndex; cycleCount++)
                {
                    result.Add(item);
                }
                elementIndex++;
            }

            return (IEnumerable<T>)result;
        }

        /// <summary>Finds all used char in string sequence</summary>
        /// <param name="data">source string sequence</param>
        /// <returns>
        ///   Returns set of chars used in string sequence.
        ///   Order of result does not matter.
        /// </returns>
        /// <example>
        ///   { "aaa", "bbb", "cccc", "abc" } => { 'a', 'b', 'c' } or { 'c', 'b', 'a' }
        ///   { " ", null, "   ", "" } => { ' ' }
        ///   { "", null } => { }
        ///   { } => { }
        /// </example>
        public IEnumerable<char> GetUsedChars(IEnumerable<string> data) {
            List<char> result = new List<char>();
            if(data != null)
            {
                foreach(string item in data)
                {
                    if(!string.IsNullOrEmpty(item))
                    {
                        foreach(var c in item)
                        {
                            if(!result.Contains(c))
                            {
                                result.Add(c);
                            }
                        }
                    }
                }
            }
            return (IEnumerable<char>)result;
        }


        /// <summary> Converts a source sequence to a string</summary>
        /// <typeparam name="T">the type of the elements of data</typeparam>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns a string respresentation of source sequence 
        /// </returns>
        /// <example>
        ///   { } => ""
        ///   { 1,2,3 } => "1,2,3"
        ///   { "a", "b", "c", null, ""} => "a,b,c,null,"
        ///   { "", "" } => ","
        /// </example>
        public string GetStringOfSequence<T>(IEnumerable<T> data) {
            if (data == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            int itemCount = data.Count();
            int index = 0;

            foreach(var item in data)
            {
                if(typeof(T).Equals(typeof(string)))
                {
                    if(item == null)
                    {
                        sb.Append("null");
                    } else if(item.ToString() == string.Empty)
                    {
                        sb.Append(string.Empty);
                    } else
                    {
                        sb.Append(item);
                    }
                } 
                else
                {
                    sb.Append(item.ToString());
                }

                if(index != itemCount -1)
                {
                    sb.Append(",");
                }
                index++;                
            }
            
            return sb.ToString();
        }

        /// <summary> Finds the 3 largest numbers from a sequence</summary>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns the 3 largest numbers from a sequence
        /// </returns>
        /// <example>
        ///   { } => { }
        ///   { 1, 2 } => { 2, 1 }
        ///   { 1, 2, 3 } => { 3, 2, 1 }
        ///   { 1,2,3,4,5,6,7,8,9,10 } => { 10, 9, 8 }
        ///   { 10, 10, 10, 10 } => { 10, 10, 10 }
        /// </example>
        public IEnumerable<int> Get3TopItems(IEnumerable<int> data) {
            int[] dataInArray = data.ToArray();
            Array.Sort(dataInArray);
            IEnumerable<int> reversedArray = dataInArray.Reverse();
            List<int> result = new List<int>();
            int index = 1;
            int neededQuantity = 3;
            foreach(int item in reversedArray)
            {
                result.Add(item);                
                if(index >= neededQuantity)
                {
                    break;
                }
                index++;
            }
            return (IEnumerable<int>)result;
        }

        /// <summary> Calculates the count of numbers that are greater then 10</summary>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns the number of items that are > 10
        /// </returns>
        /// <example>
        ///   { } => 0
        ///   { 1, 2, 10 } => 0
        ///   { 1, 2, 3, 11 } => 1
        ///   { 1, 20, 30, 40 } => 3
        /// </example>
        public int GetCountOfGreaterThen10(IEnumerable<int> data) {
            int count = 0;
            foreach(int item in data)
            {
                if (item > 10)
                    count++;
            }
            return count;
        }


        /// <summary> Find the first string that contains "first" (case insensitive search)</summary>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns the first string that contains "first" or null if such an item does not found.
        /// </returns>
        /// <example>
        ///   { "a", "b", null} => null
        ///   { "a", "IT IS FIRST", "first item", "I am really first!" } => "IT IS FIRST"
        ///   { } => null
        /// </example>
        public string GetFirstContainsFirst(IEnumerable<string> data) {
            if (data == null)
                return null;
            string searchingString = "first";
            foreach(string s in data)
            {
                if (s == null)
                    continue;
                if (s.ToLower().Contains(searchingString))
                    return s;
            }
            return null;
        }

        /// <summary> Counts the number of unique strings with length=3 </summary>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns the number of unique strings with length=3.
        /// </returns>
        /// <example>
        ///   { "a", "b", null, "aaa"} => 1    ("aaa")
        ///   { "a", "bbb", null, "", "ccc" } => 2  ("bbb","ccc")
        ///   { "aaa", "aaa", "aaa", "bbb" } => 2   ("aaa", "bbb") 
        ///   { } => 0
        /// </example>
        public int GetCountOfStringsWithLengthEqualsTo3(IEnumerable<string> data) {
            if (data == null)
                return 0;

            HashSet<string> stringWithLengthThree = new HashSet<string>();
            foreach(var s in data)
            {
                if (s == null)
                    continue;
                if (s.Length == 3)
                    stringWithLengthThree.Add(s);
            }
            return stringWithLengthThree.Count;
        }

        /// <summary> Counts the number of each strings in sequence </summary>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns the list of strings and number of occurence for each string.
        /// </returns>
        /// <example>
        ///   { "a", "b", "a", "aa", "b"} => { ("a",2), ("b",2), ("aa",1) }    
        ///   { "a", "a", null, "", "ccc", "" } => { ("a",2), (null,1), ("",2), ("ccc",1) }
        ///   { "aaa", "aaa", "aaa" } =>  { ("aaa", 3) } 
        ///   { } => { }
        /// </example>
        public IEnumerable<Tuple<string,int>> GetCountOfStrings(IEnumerable<string> data) {
            throw new NotImplementedException();
        }

        /// <summary> Counts the number of strings with max length in sequence </summary>
        /// <param name="data">source sequence</param>
        /// <returns>
        ///   Returns the number of strings with max length. (Assuming that null has Length=0).
        /// </returns>
        /// <example>
        ///   { "a", "b", "a", "aa", "b"} => 1 
        ///   { "a", "aaa", null, "", "ccc", "" } => 2   
        ///   { "aaa", "aaa", "aaa" } =>  3
        ///   { "", null, "", null } => 4
        ///   { } => { }
        /// </example>
        public int GetCountOfStringsWithMaxLength(IEnumerable<string> data) {
            if (data.Count() == 0)
                return 0;

            SortedDictionary<int, int> lenghtDictionary = new SortedDictionary<int, int>();
            foreach(var s in data)
            {
                int length;
                if (s == null)
                    length = 0;
                else
                    length = s.Length;

                if(lenghtDictionary.ContainsKey(length))
                {
                   lenghtDictionary[length]++;
                }
                else
                {
                    lenghtDictionary.Add(length, 1);
                }                
            }            
            return lenghtDictionary.Last().Value;
        }



        /// <summary> Counts the digit chars in a string</summary>
        /// <param name="data">source string</param>
        /// <returns>
        ///   Returns number of digit chars in the string
        /// </returns>
        /// <exception cref="System.ArgumentNullException">data is null</exception>
        /// <example>
        ///    "aaaa" => 0
        ///    "1234" => 4
        ///    "A1*B2" => 2
        ///    "" => 0
        ///    null => exception
        /// </example>
        public int GetDigitCharsCount(string data) {
            if (data == null)
                throw new ArgumentNullException();

            if (data.Length == 0)
                return 0;
            int count = 0;
            foreach (char c in data)
                if (char.IsDigit(c))
                    count++;

            return count;
        }


       
        /// <summary>Counts the system log events of required type</summary>
        /// <param name="value">the type of log event (Error, Event, Information etc)</param>
        /// <returns>
        ///   Returns the number of System log entries of specified type
        /// </returns>
        public int GetSpecificEventEntriesCount(EventLogEntryType value) {
            // TODO : Implement GetSpecificEventEntriesCount
            EventLogEntryCollection systemEvents = (new EventLog("System", ".")).Entries;
            
            throw new NotImplementedException();
        }


        /// <summary> Finds all exported types names which implement IEnumerable</summary>
        /// <param name="assembly">the assembly to search</param>
        /// <returns>
        ///   Returns the names list of exported types implemented IEnumerable from specified assembly
        /// </returns>
        /// <exception cref="System.ArgumentNullException">assembly is null</exception>
        /// <example>
        ///    mscorlib => { "ApplicationTrustCollection","Array","ArrayList","AuthorizationRuleCollection",
        ///                  "BaseChannelObjectWithProperties", ... }
        ///    
        /// </example>
        public IEnumerable<string> GetIEnumerableTypesNames(Assembly assembly) {
            if(assembly == null)
            {
                throw new ArgumentNullException();
            }
            Type[] types = assembly.GetExportedTypes();
            
            List<string> typeList = new List<string>();
            foreach(Type t in types)
            {
                if(typeof(IEnumerable).IsAssignableFrom(t))
                {   
                    typeList.Add(t.Name);
                }
            }

            typeList.Sort();
            
            return (IEnumerable<string>)typeList;
        }

        /// <summary>Calculates sales sum by quarter</summary>
        /// <param name="sales">the source sales data : Item1 is sales date, Item2 is amount</param>
        /// <returns>
        ///     Returns array of sales sum by quarters, 
        ///     result[0] is sales sum for Q1, result[1] is sales sum for Q2 etc  
        /// </returns>
        /// <example>
        ///    {} => { 0, 0, 0, 0}
        ///    {(1/1/2010, 10)  , (2/2/2010, 10), (3/3/2010, 10) } => { 30, 0, 0, 0 }
        ///    {(1/1/2010, 10)  , (4/4/2010, 10), (10/10/2010, 10) } => { 10, 10, 0, 10 }
        /// </example>
        public int[] GetQuarterSales(IEnumerable<Tuple<DateTime, int>> sales) {
            int[] salesSumForQuarter = new int[4];

            foreach(var salesData in sales)
            {
                int date = salesData.Item1.Month;
                if (date >= 1 && date <= 3)
                {
                    salesSumForQuarter[0] += salesData.Item2;
                } else if (date >= 4 && date <= 6 )
                {
                    salesSumForQuarter[1] += salesData.Item2;
                } else if(date >= 7 && date <= 9)
                {
                    salesSumForQuarter[2] += salesData.Item2;
                } else if( date >= 10 && date <= 12 )
                {
                    salesSumForQuarter[3] += salesData.Item2;
                }
            }
            return salesSumForQuarter;
            
        }


         /// <summary> Sorts string by length and alphabet </summary>
        /// <param name="data">the source data</param>
        /// <returns>
        /// Returns sequence of strings sorted by length and alphabet
        /// </returns>
        /// <example>
        ///  {} => {}
        ///  {"c","b","a"} => {"a","b","c"}
        ///  {"c","cc","b","bb","a,"aa"} => {"a","b","c","aa","bb","cc"}
        /// </example>
        public IEnumerable<string> SortStringsByLengthAndAlphabet(IEnumerable<string> data) {
            if (data == null)
                return new string[] { };

            string[] stringToSort = data.ToArray();
            Array.Sort(stringToSort, new StringComparer());
            return stringToSort;
        }

        /// <summary> Finds all missing digits </summary>
        /// <param name="data">the source data</param>
        /// <returns>
        /// Return all digits that are not in the string sequence
        /// </returns>
        /// <example>
        ///   {} => {'0','1','2','3','4','5','6','7','8','9'}
        ///   {"aaa","a1","b","c2","d","e3","f01234"} => {'5','6','7','8','9'}
        ///   {"a","b","c","9876543210"} => {}
        /// </example>
        public IEnumerable<char> GetMissingDigits(IEnumerable<string> data) {            
            string template = "0123456789";            

            if (data.Count() == 0)
                return (IEnumerable<char>)template;

            foreach (char t in template)
            {
                foreach (string s in data)
                {
                    if (s.Contains(t))
                        template = Regex.Replace(template, t.ToString(), string.Empty);
                }
            }

            Console.WriteLine(template);
            return (IEnumerable<char>)template;
        }


        /// <summary> Sorts digit names </summary>
        /// <param name="data">the source data</param>
        /// <returns>
        /// Return all digit names sorted by numeric order
        /// </returns>
        /// <example>
        ///   {} => {}
        ///   {"nine","one"} => {"one","nine"}
        ///   {"one","two","three"} => {"one","two","three"}
        ///   {"nine","eight","nine","eight"} => {"eight","eight","nine","nine"}
        ///   {"one","one","one","zero"} => {"zero","one","one","one"}
        /// </example>
        public IEnumerable<string> SortDigitNamesByNumericOrder(IEnumerable<string> data) {
            if (data == null)
                return new string[] { };

            Dictionary<string, int> template = new Dictionary<string, int>()
            {
                { "zero", 0 },
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 },
            };

            List<KeyValuePair<string, int>> dataDictionary = new List<KeyValuePair<string, int>>();
            foreach(string s in data)
            {
                dataDictionary.Add(new KeyValuePair<string, int>(s, template[s]));
            }

            var result = dataDictionary.OrderBy(d => d.Value).Select(v => v.Key);

            return result;
        }

        /// <summary> Combines numbers and fruits </summary>
        /// <param name="numbers">string sequience of numbers</param>
        /// <param name="fruits">string sequence of fruits</param>
        /// <returns>
        ///   Returns new sequence of merged number and fruit items
        /// </returns>
        /// <example>
        ///  {"one","two","three"}, {"apple", "bananas","pineapples"} => {"one apple","two bananas","three pineapples"}
        ///  {"one"}, {"apple", "bananas","pineapples"} => {"one apple"}
        ///  {"one","two","three"}, {"apple", "bananas" } => {"one apple","two bananas"}
        ///  {"one","two","three"}, { } => { }
        ///  { }, {"apple", "bananas" } => { }
        /// </example>
        public IEnumerable<string> CombineNumbersAndFruits(IEnumerable<string> numbers, IEnumerable<string> fruits) {
            if (numbers == null || fruits == null)
                return new string[] { };

            string[] numbersArray = numbers.ToArray();
            string[] fruitsArray = fruits.ToArray();

            int cycles = numbers.Count() > fruits.Count() ? fruits.Count() : numbers.Count(); 
            
            string[] result = new string[cycles];
            for(int i = 0; i < cycles; i++)
            {
                result[i] = numbersArray[i] + " " + fruitsArray[i];
                Console.WriteLine(result[i]);
            }

            return result;
        }


        /// <summary> Finds all chars that are common for all words </summary>
        /// <param name="data">sequence of words</param>
        /// <returns>
        ///  Returns set of chars that are occured in all words (sorted in alphabetical order)
        /// </returns>
        /// <example>
        ///   {"ab","ac","ad"} => {"a"}
        ///   {"a","b","c"} => { }
        ///   {"a","aa","aaa"} => {"a"}
        ///   {"ab","ba","aabb","baba"} => {"a","b"}
        /// </example>
        public IEnumerable<char> GetCommonChars(IEnumerable<string> data) {

            if (data == null || data.Contains(string.Empty))
                return new char[] { };

            string[] example = data.ToArray();
            List<char> common = new List<char>();
            for (int i = 0; i < example.Length - 1; i++)
            {
                common = example[0].Intersect(example[i + 1]).ToList();
                
            }           
                
            return common;
        }

        /// <summary> Calculates sum of all integers from object array </summary>
        /// <param name="data">source data</param>
        /// <returns>
        ///    Returns the sum of all inetegers from object array
        /// </returns>
        /// <example>
        ///    { 1, true, "a","b", false, 1 } => 2
        ///    { true, false } => 0
        ///    { 10, "ten", 10 } => 20 
        ///    { } => 0
        /// </example>
        public int GetSumOfAllInts(object[] data) {
            if (data == null)
                return 0;
            var integers = data.Where(x => x.GetType().Equals(typeof(int))).Select(n => (int)n).ToList();
            if (integers.Count() == 0)
                return 0;
            var sum = integers.Aggregate((x, y) => x + y);
            return sum;
        }


        /// <summary> Finds all strings in array of objects</summary>
        /// <param name="data">source array</param>
        /// <returns>
        ///   Return subsequence of string from source array of objects
        /// </returns>
        /// <example>
        ///   { "a", 1, 2, null, "b", true, 4.5, "c" } => { "a", "b", "c" }
        ///   { "a", "b", "c" } => { "a", "b", "c" }
        ///   { 1,2,3, true, false } => { }
        ///   { } => { }
        /// </example>
        public IEnumerable<string> GetStringsOnly(object[] data) {
            if (data == null)
                return new string[] { };
            var strings = data.Where(s => s != null && s.GetType().Equals(typeof(string)))
                .Select(s => (string)s).ToList();

            return strings;
        }

        /// <summary> Calculates the total length of strings</summary>
        /// <param name="data">source string sequence</param>
        /// <returns>
        ///   Return sum of length of all strings
        /// </returns>
        /// <example>
        ///   {"a","b","c","d","e","f"} => 6
        ///   { "a","aa","aaa" } => 6
        ///   { "1234567890" } => 10
        ///   { null, "", "a" } => 1
        ///   { null } => 0
        ///   { } => 0
        /// </example>
        public int GetTotalStringsLength(IEnumerable<string> data) {
            if (data == null)
                return 0;
            var result = data.Where(s => s != null).Sum(n => n.Length);
            return result;
        }

        /// <summary> Determines whether sequence has null elements</summary>
        /// <param name="data">source string sequence</param>
        /// <returns>
        ///  true if the source sequence contains null elements; otherwise, false.
        /// </returns>
        /// <example>
        ///   { "a", "b", "c", "d", "e", "f" } => false
        ///   { "a", "aa", "aaa", null } => true
        ///   { "" } => false
        ///   { null, null, null } => true
        ///   { } => false
        /// </example>
        public bool IsSequenceHasNulls(IEnumerable<string> data) {
            return data.Contains(null);
        }

        /// <summary> Determines whether all strings in sequence are uppercase</summary>
        /// <param name="data">source string sequence</param>
        /// <returns>
        ///  true if all strings from source sequence are uppercase; otherwise, false.
        /// </returns>
        /// <example>
        ///   { "A", "B", "C", "D", "E", "F" } => true
        ///   { "AA", "AA", "AAA", "AAAa" } => false
        ///   { "" } => false
        ///   { } => false
        /// </example>
        public bool IsAllStringsAreUppercase(IEnumerable<string> data) {
            if (data.Contains(string.Empty) || data.Count() == 0)
                return false;

            var result = data.SelectMany(s => s).All(c => Char.IsUpper(c));
            Console.WriteLine(result);
            return result;
        }

        /// <summary> Finds first subsequence of negative integers </summary>
        /// <param name="data">source integer sequence</param>
        /// <returns>
        ///   Returns the first subsequence of negative integers from source
        /// </returns>
        /// <example>
        ///    { -2, -1 , 0, 1, 2 } => { -2, -1 }
        ///    { 2, 1, 0, -1, -2 } => { -1, -2 }
        ///    { 1, 1, 1, -1, -1, -1, 0, 0, 0, -2, -2, -2 } => { -1, -1, -1 }
        ///    { -1 , 0, -2 } => { -1 }
        ///    { 1, 2, 3 } => { }
        ///    { } => { }
        /// </example>
        public IEnumerable<int> GetFirstNegativeSubsequence(IEnumerable<int> data) {
            
            if (data.Count() == 0)
                return new int[] { };
           
            if(data.First() > 0)
            {
                var result = data.SkipWhile(n => n >= 0).TakeWhile(t => t < 0).ToArray();                
                return result;
            }
            else
            {                
                var result = data.TakeWhile(n => n < 0 || n != 0).ToArray();                
                return result;
            }
        }


        /// <summary> 
        /// Compares two numeric sequences
        /// </summary>
        /// <param name="integers">sequence of integers</param>
        /// <param name="doubles">sequence of doubles</param>
        /// <returns>
        /// true if integers are equals doubles; otherwise, false.
        /// </returns>
        /// <example>
        /// { 1,2,3 } , { 1.0, 2.0, 3.0 } => true
        /// { 0,0,0 } , { 1.0, 2.0, 3.0 } => false
        /// { 3,2,1 } , { 1.0, 2.0, 3.0 } => false
        /// { -10 } => { -10.0 } => true
        /// </example>
        public bool AreNumericListsEqual(IEnumerable<int> integers, IEnumerable<double> doubles) {
            var result = integers.Zip(doubles, (i, d) => new { IntegerValue = i, DoubleValue = d })
                                 .All(value => value.DoubleValue.Equals((double)value.IntegerValue));
                    
            return result;
        }

        /// <summary>
        /// Finds the next after specified version from the list 
        /// </summary>
        /// <param name="versions">source list of versions</param>
        /// <param name="currentVersion">specified version</param>
        /// <returns>
        ///   Returns the next after specified version from the list; otherwise, null.
        /// </returns>
        /// <example>
        ///    { "1.1", "1.2", "1.5", "2.0" }, "1.1" => "1.2"
        ///    { "1.1", "1.2", "1.5", "2.0" }, "1.2" => "1.5"
        ///    { "1.1", "1.2", "1.5", "2.0" }, "1.4" => null
        ///    { "1.1", "1.2", "1.5", "2.0" }, "2.0" => null
        /// </example>
        public string GetNextVersionFromList(IEnumerable<string> versions, string currentVersion) {
            string[] list = versions.ToArray();
            int indexInList = Array.IndexOf(list, currentVersion);
            int indexOfLastElement = list.Length - 1;
            if (indexInList == -1 || indexInList == indexOfLastElement)
                return null;
            else
                return list.ElementAt(indexInList + 1);
        }

        /// <summary>
        ///  Calcuates the sum of two vectors:
        ///  (x1, x2, ..., xn) + (y1, y2, ..., yn) = (x1+y1, x2+y2, ..., xn+yn)
        /// </summary>
        /// <param name="vector1">source vector 1</param>
        /// <param name="vector2">source vector 2</param>
        /// <returns>
        ///  Returns the sum of two vectors
        /// </returns>
        /// <example>
        ///   { 1, 2, 3 } + { 10, 20, 30 } => { 11, 22, 33 }
        ///   { 1, 1, 1 } + { -1, -1, -1 } => { 0, 0, 0 }
        /// </example>
        public IEnumerable<int> GetSumOfVectors(IEnumerable<int> vector1, IEnumerable<int> vector2) {
            // TODO : Implement GetSumOfVectors
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Calcuates the product of two vectors:
        ///  (x1, x2, ..., xn) + (y1, y2, ..., yn) = x1*y1 + x2*y2 + ... + xn*yn
        /// </summary>
        /// <param name="vector1">source vector 1</param>
        /// <param name="vector2">source vector 2</param>
        /// <returns>
        ///  Returns the product of two vectors
        /// </returns>
        /// <example>
        ///   { 1, 2, 3 } * { 1, 2, 3 } => 1*1 + 2*2 + 3*3 = 14
        ///   { 1, 1, 1 } * { -1, -1, -1 } => 1*-1 + 1*-1 + 1*-1 = -3
        ///   { 1, 1, 1 } * { 0, 0, 0 } => 1*0 + 1*0 +1*0 = 0
        /// </example>
        public int GetProductOfVectors(IEnumerable<int> vector1, IEnumerable<int> vector2) {
            // TODO : Implement GetProductOfVectors
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Finds all boy+girl pair
        /// </summary>
        /// <param name="boys">boys' names</param>
        /// <param name="girls">girls' names</param>
        /// <returns>
        ///   Returns all combination of boys and girls names 
        /// </returns>
        /// <example>
        ///  {"John", "Josh", "Jacob" }, {"Ann", "Alice"} => {"John+Ann","John+Alice", "Josh+Ann","Josh+Alice", "Jacob+Ann", "Jacob+Alice" }
        ///  {"John"}, {"Alice"} => {"John+Alice"}
        ///  {"John"}, { } => { }
        ///  { }, {"Alice"} => { }
        /// </example>
        public IEnumerable<string> GetAllPairs(IEnumerable<string> boys, IEnumerable<string> girls) {
            // TODO : Implement GetAllPairs
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Calculates the average of all double values from object collection
        /// </summary>
        /// <param name="data">the source sequence</param>
        /// <returns>
        ///  Returns the average of all double values
        /// </returns>
        /// <example>
        ///  { 1.0, 2.0, null, "a" } => 1.5
        ///  { "1.0", "2.0", "3.0" } => 0.0  (no double values, strings only)
        ///  { null, 1.0, true } => 1.0
        ///  { } => 0.0
        /// </example>
        public double GetAverageOfDoubleValues(IEnumerable<object> data) {
            // TODO : Implement GetAverageOfDoubleValues
            throw new NotImplementedException();
        }

    }

    
}
