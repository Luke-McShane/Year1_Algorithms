//Imports various libraries for the application to use
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

//Creat a class that will house the application
class mainBody
{
	//Create an empty dictionary.
	//This dictionary will store all the information from each and every file that is being read. This is potentially the most important variable in the program
	private static Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
	//This array is used to quickly access data from the main dictionary.
	private static string[]  fileNamesNum = {"Month", "WS1_AF", "WS1_Rain", "WS1_Sun", "WS1_TMax", "WS1_TMin", "WS2_AF", "WS2_Rain", "WS2_Sun", "WS2_TMax", "WS2_TMin", "Year"};
	//This array is used to correctly name each Key in the main 'dictionary'.
	private static string[] fileNamesNumList = {"Month_List", "WS1_AF_List", "WS1_Rain_List", "WS1_Sun_List", "WS1_TMax_List", "WS1_TMin_List", "WS2_AF_List", "WS2_Rain_List", "WS2_Sun_List", "WS2_TMax_List", "WS2_TMin_List", "year_List"};
	//This line of code runs the method that reads files and stores data to the appropriate Keys and Values in 'dictionary'.
	private static Dictionary<string, List<string>> userDictionary = createArrays();
	private static int count = 0;
	//This method is automatically run when the program is run.
	public static void Main()
	{
	//Presents an interface/menu system to the user.
	Console.WriteLine("Would you like to sort an array, search according to the year, search according to the month, or exit the program? Please enter the corresponding number: \n1.  Sort array \n2.  Search according to year \n3.  Search according to month \n4.  Exit");
	try
	{
		//This 'try' statements checks whether the user has entered an integer or not. This is used so we can inform the user of the specific error they have made.
		int userInputMain = Convert.ToInt32(Console.ReadLine());
		//Checks if the input is within a suitable range.
		if(Enumerable.Range(1,4).Contains(userInputMain))
		{
			//If the input is within the correct range, a method is called dependent on the valid input.
			if(userInputMain==1)
				Analyse();
			else if(userInputMain==2)
				yearSearch();
			else if(userInputMain==3)
				monthSearch();
			else
				System.Environment.Exit(1);
		}
		else
		{
			//The 'Main' method is called again if an invalid input is entered.
			Console.WriteLine("Please enter a number within the range: 1-4");
			Main();
		}
	}
	//The error is caught and presented to the user.
	catch(Exception ex)
	{
		Console.WriteLine("{0}\n" , ex);
		Console.WriteLine("Please just enter the number that corresponds to the Array. For example, typing \"1\" would indicate that you would like to analyse the \"Month\" array \n");
		Main();
	}
	
	}
	
	public static void Analyse()
	{
		//Presents the user with an interface and then saves their input as a string variable.
		Console.WriteLine("What Array would you like to be analysed and sorted? Please enter the corresponding number: \n1.  WS1_AF \n2.  WS1_Rain \n3.  WS1_Sun \n4.  WS1_TMax \n5.  WS1_TMin \n6.  WS2_AF \n7.  WS2_Rain \n8.  WS2_Sun \n9.  WS2_TMax \n10. WS2_TMin");
		string userInput = Console.ReadLine();
		try
		{
			//Attemts to convert the user input to an integer, for future yse.
			int userInputInt = Convert.ToInt32(userInput);
			//Selects the relvent array in the dictionary based on the input.
			List<string> selecetedArray = userDictionary[fileNamesNumList[userInputInt]];
			//Ensures that the selected data type is an array.
			string[] userArray = selecetedArray.ToArray();
			//Converts all string values in the selected array into the 'double' data type to account for decimal values.
			double[] userIntArray = Array.ConvertAll(userArray, double.Parse);
			//Calls the sorting algorithm and passes through relevant data.
			double[] sortedArray = sortAlgorithm(userIntArray, 0, userIntArray.Length-1);
			//This line of code prints the total amount of swaps needed to sort the array.
			Console.WriteLine("Total number of swaps: {0}", count);
			//This line of code locates and prints the maximum, minimum and median values within the sorted array.
			Console.WriteLine("{0}: Max: {1}   Min: {2}   Median: {3}", fileNamesNumList[userInputInt], sortedArray[sortedArray.Length-1], sortedArray[0], sortedArray[(sortedArray.Length)/2]);
			//After the array has been sorted, a method is called to correctly print the array in the desired manner.
			printSortedArray(sortedArray);
		}
		
		catch(Exception ex)
		{
			Console.WriteLine("{0}\n" , ex);
			Console.WriteLine("Please just enter the number that corresponds to the Array. For example, typing \"1\" would indicate that you would like to analyse the \"Month\" array \n");
			Analyse();
		}
	}
	
	public static void printSortedArray(double[] sorted)
	{	
		//Asks the user how they would like the sorted array to be printed. If the user enters an invalid input, they are requested input again.
		Console.WriteLine("Would you like to sorted array to be printed in ascending or descending order? Please enter \"1\" or \"2\" respectively");
		string userAorD = Console.ReadLine();
		
		
		if(userAorD == "1")
		{
			foreach(double element in sorted)
				Console.Write(" {0}", element);
				Console.WriteLine("\n");
			Main();
		}
		else if(userAorD == "2")
		{
			//This method reverses the order of the array, thus allowing it to be printed in descending order.
			Array.Reverse(sorted);
			foreach(double element in sorted)
				Console.Write(" {0}", element);	
			Console.WriteLine("\n");
			Main();
		}
		else
			printSortedArray(sorted);
	}
	
	public static Dictionary<string, List<string>> createArrays()
	{	
		//Creates a temporary variable that will increment each iteration of the foreach loop.
		int temp = 0;
		//This loop iterates each file name, thus allowing the streamreader to correctly read the files from the folder.
		foreach (string name in fileNamesNum)
		{
			//Creates a temporary variable that is the location of the current file being accessed.
			string path = "CMP1124M_Weather_Data/" + name + ".txt";
			//Creates an instance of streamreader to read data from the selected file.
			using (StreamReader sr = new StreamReader(path))
				{
					//Checks if there is still data to be read or not. If so, then the while loop will execute.
					while (sr.Peek() >= 0)
					{
						//Checks if the title of the file, the key, is already present in the dictionary.
						if(dictionary.ContainsKey(fileNamesNumList[temp]))
							//Adds the current line being read from the file to the list (which is the value in the dictionary).
							dictionary[fileNamesNumList[temp]].Add(sr.ReadLine());
						else
						{
							//If the key is not present, then the name of the file is added as the key.
							dictionary.Add(fileNamesNumList[temp], new List<string>());
							dictionary[fileNamesNumList[temp]].Add(sr.ReadLine());
						}
					}
				}
			temp +=1;
		}
		//Returns the dictionary with all the data successfully added.
		return dictionary;
	}
	
	public static double[] sortAlgorithm(double[] sortMe, int left, int right)
	{
		
		//This criteria will remain true until the array is sorted. A while loop could have been used as an alternative here.
		if(left < right)
		{
			//Calls a separate method partitions current section of the array and also returns the partition value (where the array is currently being partitioned).
			int partitionValue = partition(sortMe, left, right);
			//Sorts the left-hand side of the current partition, this is obviously recursive as the method is being called within itself/the method is defined within itself.
			sortAlgorithm(sortMe, left, partitionValue - 1);
			//sorts the right-hand side of the current partition recursively.
			sortAlgorithm(sortMe, partitionValue + 1, right);
		}
		//Returns the sorted array.
		
		return sortMe;
	}
	
	public static int partition(double[] tempArray, int left, int right)
	{
		//Selects the pivot value as the rightmost value in the array.
		//Automatically selected as a double since the method could be dealing with a decimal value.
		//If an int data type was used, an error would be encountered if the array contained a decimal(s).
		double pivot = tempArray[right];
		double temp;
		
		//Creates a variable identical to left that will iterate throughout the for loop.
		int i = left;
		//This for loop iterates equal to the length of the sub-array being sorted.
		for(int j = left; j < right; j++)
		{
			//This sections checks values against one another and swaps them (using the temp variable defined earlier) if they are in the incorrect order.
			if(tempArray[j] <= pivot)
			{
				temp = tempArray[j];
				tempArray[j] = tempArray[i];
				tempArray[i] = temp;
				i++;
				count++;
			}
		}
		tempArray[right] = tempArray[i];
		tempArray[i] = pivot;
		//Returns the partition value to be used in the recursive part of the quicksort algorithm.
		return i;
	}
	
	public static void yearSearch()
	{
		Console.WriteLine("Please enter the year you would like to search for, if you would like to exit to the main menu, please type \"exit\"");
		string yearSearchInput = Console.ReadLine();
		//Gives the user to exit this part of the algorith as the user is asked for an input after the method outputs the data.
		if(yearSearchInput.ToUpper() == "EXIT")
			Main();
		try
		{
			//Checks if "year_List" list in the dictionary contain the user's year input
			if(userDictionary["year_List"].Contains(yearSearchInput))
			{
				//Creates an empty list that will house the indices of the year they entered.
				//This is vital as this information will be used to index data within other files.
				List<int> yearIndexes = new List<int>();
				while (true)
				{
					//If the index is empty, then we can add the index of the first encounter of the proposed year.
					if (yearIndexes.Count == 0)
						yearIndexes.Add(userDictionary["year_List"].IndexOf(yearSearchInput));
					else
					{
						//This criteria is met if the program has already encountered the year the user proposed.
						//In this case, the program skips all the indices that have already been added to the list.
						//If there are no more occurences of the year in the list, then th loop will break and the list has been completed.
						//If there are more occurences, then they next occurence will be located and its index added to the list.
						if(userDictionary["year_List"].IndexOf(yearSearchInput, (userDictionary["year_List"].IndexOf(yearSearchInput)+yearIndexes.Count)) != -1)
							yearIndexes.Add(userDictionary["year_List"].IndexOf(yearSearchInput,(userDictionary["year_List"].IndexOf(yearSearchInput)+yearIndexes.Count)));				
						else
							break;
					}
				}
				//This code prints relevant information from all other files depending on the year the user searched for.
				foreach(int listYearIndex in yearIndexes)
				{
					//Console.WriteLine("Month: {0}  WS1_AF: {1}  WS1_Rain: {2}  WS1_Sun: {3}  WS1_TMax: {4}  WS1_TMin: {5}  WS2_AF: {6}  WS2_Rain: {7}  WS2_Sun: {8}  WS2_TMax: {9}  WS2_TMin: {10}  Year: {11}", dictionary["Month_List"][listIndex], dictionary["WS1_AF_List"][listIndex], dictionary["WS1_Rain_List"][listIndex], dictionary["WS1_Sun_List"][listIndex], dictionary["WS1_TMax_List"][listIndex], dictionary["WS1_TMin_List"][listIndex], dictionary["WS2_AF_List"][listIndex], dictionary["WS2_Rain_List"][listIndex], dictionary["WS2_Sun_List"][listIndex], dictionary["WS2_TMax_List"][listIndex], dictionary["WS2_TMin_List"][listIndex], dictionary["year_List"][listIndex]);
					Console.WriteLine("Month: {0}  WS1_AF: {1}  WS1_Rain: {2}  WS1_Sun: {3}  WS1_TMax: {4}  WS1_TMin: {5}  Year: {6}" , dictionary["Month_List"][listYearIndex], dictionary["WS1_AF_List"][listYearIndex], dictionary["WS1_Rain_List"][listYearIndex], dictionary["WS1_Sun_List"][listYearIndex], dictionary["WS1_TMax_List"][listYearIndex], dictionary["WS1_TMin_List"][listYearIndex], dictionary["year_List"][listYearIndex]);
					Console.WriteLine("\n");
					
				}
				Console.WriteLine("\n");
				
				
			}
			else
			{
				Console.WriteLine("Please enter a valid year input");
				yearSearch();
			}
			Main();
		}
		
		catch(Exception ex)
		{
			Console.WriteLine("{0}", ex);
			Console.WriteLine("Please enter integers only");
			yearSearch();
		}	
	}
	
	public static void monthSearch()
	{
		Console.WriteLine("Please enter the month you would like to search for, if you would like to exit to the main menu, please type \"exit\"");
		string monthSearchInput = Console.ReadLine();
		if(monthSearchInput.ToUpper() == "EXIT")
			Main();
		try
		{
			if(userDictionary["Month_List"].Contains(monthSearchInput))
			{
				List<int> monthIndexes = new List<int>();
				while (true)
				{
					if (monthIndexes.Count == 0)
						monthIndexes.Add(userDictionary["Month_List"].IndexOf(monthSearchInput));
					else
					{
						//This code contains the only major difference from yearSearch().
						//Here we skip 12 values in the array since the month being searched for will only occur once in every 12 iterations due to the amount of months in a year.
						//Again, we check that there is at least one more occurence present or else we would get an error. Checking ahead of time ensures we do not encounter this error.
						//Console.WriteLine("{0}  {1}    {2}", userDictionary["Month_List"].Count, userDictionary["Month_List"].IndexOf(monthSearchInput,((12*monthIndexes.Count)-12)), ((userDictionary["Month_List"].Count)-(userDictionary["Month_List"].IndexOf(monthSearchInput,((12*monthIndexes.Count)-12)))));
						if(((userDictionary["Month_List"].Count)-(userDictionary["Month_List"].IndexOf(monthSearchInput,((12*monthIndexes.Count)-12)))) >12)
							monthIndexes.Add(userDictionary["Month_List"].IndexOf(monthSearchInput,((12*monthIndexes.Count))));				
						else
							break;
					}
				}
				foreach(int listMonthIndex in monthIndexes)
				{
					//Console.WriteLine("Month: {0}  WS1_AF: {1}  WS1_Rain: {2}  WS1_Sun: {3}  WS1_TMax: {4}  WS1_TMin: {5}  WS2_AF: {6}  WS2_Rain: {7}  WS2_Sun: {8}  WS2_TMax: {9}  WS2_TMin: {10}  Year: {11}", dictionary["Month_List"][listIndex], dictionary["WS1_AF_List"][listIndex], dictionary["WS1_Rain_List"][listIndex], dictionary["WS1_Sun_List"][listIndex], dictionary["WS1_TMax_List"][listIndex], dictionary["WS1_TMin_List"][listIndex], dictionary["WS2_AF_List"][listIndex], dictionary["WS2_Rain_List"][listIndex], dictionary["WS2_Sun_List"][listIndex], dictionary["WS2_TMax_List"][listIndex], dictionary["WS2_TMin_List"][listIndex], dictionary["year_List"][listIndex]);
					Console.WriteLine("Month: {0}  WS1_AF: {1}  WS1_Rain: {2}  WS1_Sun: {3}  WS1_TMax: {4}  WS1_TMin: {5}  Year: {6}" , dictionary["Month_List"][listMonthIndex], dictionary["WS1_AF_List"][listMonthIndex], dictionary["WS1_Rain_List"][listMonthIndex], dictionary["WS1_Sun_List"][listMonthIndex], dictionary["WS1_TMax_List"][listMonthIndex], dictionary["WS1_TMin_List"][listMonthIndex], dictionary["year_List"][listMonthIndex]);
					Console.WriteLine("\n");
				}
				Console.WriteLine("\n");
			}
			else
			{
				Console.WriteLine("Please enter a valid month input");
				monthSearch();
			}
			monthSearch();
		}
		
		catch(Exception ex)
		{
			Console.WriteLine("{0}", ex);
			Console.WriteLine("Please enter integers only");
			monthSearch();
		}	
	}
}

