using System;
using System.IO;
using System.Linq;

namespace true_documentation_automation_tool
{
	class Program
	{
		private const string CONFIG_FILE_NAME = "true-dat";

		static void Main(string[] args)
		{
			try
			{
				var root = args[0];
				var configFileLocation = Path.Combine(root, CONFIG_FILE_NAME);
				
				if (!File.Exists(configFileLocation))
					throw new Exception($"config file ${configFileLocation} missing.");

				var filesNames = File.ReadAllLines(configFileLocation).Select(x => x.Trim());

				foreach (var fileName in filesNames)
				{
					string generatedContent = FileProcessorUtils.ProcessFile(fileName);
					File.WriteAllText(Path.Combine(root, Path.GetExtension(fileName) + ".md"), generatedContent);
				}
			}
			catch (Exception e)
			{
				HandleTopLevelException(e);
			}
		}

		private static void HandleTopLevelException(Exception e)
		{
			Console.WriteLine();
			Console.WriteLine($"Erro: {e.Message}");
			Console.WriteLine();
		}
	}
}
