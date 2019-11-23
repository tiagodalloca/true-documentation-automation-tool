using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace true_documentation_automation_tool
{
	public static class FileProcessorUtils
	{
		public static async Task<string> ProcessFile(string fileName)
		{
			var sb = new StringBuilder();

			using (var reader = File.OpenText(fileName))
			{
				var isCommentLine = false;
				var trueDatOn = false;
				var nextLineIsNewBlockCode = false;
				var previousLineWasACodeLine = false;
				var whiteSpacesCount = 0;

				while (!reader.EndOfStream)
				{
					var line = await reader.ReadLineAsync();

					var trimmedLine = line.TrimStart();

					if (trimmedLine.StartsWith("// true dat on"))
					{
						trueDatOn = true;
						nextLineIsNewBlockCode = true;
						isCommentLine = false; 
						for (whiteSpacesCount = 0; char.IsWhiteSpace(line[whiteSpacesCount]); whiteSpacesCount++) { }
						//whiteSpacesCount = whiteSpacesCount == 0 ? whiteSpacesCount : whiteSpacesCount - 1;
						continue;
					}
					if (trimmedLine.StartsWith("// true dat off"))
					{
						if (trueDatOn && previousLineWasACodeLine) sb.AppendLine("```");
						trueDatOn = false;
						continue;
					}

					if (trimmedLine.StartsWith("/*"))
					{
						isCommentLine = true;
						nextLineIsNewBlockCode = false;

						if (trueDatOn && previousLineWasACodeLine) sb.AppendLine("```");

						continue;
					}
					else if (isCommentLine && trimmedLine.StartsWith("*/"))
					{
						isCommentLine = false;
						nextLineIsNewBlockCode = true;
						continue;
					}

					if (trueDatOn)
					{
						if (isCommentLine)
						{
							sb.AppendLine(trimmedLine);

							previousLineWasACodeLine = false;
						}
						else
						{
							if (nextLineIsNewBlockCode) sb.AppendLine("```csharp");
							sb.AppendLine(line.Substring(whiteSpacesCount > line.Length ? 0 : whiteSpacesCount));

							previousLineWasACodeLine = true;
							nextLineIsNewBlockCode = false;
						}
					}
				}
			}

			return sb.ToString();
		}
	}
}
