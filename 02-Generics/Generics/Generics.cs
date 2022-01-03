using System;
using System.Collections.Generic;
using System.Text;

namespace Task.Generics {

	public static class ListConverter {

		private static char ListSeparator = ',';  // Separator used to separate values in string

		/// <summary>
		///   Converts a source list into a string representation
		/// </summary>
		/// <typeparam name="T">type  of list items</typeparam>
		/// <param name="list">source list</param>
		/// <returns>
		///   Returns the string representation of a list 
		/// </returns>
		/// <example>
		///   { 1,2,3,4,5 } => "1,2,3,4,5"
		///   { '1','2','3','4','5'} => "1,2,3,4,5"
		///   { true, false } => "True,False"
		///   { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan } => "Black,Blue,Cyan"
		///   { new TimeSpan(1, 0, 0), new TimeSpan(0, 0, 30) } => "01:00:00,00:00:30",
		/// </example>
		public static string ConvertToString<T>(this IEnumerable<T> list) {
			if(list != null)
            {
				StringBuilder sb = new StringBuilder();
				int counter = 0;
				foreach(var item in list)
                {
					if(counter != 0)
                    {
						sb.Append(ListSeparator);
                    }
					sb.Append(item.ToString());
					counter++;
                }
				return sb.ToString();
            }
			return null;
		}

		/// <summary>
		///   Converts the string respresentation to the list of items
		/// </summary>
		/// <typeparam name="T">required type of output items</typeparam>
		/// <param name="list">string representation of the list</param>
		/// <returns>
		///   Returns the list of items from specified string
		/// </returns>
		/// <example>
		///  "1,2,3,4,5" for int => {1,2,3,4,5}
		///  "1,2,3,4,5" for char => {'1','2','3','4','5'}
		///  "1,2,3,4,5" for string => {"1","2","3","4","5"}
		///  "true,false" for bool => { true, false }
		///  "Black,Blue,Cyan" for ConsoleColor => { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan }
		///  "1:00:00,0:00:30" for TimeSpan =>  { new TimeSpan(1, 0, 0), new TimeSpan(0, 0, 30) },
		///  </example>
		public static IEnumerable<T> ConvertToList<T>(this string list) {
			if(list != null)
            {
				Type t = typeof(T);

				if (t.Equals(typeof(int)))
				{
					return ConvertToInt<T>(list);
				}
				

				if (t.Equals(typeof(char)))
                {
					return ConvertToChar<T>(list);
                }

				if(t.Equals(typeof(bool)))
                {
					return ConvertToBool<T>(list);
				}

				if(t.Equals(typeof(ConsoleColor)))
                {
					return ConvertToConsoleColor<T>(list);
				}

                if (t.Equals(typeof(TimeSpan)))
                {
					return ConvertToTimeSpan<T>(list);
                }
            }
            return null;
		}
		private static IEnumerable<T> ConvertToInt<T>(string list)
		{
			string[] stringArray = list.Split(',');
			List<int> resultList = new List<int>();
			int number;
			foreach (var s in stringArray)
			{
				if (Int32.TryParse(s, out number))
				{
					resultList.Add(number);
				}
			}
			return (IEnumerable<T>)resultList;
		}
		private static IEnumerable<T> ConvertToChar<T>(string list)
        {
			string[] stringArray = list.Split(',');
			List<char> resultList = new List<char>();
			char result;
			foreach (var s in stringArray)
			{
				result = Convert.ToChar(s);
				resultList.Add(result);
			}
			return (IEnumerable<T>)resultList;
		}
		private static IEnumerable<T> ConvertToBool<T>(string list)
        {
			string[] stringArray = list.Split(',');
			List<bool> resultList = new List<bool>();
			bool boolVariable;
			foreach (var s in stringArray)
			{
				if (bool.TryParse(s, out boolVariable))
				{
					resultList.Add(boolVariable);
				}
			}
			return (IEnumerable<T>)resultList;
		}
		private static IEnumerable<T> ConvertToConsoleColor<T>(string list)
        {
			string[] stringArray = list.Split(',');
			List<ConsoleColor> resultList = new List<ConsoleColor>();
			ConsoleColor color;
			foreach (var s in stringArray)
			{
				if (Enum.TryParse(s, out color))
				{
					if (Enum.IsDefined(typeof(ConsoleColor), color))
					{
						resultList.Add(color);
					}
				}
			}
			return (IEnumerable<T>)resultList;
		}
		private static IEnumerable<T> ConvertToTimeSpan<T>(string list)
        {
			string[] stringArray = list.Split(',');
			List<TimeSpan> dates = new List<TimeSpan>();
			TimeSpan date;
			foreach (var s in stringArray)
			{
				if (TimeSpan.TryParse(s, out date))
				{
					dates.Add(date);
				}
			}
			return (IEnumerable<T>)dates;
		}

	}
	

	public static class ArrayExtentions {

		/// <summary>
		///   Swaps the one element of source array with another
		/// </summary>
		/// <typeparam name="T">required type of</typeparam>
		/// <param name="array">source array</param>
		/// <param name="index1">first index</param>
		/// <param name="index2">second index</param>
		public static void SwapArrayElements<T>(this T[] array, int index1, int index2) {
			if(index1 > array.Length - 1 || index2 > array.Length)
            {
				throw new IndexOutOfRangeException();
            }
			if (index1 == index2)
				return;

			if(array.Length > 1 && index1 >= 0 &&  index2 >= 0 )
            {
				T temp = array[index1];
				array[index1] = array[index2];
				array[index2] = temp;
            }
		}

		/// <summary>
		///   Sorts the tuple array by specified column in ascending or descending order
		/// </summary>
		/// <param name="array">source array</param>
		/// <param name="sortedColumn">index of column</param>
		/// <param name="ascending">true if ascending order required; otherwise false</param>
		/// <example>
		///   source array : 
		///   { 
		///     { 1, "a", false },
		///     { 3, "b", false },
		///     { 2, "c", true  }
		///   }
		///   result of SortTupleArray(array, 0, true) is sort rows by first column in a ascending order: 
		///   { 
		///     { 1, "a", false },
		///     { 2, "c", true  },
		///     { 3, "b", false }
		///   }
		///   result of SortTupleArray(array, 1, false) is sort rows by second column in a descending order: 
		///   {
		///     { 2, "c", true  },
		///     { 3, "b", false }
		///     { 1, "a", false },
		///   }
		/// </example>
		public static void SortTupleArray<T1, T2, T3>(this Tuple<T1, T2, T3>[] array, int sortedColumn, bool ascending) where T1 : IComparable
																														where T2 : IComparable
																														where T3 : IComparable
		{
			if(sortedColumn < 0 || sortedColumn > 2)
            {
				throw new IndexOutOfRangeException();
            }
			if(ascending)
            {
				switch (sortedColumn)
				{
					case 0:
						Array.Sort(array, (x, y) => x.Item1.CompareTo(y.Item1));
						break;
					case 1:
						Array.Sort(array, (x, y) => x.Item2.CompareTo(y.Item2));
						break;
					case 2:
						Array.Sort(array, (x, y) => x.Item3.CompareTo(y.Item3));
						break;
				}
			}
			else
            {
				switch (sortedColumn)
				{
					case 0:
						Array.Sort(array, (x, y) => y.Item1.CompareTo(x.Item1));
						break;
					case 1:
						Array.Sort(array, (x, y) => y.Item2.CompareTo(x.Item2));
						break;
					case 2:
						Array.Sort(array, (x, y) => y.Item3.CompareTo(x.Item3));
						break;
				}
			}
            

            // TODO :SortTupleArray<T1, T2, T3>
            // HINT : Add required constraints to generic types
        }

    }

	/// <summary>
	///   Generic singleton class
	/// </summary>
	/// <example>
	///   This code should return the same MyService object every time:
	///   MyService singleton = Singleton<MyService>.Instance;
	/// </example>
	public static class Singleton<T> {
		// TODO : Implement generic singleton class 

		public static T Instance {
			get { throw new NotImplementedException(); }
		}
	}



	public static class FunctionExtentions {
		/// <summary>
		///   Tries to invoke the specified function up to 3 times if the result is unavailable 
		/// </summary>
		/// <param name="function">specified function</param>
		/// <returns>
		///   Returns the result of specified function, if WebException occurs duaring request then exception should be logged into trace 
		///   and the new request should be started (up to 3 times).
		/// </returns>
		/// <example>
		///   Sometimes if network is unstable it is required to try several request to get data:
		///   
		///   Func<string> f1 = ()=>(new System.Net.WebClient()).DownloadString("http://www.google.com/");
		///   string data = f1.TimeoutSafeInvoke();
		///   
		///   If the first attemp to download data is failed by WebException then exception should be logged to trace log and the second attemp should be started.
		///   The second attemp has the same workflow.
		///   If the third attemp fails then this exception should be rethrow to the application.
		/// </example>
		public static T TimeoutSafeInvoke<T>(this Func<T> function) {
			// TODO : Implement TimeoutSafeInvoke<T>
			throw new NotImplementedException();
		}


		/// <summary>
		///   Combines several predicates using logical AND operator 
		/// </summary>
		/// <param name="predicates">array of predicates</param>
		/// <returns>
		///   Returns a new predicate that combine the specified predicated using AND operator
		/// </returns>
		/// <example>
		///   var result = CombinePredicates(new Predicate<string>[] {
		///            x=> !string.IsNullOrEmpty(x),
		///            x=> x.StartsWith("START"),
		///            x=> x.EndsWith("END"),
		///            x=> x.Contains("#")
		///        })
		///   should return the predicate that identical to 
		///   x=> (!string.IsNullOrEmpty(x)) && x.StartsWith("START") && x.EndsWith("END") && x.Contains("#")
		///
		///   The following example should create predicate that returns true if int value between -10 and 10:
		///   var result = CombinePredicates(new Predicate<int>[] {
		///            x=> x>-10,
		///            x=> x<10
		///       })
		/// </example>
		public static Predicate<T> CombinePredicates<T>(Predicate<T>[] predicates) {
			// TODO : Implement CombinePredicates<T>
			throw new NotImplementedException();
		}

	}


}
