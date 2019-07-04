using System;
using System.Collections.Generic;
using System.IO;

namespace Regex
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start");
			var path = "C:\\_code\\istart\\Database\\Procedures";
			var searchWord = "RAISERROR";
			string[] files = Directory.GetFiles(path);
			var filesWithSearchWord = new List<string>();

			foreach(string fileName in files)
			{
				string contents = File.ReadAllText(fileName);
				if (contents.Contains(searchWord))
				{
					filesWithSearchWord.Add(fileName);
				}
			}

			foreach ( var file in filesWithSearchWord)
			{	
				ReadEachLineFromFile(file);
			}
		}

		private static void ReadEachLineFromFile(string file)
		{
			string line;
			try
			{
				//Pass the file path and file name to the StreamReader constructor
				StreamReader sr = new StreamReader(file);
				int lineNumber = 1;
				//Read the first line of text
				line = sr.ReadLine();
				lineNumber++;

				//Continue to read until you reach end of file
				while (line != null)
				{
					//Read the next line
					line = sr.ReadLine();
					if (line.Contains("RAISERROR"))
					{
						//RAISERROR  20002 'prc_EmailMessageDelete: Cannot delete because foreign keys still exist in RECORDED_STATISTIC '

						//RAISERROR('prc_EmailMessageDelete: Cannot delete because foreign keys still exist in RECORDED_STATISTIC ', 20002, 1)
						var parsedWords = MySplit(line);
						sr.Close();
						var newLine = GetNewLine(parsedWords);
						lineChanger(newLine, file, lineNumber);
						Console.WriteLine(line);
						return;
					}
					lineNumber++;
				}

				//close the file
				sr.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
			finally
			{
				Console.WriteLine("Executing finally block.");
			};
		}

		private static string GetNewLine(List<string> parsedWords)
		{
			var newLine = "		RAISERROR(" + parsedWords[1] + "," + parsedWords[0] + "," + 1 + ")";
			return newLine;
		}

		private static List<string> MySplit(string line)
		{
			var words = new List<string>();
			var word = "";
			for (int i = 0; i < line.Length; i++)
			{
				if (line[i] == '2')
				{
					word += line[i];
					i++;
					while(line[i]!=' ')
					{
						word += line[i];
						i++;
					}
					words.Add(word);
					word = "";
					
				}

				if (line[i] == '\'')
				{
					word += line[i];
					i++;
					while (line[i] != '\'')
					{
						word += line[i];
						i++;
					}
					word += line[i];
					words.Add(word);

				}

			}

			return words;
		}
		private static void lineChanger(string newText, string fileName, int line_to_edit)
		{
			string[] arrLine = File.ReadAllLines(fileName);
			arrLine[line_to_edit - 1] = newText;
			File.WriteAllLines(fileName, arrLine);
		}
	}
}
