using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace FindMyFamilies.Util {
	/// <summary>
	/// This class contains all the string manipulations functions.
	/// </summary>
	/// Note:
	/// One string function is not implemented
	/// public static string StrConv(stringValue, nConversionSetting [, nLocaleID])
	public class Strings {
		/// <summary>
		/// Removes leading and trailing spaces from stringValue
		/// </summary>
		/// <param name="stringValue"></param>
		public static string TrimAll(string stringValue) {
			return stringValue.Trim();
		}

		/// <summary>
		/// Receives a character as a parameter and returns its ANSI code
		/// <pre>
		/// Example
		/// Asc('#');		//returns 35
		/// </pre>
		/// </summary>
		/// <param name="cCharacter"> </param>
		public static int Asc(char character) {
			return (int) character;
		}

		/// <summary>
		/// Receives two strings as parameters and searches for one string within another. 
		/// If found, returns the beginning numeric position otherwise returns 0
		/// <pre>
		/// Example:
		/// Strings.SearchFromLeft("D", "Joe Doe");	//returns 5
		/// </pre>
		/// </summary>
		/// <param name="searchFor"> </param>
		/// <param name="searchIn"> </param>
		public static int SearchFromLeft(string searchFor, string searchIn) {
			return searchIn.IndexOf(searchFor) + 1;
		}

		/// <summary>
		/// Receives two strings and an occurence position (1st, 2nd etc) as parameters and 
		/// searches for one string within another for that position. 
		/// If found, returns the beginning numeric position otherwise returns 0
		/// <pre>
		/// Example:
		/// Strings.SearchFromLeft("o", "Joe Doe", 1);	//returns 2
		/// Strings.SearchFromLeft("o", "Joe Doe", 2);	//returns 6
		/// </pre>
		/// </summary>
		/// <param name="searchFor"> </param>
		/// <param name="searchIn"> </param>
		/// <param name="occurence"> </param>
		public static int SearchFromLeft(string searchFor, string searchIn, int occurence) {
			return __at(searchFor, searchIn, occurence, 1);
		}

		/// Private Implementation: This is the actual implementation of the SearchFromLeft() and SearchFromRight() functions. 
		/// Receives two strings, the stringValue in which search is performed and the stringValue to search for. 
		/// Also receives an occurence position and the mode (1 or 0) that specifies whether it is a search
		/// from Left to Right (for SearchFromLeft() function)  or from Right to Left (for SearchFromRight() function)
		private static int __at(string searchFor, string searchIn, int occurence, int mode) {
			//In this case we actually have to locate the occurence
			int i = 0;
			int occured = 0;
			int pos = 0;
			if (mode == 1) {
				pos = 0;
			} else {
				pos = searchIn.Length;
			}

			//Loop through the string and get the position of the requiref occurence
			for (i = 1; i <= occurence; i++) {
				if (mode == 1) {
					pos = searchIn.IndexOf(searchFor, pos);
				} else {
					pos = searchIn.LastIndexOf(searchFor, pos);
				}

				if (pos < 0) {
					//This means that we did not search the item
					break;
				} else {
					//Increment the occured counter based on the current mode we are in
					occured++;

					//Check if this is the occurence we are looking for
					if (occured == occurence) {
						return pos + 1;
					} else {
						if (mode == 1) {
							pos++;
						} else {
							pos--;
						}

					}
				}
			}
			//We never found our guy if we reached here
			return 0;
		}

		/// <summary>
		/// Receives two strings as parameters and searches for one string within another. 
		/// This function ignores the case and if found, returns the beginning numeric position 
		/// otherwise returns 0
		/// <pre>
		/// Example:
		/// Strings.SearchFromLeftC("d", "Joe Doe");	//returns 5
		/// </pre>
		/// </summary>
		/// <param name="searchFor"> </param>
		/// <param name="searchIn"> </param>
		public static int SearchFromLeftIgnoreCase(string searchFor, string searchIn) {
			return searchIn.ToLower().IndexOf(searchFor.ToLower()) + 1;
		}

		/// <summary>
		/// Receives two strings and an occurence position (1st, 2nd etc) as parameters and 
		/// searches for one string within another for that position. This function ignores the
		/// case of both the strings and if found, returns the beginning numeric position 
		/// otherwise returns 0.
		/// <pre>
		/// Example:
		/// Strings.SearchFromLeftC("d", "Joe Doe", 1);	//returns 5
		/// Strings.SearchFromLeftC("O", "Joe Doe", 2);	//returns 6
		/// </pre>
		/// </summary>
		/// <param name="searchFor"> </param>
		/// <param name="searchIn"> </param>
		/// <param name="occurence"> </param>
		public static int SearchFromLeftOccur(string searchFor, string searchIn, int occurence) {
			return __at(searchFor.ToLower(), searchIn.ToLower(), occurence, 1);
		}

		/// <summary>
		/// Receives an integer ANSI code and returns a character associated with it
		/// <pre>
		/// Example:
		/// Strings.Chr(35);		//returns '#'
		/// </pre>
		/// </summary>
		/// <param name="nAnsiCode"> </param>
		public static char Chr(int ansiCode) {
			return (char) ansiCode;
		}

		/// <summary>
		/// Replaces each character in a character stringValue that matches a character
		/// in a second character stringValue with the corresponding character in a 
		/// third character stringValue
		/// </summary>
		/// <example>
		/// Console.WriteLine(ChrTran("ABCDEF", "ACE", "XYZ"));  //Displays XBYDZF
		/// Console.WriteLine(ChrTran("ABCD", "ABC", "YZ"));	//Displays YZD
		/// Console.WriteLine(ChrTran("ABCDEF", "ACE", "XYZQRST"));	//Displays XBYDZF
		/// </example>
		/// <param name="searchIn"> </param>
		/// <param name="searchFor"> </param>
		/// <param name="replaceWith"> </param>
		public static string SearchAndReplaceChar(string searchIn, string searchFor, string replaceWith) {
			string returnValue = searchIn;
			string replaceChar;
			for (int i = 0; i < searchFor.Length; i++) {
				if (replaceWith.Length <= i) {
					replaceChar = "";
				} else {
					replaceChar = replaceWith[i].ToString();
				}

				returnValue = SearchAndReplace(returnValue, searchFor[i].ToString(), replaceChar);
			}
			return returnValue;
		}

		/// <summary>
		/// Receives a file name as a parameter and returns the contents of that file
		/// as a string.
		/// </summary>
		/// Example:
		/// Strings.FileToStr("c:\\My Folders\\MyFile.txt");	//returns the contents of the file
		/// </pre>
		/// </summary>
		/// <param name="fileName"> </param>
		public static string FileToStr(string fileName) {
			//Create a StreamReader and open the file
			StreamReader reader = File.OpenText(fileName);

			//Read all the contents of the file in a string
			string stringValue = reader.ReadToEnd();

			//Close the StreamReader and return the string
			reader.Close();
			return stringValue;
		}

		/// <summary>
		/// Receives a string as a parameter and counts the number of words in that string
		/// <pre>
		/// Example:
		/// string stringValue = "Joe Doe is a good man";
		/// Strings.GetWordCount(stringValue);		//returns 6
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		public static long GetWordCount(string stringValue) {
			int i = 0;
			long length = stringValue.Length;
			long wordCount = 0;

			//Begin by checking for the first word
			if (!Char.IsWhiteSpace(stringValue[0])) {
				wordCount ++;
			}

			//Now look for white spaces and count each word
			for (i = 0; i < length; i++) {
				//Check for a space to begin counting a word
				if (Char.IsWhiteSpace(stringValue[i])) {
					//We think we encountered a word
					//Remove any following white spaces if any after this word
					do {
						//Check if we have reached the limit and if so then exit the loop
						i ++;
						if (i >= length) {
							break;
						}
						if (!Char.IsWhiteSpace(stringValue[i])) {
							wordCount++;
							break;
						}
					} while (true);

				}

			}
			return wordCount;
		}

		/// <summary>
		/// Based on the position specified, returns a word from a string 
		/// Receives a string as a parameter and counts the number of words in that string
		/// <pre>
		/// Example:
		/// string stringValue = "Joe Doe is a good man";
		/// Strings.GetWordNumber(stringValue, 5);		//returns "good"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="wordPosition"> </param>
		public static string GetWordNumber(string stringValue, int wordPosition) {
			int begipos = Strings.SearchFromLeft(" ", stringValue, wordPosition - 1);
			int endPos = Strings.SearchFromLeft(" ", stringValue, wordPosition);
			return Strings.Extract(stringValue, begipos + 1, endPos - 1 - begipos);
		}

		/// <summary>
		/// Returns a bool indicating if the first character in a string is an alphabet or not
		/// <pre>
		/// Example:
		/// Strings.IsAlpha("Joe Doe");		//returns true
		/// 
		/// Tip: This method uses Char.IsAlpha(char) to check if it is an alphabet or not. 
		///      In order to check if the first character is a digit use Char.IsDigit(char)
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		public static bool IsAlpha(string stringValue) {
			//Check if the first character is a letter
			return Char.IsLetter(stringValue[0]);
		}

		/// <summary>
		/// Checks if the first character of a string is a lowercase char and if so then returns true
		/// <pre>
		/// Example:
		/// Strings.IsLower("MyName");	//returns false
		/// Strings.IsLower("mYnAme");	//returns true
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		public static bool IsLower(string stringValue) {
			try {
				//Get the first character in the string
				stringValue = stringValue.Substring(0, 1);

				//Return a bool indicating if the char is an lowercase or not
				return stringValue == stringValue.ToLower();
			} catch {
				//In case of an error return false
				return false;
			}
		}

		/// <summary>
		/// Checks if the first character of a string is an uppercase and if so then returns true
		/// <pre>
		/// Example:
		/// Strings.IsUpper("MyName");	//returns true
		/// Strings.IsUpper("mYnAme");	//returns false
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		public static bool IsUpper(string stringValue) {
			try {
				//Get the first character in the string
				stringValue = stringValue.Substring(0, 1);

				//Return a bool indicating if the char is an uppercase or not
				return stringValue == stringValue.ToUpper();
			} catch {
				//In case of an error return false
				return false;
			}
		}

		/// <summary>
		/// Receives a string and the number of characters as parameters and returns the
		/// specified number of leftmost characters of that string
		/// <pre>
		/// Example:
		/// Strings.Left("Joe Doe", 3);	//returns "Joe"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="length"> </param>
		public static string Left(string stringValue, int length) {
			string returnValue = stringValue;

			if (stringValue != null) {
				if (stringValue.Length >= length) {
					returnValue = stringValue.Substring(0, length);
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Receives a string and the number of characters as parameters and returns the
		/// specified number of leftmost characters of that string
		/// <pre>
		/// Example:
		/// Strings.Left("Joe Doe", 3);	//returns "Joe"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="length"> </param>
		public static string Remove(string stringValue, int start, int length) {
			string returnValue = "";

			if (stringValue != null) {
				if (stringValue.Length >= length) {
					int end = start + length;
					for (int i = 0; i < stringValue.Length; i++) {
						if (i < start) {
							returnValue += stringValue[i];
						} else if (i > end) {
							returnValue += stringValue[i];
						}
					}
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Removes the leading spaces in stringValue
		/// </summary>
		/// <param name="stringValue"> </param>
		public static string TrimLeft(string stringValue) {
			//Hint: Pass null as the first parameter to remove white spaces
			return stringValue.TrimStart(null);
		}

		/// <summary>
		/// Returns the number of occurences of a character within a string
		/// <pre>
		/// Example:
		/// Strings.Occurs('o', "Joe Doe");		//returns 2
		/// 
		/// Tip: If we have a string say stringValue, then stringValue[3] gives us the 3rd character in the string
		/// </pre>
		/// </summary>
		/// <param name="cChar"> </param>
		/// <param name="stringValue"> </param>
		public static int Occurs(char charValue, string stringValue) {
			int i, occured = 0;

			//Loop through the string
			for (i = 0; i < stringValue.Length; i++) {
				//Check if each stringValue is equal to the one we want to check against
				if (stringValue[i] == charValue) {
					//if  so increment the counter
					occured++;
				}
			}
			return occured;
		}

		/// <summary>
		/// Returns the number of occurences of one string within another string
		/// <pre>
		/// Example:
		/// Strings.Occurs("oe", "Joe Doe");		//returns 2
		/// Strings.Occurs("Joe", "Joe Doe");		//returns 1
		/// 
		/// Tip: String.IndexOf() searches the string (starting from left) for another character or string stringValue
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="withinString"> </param>
		public static int Occurs(string stringValue, string withinString) {
			int pos = 0;
			int occured = 0;
			do {
				//Look for the search string in the stringValue
				pos = withinString.IndexOf(stringValue, pos);

				if (pos < 0) {
					//This means that we did not search the item
					break;
				} else {
					//Increment the occured counter based on the current mode we are in
					occured++;
					pos++;
				}
			} while (true);

			//Return the number of occurences
			return occured;
		}

		/// <summary>
		/// Receives a string and the length of the result string as parameters. Pads blank 
		/// characters on the both sides of this string and returns a string with the length specified.
		/// <pre>
		/// Example:
		/// Strings.PadBlankBothSides("Joe Doe", 10);		//returns " Joe Doe  "
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="resultSize"> </param>
		public static string PadBlankBothSides(string stringValue, int resultSize) {
			//Determine the number of padding characters
			int nPaddTotal = resultSize - stringValue.Length;
			int halfLength = (int) (nPaddTotal/2);

			stringValue = PadLeft(stringValue, stringValue.Length + halfLength);
			return stringValue.PadRight(resultSize);
		}

		/// <summary>
		/// Receives a string, the length of the result string and the padding character as 
		/// parameters. Pads the padding character on both sides of this string and returns a string 
		/// with the length specified.
		/// <pre>
		/// Example:
		/// Strings.PadBothSides("Joe Doe", 10, 'x');		//returns "xJoe Doexx"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="resultSize"> </param>
		/// <param name="paddingChar"> </param>
		public static string PadBothSides(string stringValue, int resultSize, char paddingChar) {
			//Determine the number of padding characters
			int nPaddTotal = resultSize - stringValue.Length;
			int halfLength = (int) (nPaddTotal/2);

			stringValue = PadLeft(stringValue, stringValue.Length + halfLength, paddingChar);
			return stringValue.PadRight(resultSize, paddingChar);
		}

		/// <summary>
		/// Receives a string and the length of the result string as parameters. Pads blank 
		/// characters on the left of this string and returns a string with the length specified.
		/// <pre>
		/// Example:
		/// Strings.PadL("Joe Doe", 10);		//returns "   Joe Doe"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="resultSize"> </param>
		public static string PadLeft(string stringValue, int resultSize) {
			return stringValue.PadLeft(resultSize);
		}

		/// <summary>
		/// Receives a string, the length of the result string and the padding character as 
		/// parameters. Pads the padding character on the left of this string and returns a string 
		/// with the length specified.
		/// <pre>
		/// Example:
		/// Strings.PadLeft("Joe Doe", 10, 'x');		//returns "xxxJoe Doe"
		/// 
		/// Tip: Use single quote to create a character type data and double quotes for strings
		/// </pre>
		/// </summary>
		public static string PadLeft(string stringValue, int resultSize, char paddingChar) {
			return stringValue.PadLeft(resultSize, paddingChar);
		}

		/// <summary>
		/// Receives a string and the length of the result string as parameters. Pads blank 
		/// characters on the right of this string and returns a string with the length specified.
		/// <pre>
		/// Example:
		/// Strings.PadRight("Joe Doe", 10);		//returns "Joe Doe   "
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="resultSize"> </param>
		public static string PadRight(string stringValue, int resultSize) {
			return stringValue.PadRight(resultSize);
		}

		/// <summary>
		/// Receives a string, the length of the result string and the padding character as 
		/// parameters. Pads the padding character on the right of this string and returns a string 
		/// with the length specified.
		/// <pre>
		/// Example:
		/// Strings.PadL("Joe Doe", 10, 'x');		//returns "Joe Doexxx"
		/// 
		/// Tip: Use single quote to create a character type data and double quotes for strings
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="resultSize"> </param>
		/// <param name="paddingChar"> </param>
		public static string PadRright(string stringValue, int resultSize, char paddingChar) {
			return stringValue.PadRight(resultSize, paddingChar);
		}

		/// <summary>
		/// Receives a string as a parameter and returns the string in Proper format (makes each letter after a space capital)
		/// <pre>
		/// Example:
		/// Strings.Proper("joe doe is a good man");	//returns "Joe Doe Is A Good Man"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// ToDo: Split the string instead and you do not have to worry about comparing each char
		public static string Proper(string stringValue) {
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(stringValue);

			int i, j = 0;
			int length = stringValue.Length;

			for (i = 0; i < length; i++) {
				//look for a blank space and once found make the next character to uppercase
				if ((i == 0) || (char.IsWhiteSpace(stringValue[i]))) {
					//Handle the first character differently
					if (i == 0) {
						j = i;
					} else {
						j = i + 1;
					}

					//Make the next character uppercase and update the stringBuilder
					sb.Remove(j, 1);
					sb.Insert(j, Char.ToUpper(stringValue[j]));
				}
			}
			return sb.ToString();
		}

		public static string GetConstant(string stringValue) {
			string constant = stringValue[0].ToString();

			for (int i = 1; i < stringValue.Length; i++) {
				if (IsUpper(stringValue[i].ToString()) && IsLower(stringValue[i - 1].ToString())) {
					constant += "_";
				}
				constant += stringValue[i];
			}
			return constant.ToUpper();
		}

		/// <summary>
		/// Receives two strings as parameters and searches for one string within another. 
		/// The search is performed starting from Right to Left and if found, returns the 
		/// beginning numeric position otherwise returns 0
		/// <pre>
		/// Example:
		/// Strings.SearchFromRight("o", "Joe Doe");	//returns 6
		/// </pre>
		/// </summary>
		/// <param name="searchFor"> </param>
		/// <param name="searchIn"> </param>
		public static int SearchFromRight(string searchFor, string searchIn) {
			return searchIn.LastIndexOf(searchFor) + 1;
		}

		/// <summary>
		/// Receives two strings as parameters and an occurence position as parameters. 
		/// The function searches for one string within another and the search is performed 
		/// starting from Right to Left and if found, returns the beginning numeric position 
		/// otherwise returns 0
		/// <pre>
		/// Example:
		/// Strings.SearchFromRight("o", "Joe Doe", 1);	//returns 6
		/// Strings.SearchFromRight("o", "Joe Doe", 2);	//returns 2
		/// </pre>
		/// </summary>
		/// <param name="searchFor"> </param>
		/// <param name="searchIn"> </param>
		/// <param name="occurence"> </param>
		public static int SearchFromRight(string searchFor, string searchIn, int occurence) {
			return __at(searchFor, searchIn, occurence, 0);
		}

		/// <summary>
		/// Receives a string stringValue and a numeric value indicating number of time
		/// and replicates that string for the specified number of times.
		/// <pre>
		/// Example:
		/// Strings.Replicate("Joe", 5);		//returns JoeJoeJoeJoeJoe
		/// 
		/// Tip: Use a StringBuilder when lengthy string manipulations are required.
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="nTimes"> </param>
		public static string Replicate(string stringValue, int nTimes) {
			//Create a stringBuilder
			StringBuilder sb = new StringBuilder();

			//Insert the stringValue into the StringBuilder for nTimes
			sb.Insert(0, stringValue, nTimes);

			//Convert it to a string and return it back
			return sb.ToString();
		}

		/// <summary>
		/// Receives a string and the number of characters as parameters and returns the
		/// specified number of rightmost characters of that string
		/// <pre>
		/// Example:
		/// Strings.Right("Joe Doe", 3);	//returns "Doe"
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="length"> </param>
		public static string Right(string stringValue, int length) {
			string returnValue = stringValue;

			if (stringValue != null) {
				if (stringValue.Length >= length) {
					returnValue = stringValue.Substring(stringValue.Length - length);
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Removes the trailing spaces in stringValue
		/// </summary>
		/// <example>
		/// </example>
		/// <param name="stringValue"> </param>
		public static string TrimRight(string stringValue) {
			//Hint: Pass null as the first parameter to remove white spaces
			return stringValue.TrimEnd(null);
		}

		/// <summary>
		/// Receives an integer as a parameter and returns an empty string of that length
		/// <pre>
		/// Example:
		/// Strings.Space(20);	//returns a string with 20 spaces
		/// </pre>
		/// </summary>
		/// <param name="nSpaces"> </param>
		public static string Space(int nSpaces) {
			//Create a new string and return those many spaces in it
			char val = ' ';
			return new string(val, nSpaces);
		}

		/// <summary>
		/// Receives a string along with starting and ending delimiters and returns the 
		/// part of the string between the delimiters. Receives a beginning occurence
		/// to begin the extraction from and also receives a flag (0/1) where 1 indicates
		/// that the search should be case insensitive.
		/// <pre>
		/// Example:
		/// string stringValue = "JoeDoeJoeDoe";
		/// Strings.StrExtract(stringValue, "o", "eJ", 1, 0);		//returns "eDo"
		/// </pre>
		/// </summary>
		public static string ExtractString(string searchExpression, string beginDelimiter, string endDelimiter, int beginOccurence, int nFlags) {
			string cstring = searchExpression;
			string cb = beginDelimiter;
			string ce = endDelimiter;
			string returnValue = "";

			//Check for case-sensitive or insensitive search
			if (nFlags == 1) {
				cstring = cstring.ToLower();
				cb = cb.ToLower();
				ce = ce.ToLower();
			}

			//Lookup the position in the string
			int nbpos = SearchFromLeft(cb, cstring, beginOccurence) + cb.Length - 1;
			int nepos = cstring.IndexOf(ce, nbpos + 1);

			//Extract the part of the strign if we get it right
			if (nepos > nbpos) {
				returnValue = searchExpression.Substring(nbpos, nepos - nbpos);
			}

			return returnValue;
		}

		/// <summary>
		/// Receives a string and a delimiter as parameters and returns a string starting 
		/// from the position after the delimiter
		/// <pre>
		/// Example:
		/// string stringValue = "JoeDoeJoeDoe";
		/// Strings.StrExtract(stringValue, "o");		//returns "eDoeJoeDoe"
		/// </pre>
		/// </summary>
		/// <param name="searchExpression"> </param>
		/// <param name="beginDelim"> </param>
		public static string ExtractString(string searchExpression, string beginDelim) {
			int nbpos = SearchFromLeft(beginDelim, searchExpression);
			return searchExpression.Substring(nbpos + beginDelim.Length - 1);
		}

		/// <summary>
		/// Receives a string along with starting and ending delimiters and returns the 
		/// part of the string between the delimiters
		/// <pre>
		/// Example:
		/// string stringValue = "JoeDoeJoeDoe";
		/// Strings.StrExtract(stringValue, "o", "eJ");		//returns "eDo"
		/// </pre>
		/// </summary>
		/// <param name="searchExpression"> </param>
		/// <param name="beginDelimiter"> </param>
		/// <param name="endDelimiter"> </param>
		public static string ExtractString(string searchExpression, string beginDelimiter, string endDelimiter) {
			return ExtractString(searchExpression, beginDelimiter, endDelimiter, 1, 0);
		}

		/// <summary>
		/// Receives a string along with starting and ending delimiters and returns the 
		/// part of the string between the delimiters. It also receives a beginning occurence
		/// to begin the extraction from.
		/// <pre>
		/// Example:
		/// string stringValue = "JoeDoeJoeDoe";
		/// Strings.StrExtract(stringValue, "o", "eJ", 2);		//returns ""
		/// </pre>
		/// </summary>
		/// <param name="searchExpression"> </param>
		/// <param name="beginDelimiter"> </param>
		/// <param name="endDelimiter"> </param>
		/// <param name="beginOccurence"> </param>
		public static string ExtractString(string searchExpression, string beginDelimiter, string endDelimiter, int beginOccurence) {
			return ExtractString(searchExpression, beginDelimiter, endDelimiter, beginOccurence, 0);
		}

		/// <summary>
		/// Receives a string and a file name as parameters and writes the contents of the
		/// string to that file
		/// <pre>
		/// Example:
		/// string stringValue = "This is the line we want to insert in our file.";
		/// Strings.StringToFile(stringValue, "c:\\My Folders\\MyFile.txt");
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="fileName"> </param>
		public static void StringToFile(string stringValue, string fileName) {
			//Check if the sepcified file exists
			if (File.Exists(fileName) == true) {
				//If so then Erase the file first as in this case we are overwriting
				File.Delete(fileName);
			}

			//Create the file if it does not exist and open it
			FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);

			//Create a writer for the file
			StreamWriter writer = new StreamWriter(fileStream);

			//Write the contents
			writer.Write(stringValue);
			writer.Flush();
			writer.Close();

			fileStream.Close();
		}

		/// <summary>
		/// Receives a string and a file name as parameters and writes the contents of the
		/// string to that file. Receives an additional parameter specifying whether the 
		/// contents should be appended at the end of the file
		/// <pre>
		/// Example:
		/// string stringValue = "This is the line we want to insert in our file.";
		/// Strings.StringToFile(stringValue, "c:\\My Folders\\MyFile.txt");
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="fileName"> </param>
		/// <param name="additive"> </param>
		public static void StringToFile(string stringValue, string fileName, bool additive) {
			//Create the file if it does not exist and open it
			FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			//Create a writer for the file
			StreamWriter writer = new StreamWriter(fileStream);

			//Set the pointer to the end of file
			writer.BaseStream.Seek(0, SeekOrigin.End);

			//Write the contents
			writer.Write(stringValue);
			writer.Flush();
			writer.Close();
			fileStream.Close();
		}

		/// <summary>
		/// Searches one string into another string and replaces all occurences with
		/// a blank character.
		/// <pre>
		/// Example:
		/// Strings.StrTran("Joe Doe", "o");		//returns "J e D e" :)
		/// </pre>
		/// </summary>
		/// <param name="searchIn"> </param>
		/// <param name="searchFor"> </param>
		public static string SearchAndReplaceBlank(string searchIn, string searchFor) {
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(searchIn);

			//Call the Replace() method of the StringBuilder
			return sb.Replace(searchFor, " ").ToString();
		}

		/// <summary>
		/// Searches one string into another string and replaces all occurences with
		/// a third string.
		/// <pre>
		/// Example:
		/// Strings.StrTran("Joe Doe", "o", "ak");		//returns "Jake Dake" 
		/// </pre>
		/// </summary>
		/// <param name="searchIn"> </param>
		/// <param name="searchFor"> </param>
		/// <param name="replaceWith"> </param>
		public static string SearchAndReplace(string searchIn, string searchFor, string replaceWith) {
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(searchIn);

			//There is a bug in the replace method of the StringBuilder
			sb.Replace(searchFor, replaceWith);

			//Call the Replace() method of the StringBuilder and specify the string to replace with
			return sb.Replace(searchFor, replaceWith).ToString();
		}

		/// Searches one string into another string and replaces each occurences with
		/// a third string. The fourth parameter specifies the starting occurence and the 
		/// number of times it should be replaced
		/// <pre>
		/// Example:
		/// Strings.StrTran("Joe Doe", "o", "ak", 2, 1);		//returns "Joe Dake" 
		/// </pre>
		public static string SearchAndReplace(string searchIn, string searchFor, string replaceWith, int nStartoccurence, int nCount) {
			//Create the StringBuilder
			StringBuilder sb = new StringBuilder(searchIn);

			//There is a bug in the replace method of the StringBuilder
			sb.Replace(searchFor, replaceWith);

			//Call the Replace() method of the StringBuilder specifying the replace with string, occurence and count
			return sb.Replace(searchFor, replaceWith, nStartoccurence, nCount).ToString();
		}

		/// <summary>
		/// Receives a string (stringValue) as a parameter and replaces a specified number 
		/// of characters in that string (charactersReplaced) from a specified location
		/// (startReplacements) with a specified string (cReplacement)
		/// <pre>
		/// Example:
		/// string stringValue = "Joe Doe";
		/// string lcReplace = "Foo ";
		/// Strings.Stuff(stringValue, 5, 0, lcReplace);	//returns "Joe Foo Doe";
		/// Strings.Stuff(stringValue, 5, 3, lcReplace);	//returns "Joe Foo ";
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="startReplacements"> </param>
		/// <param name="charactersReplaced"> </param>
		/// <param name="cReplacement"> </param>
		public static string Stuff(string stringValue, int startReplacements, int charactersReplaced, string cReplacement) {
			//Create a stringbuilder to work with the string
			StringBuilder sb = new StringBuilder(stringValue);

			if (charactersReplaced + startReplacements - 1 >= stringValue.Length) {
				charactersReplaced = stringValue.Length - startReplacements + 1;
			}

			//First remove the characters specified in nCharacterReplaced
			if (charactersReplaced != 0) {
				sb.Remove(startReplacements - 1, charactersReplaced);
			}

			//Now Add the new string at the right location
			//sb.Insert(0,stringValue,nTimes);
			sb.Insert(startReplacements - 1, cReplacement);
			return sb.ToString();
		}

		/// <summary>
		/// Receives a string as a parameter and returns a part of the string based on the parameters specified.
		/// <pre>
		/// string lcName = "Joe Doe";
		/// SubStr(lcName, 1, 3);	//returns "Joe"
		/// SubStr(lcName, 5);	//returns Doe
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="startPosition"> </param>
		public static string Extract(string stringValue, int startPosition) {
			return stringValue.Substring(startPosition - 1);
		}

		/// <summary>
		/// Overloaded method for SubStr() that receives starting position and length
		/// </summary>
		/// <param name="stringValue"> </param>
		/// <param name="startPosition"> </param>
		/// <param name="length"> </param>
		public static string Extract(string stringValue, int startPosition, int length) {
			startPosition--;
			if ((length + startPosition) > stringValue.Length) {
				return stringValue.Substring(startPosition);
			} else {
				return stringValue.Substring(startPosition, length);
			}
		}

		/// <summary>
		/// Receives a string and converts it to an integer
		/// <pre>
		/// Example:
		/// Strings.Val("1325");	//returns 1325
		/// </pre>
		/// </summary>
		/// <param name="stringValue"> </param>
		public static int ConvertToInt(string stringValue) {
			//Remove all the spaces and commas from the string
			//Get the integer portion of the string
			return Int32.Parse(stringValue, NumberStyles.Any);
		}

		/// <summary>
		/// Receives a string and converts it to an integer
		/// <pre>
		/// Example:
		/// Strings.SearchFromLeftLine("Is", "Is Life Beautiful? \r\n It sure is");	//returns 1
		/// </pre>
		/// </summary>
		/// <param name="tsearchExpression"></param>
		/// <param name="stringValueSearched"></param>
		/// <returns></returns>
		public static int SearchFromLeftLine(string tsearchExpression, string stringValueSearched) {
			string stringValue;
			int position;
			int nCount = 0;

			try {
				position = Strings.SearchFromLeft(tsearchExpression, stringValueSearched);
				if (position > 0) {
					stringValue = Strings.Extract(stringValueSearched, 1, position - 1);
					nCount = Strings.Occurs(@"\r", stringValue) + 1;
				}
			} catch {
				nCount = 0;
			}

			return nCount;
		}

		/// <summary>
		/// Receives a search stringValue and string to search as parameters and returns an integer specifying
		/// the line where it was found. This function starts it search from the end of the string.
		/// <pre>
		/// Example:
		/// Strings.SearchFromRightLine("sure", "Is Life Beautiful? \r\n It sure is") 'returns 2
		/// </pre>
		/// </summary>
		/// <param name="tsearchExpression"></param>
		/// <param name="stringValueSearched"></param>
		/// <returns></returns>
		public static int SearchFromRightLine(string tsearchExpression, string stringValueSearched) {
			string stringValue;
			int position;
			int nCount = 0;

			try {
				position = Strings.SearchFromRight(tsearchExpression, stringValueSearched);
				if (position > 0) {
					stringValue = Strings.Extract(stringValueSearched, 1, position - 1);
					nCount = Strings.Occurs(@"\r", stringValue) + 1;
				}
			} catch {
				nCount = 0;
			}

			return nCount;
		}

		/// <summary>
		/// Returns the line number of the first occurence of a string stringValue within 
		/// another string stringValue without regard to case (upper or lower)
		/// <pre>
		/// Example:
		/// Strings.SearchFromLeftCLine("Is Life Beautiful? \r\n It sure is", "Is");	//returns 1
		/// </pre>
		/// </summary>
		/// <param name="tsearchExpression"></param>
		/// <param name="stringValueSearched"></param>
		/// <returns></returns>
		public static int SearchFromLeftCLine(string tsearchExpression, string stringValueSearched) {
			return SearchFromLeftLine(tsearchExpression.ToLower(), stringValueSearched.ToLower());
		}

		/// <summary>
		/// Receives a string as a parameter and returns a bool indicating if the left most
		/// character in the string is a valid digit.
		/// <pre>
		/// Example:
		/// if(Strings.IsDigit("1Kamal")){...}	//returns true
		/// </pre>
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		public static bool IsDigit(string stringValue) {
			//get the first character in the string
			char c = stringValue[0];
			return Char.IsDigit(c);
		}

		/// <summary>
		/// Returns the number of lines in a string
		/// <pre>
		/// Example:
		/// int lines = Strings.NumberOfLines(lcMyLongString);
		/// </pre>
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		public static int Lines(string stringValue) {
			if (stringValue.Trim().Length == 0) {
				return 0;
			} else {
				return Strings.Occurs("\\r", stringValue) + 1;
			}
		}

		/// <summary>
		/// Receives a string and a line number as parameters and returns the
		/// specified line in that string
		/// <pre>
		/// Example:
		/// string lcCity = Strings.LineInString(tcAddress, 2); // Not that you would want to do something like this but you could ;)
		/// </pre>
		/// </summary>
		/// <param name="stringValue"></param>
		/// <param name="lineNo"></param>
		/// <returns></returns>
		public static string LineInString(string stringValue, int lineNumber) {
			string[] lines = stringValue.Split('\r');
			string returnValue = "";
			try {
				returnValue = lines[lineNumber - 1];
			} catch {
				//Ignore the exception as LineInString always returns a value
			}

			return returnValue;
		}

		public string WordWrap(string origString, int wrapNumber) {
			int startPosition = 0;
			int length = wrapNumber;
			int strLoop = origString.Length/wrapNumber;
			string wrapString = "";
			if (strLoop > 0) {
				for (int i = 0; i < strLoop; i++) {
					if (startPosition > 0) {
						length = wrapNumber;
						if (startPosition + length > origString.Length) {
							length = length - startPosition;
						}
					}
					int j;
					for (j = length; j > 0; j--) {
						if (origString.Substring(startPosition + j, 1).Equals(" ")) {
							break;
						}
					}
					if (startPosition + length < origString.Length) {
						length = j + 1;
					}
					wrapString += Strings.Extract(origString, startPosition, length) + "<br>";
					startPosition = startPosition + length;
				}
				wrapString += origString.Substring(startPosition, origString.Length - startPosition);
			} else {
				wrapString = origString;
			}
			return wrapString;
		}

		public static bool IsEmpty(string value) {
			bool empty = true;
			if (value != null) {
				value = value.Trim();
				if (value.Length > 0) {
					empty = false;
				}
			}
			return empty;
		}

		//Reverse 
		//This will reverse the String argument passed and return it. 
		// Usage:Reverse(Source) 
		//  
		//eg.Reverse("abc") will return "cba" 
		public static String Reverse(String strParam) {
			if (strParam.Length == 1) {
				return strParam;
			} else {
				return Reverse(strParam.Substring(1)) + strParam.Substring(0, 1);
			}
		}

		//IsPalindrome 
		//  
		//This function will return whether the passed string is palindrome or not. 
		//  
		//Usage:IsPalindrome(Source) 
		//  
		//eg.IsPalindrome("abc") will return false wherease IsPalindrome("121") will return true. 
		public static bool IsPalindrome(String strParam) {
			int iLength, iHalfLen;
			iLength = strParam.Length - 1;
			iHalfLen = iLength/2;
			for (int iIndex = 0; iIndex <= iHalfLen; iIndex++) {
				if (strParam.Substring(iIndex, 1) != strParam.Substring(iLength - iIndex, 1)) {
					return false;
				}
			}
			return true;
		}

		//CharCount 
		//  
		//This CharCount will no. of occurrences of a sub string in the main string. This will be useful in parsing functions. 
		// 
		//Usage:CharCount(Source,Find) 
		//  
		//eg.CharCount("aaaaac","a")  will return 5 
		public static int CharCount(String strSource, String strToCount) {
			int iCount = 0;
			int iPos = strSource.IndexOf(strToCount);
			while (iPos != -1) {
				iCount++;
				strSource = strSource.Substring(iPos + 1);
				iPos = strSource.IndexOf(strToCount);
			}
			return iCount;
		}

		//Function to count no.of occurences of Substring in Main string
		public static int CharCount(String strSource, String strToCount, bool IgnoreCase) {
			if (IgnoreCase) {
				return CharCount(strSource.ToLower(), strToCount.ToLower());
			} else {
				return CharCount(strSource, strToCount);
			}
		}

		//ToSingleSpace 
		//
		//ToSingleSpace is a function which will trims off multiple whitespace characters to single whitespace characters.  
		//  
		//Usage:ToSingleSpace(SourceString)  
		//eg.ToSingleSpace("Welcome               to                 C#") will return "Welcome to C#" 
		//  
		public static String ToSingleSpace(String strParam) {
			int iPosition = strParam.IndexOf("  ");
			if (iPosition == -1) {
				return strParam;
			} else {
				return ToSingleSpace(strParam.Substring(0, iPosition) + strParam.Substring(iPosition + 1));
			}
		}

		///<summary>
		///<returns>(int)</returns>
		///<param name="stringOpA">first input string(string)</param>
		///<param name="stringOpB">second input string(string)</param>
		///<param name="lenToCompare">length of chars to compare(int)</param>
		///</summary>
		public int StringINCompare(string stringOpA, string stringOpB, int lenToCompare) {
			char[] a1 = stringOpA.ToCharArray();
			char[] b1 = stringOpB.ToCharArray();

			if (lenToCompare == 0) {
				return 0;
			}
			if (lenToCompare > stringOpA.Length && lenToCompare > stringOpB.Length) {
				return -1;
			} //throw an exception
			if (lenToCompare > stringOpA.Length) {
				return 1;
			}
			if (lenToCompare > stringOpB.Length) {
				return -1;
			}

			for (int ii = 0; ii < lenToCompare; ii++) {
				if (a1[ii] > b1[ii]) {
					return 1;
				}
				if (a1[ii] < b1[ii]) {
					return -1;
				}
			}
			return 0;
		}

	}
}