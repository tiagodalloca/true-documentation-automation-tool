using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace true_documentation_automation_tool
{
	class Program
	{
		private const string CONFIG_FILE_NAME = "true-dat";

		static void Main(string[] args)
		{
			try
			{
				MainAsync(args).Wait();
			}
			catch (Exception e)
			{
				HandleTopLevelException(e);
			}
		}

		private static async Task MainAsync(string[] args)
		{
			var root = args[0];
			var configFileLocation = Path.Combine(root, CONFIG_FILE_NAME);

			if (!File.Exists(configFileLocation))
				throw new Exception($"config file ${configFileLocation} missing.");

			var filesNames = File.ReadAllLines(configFileLocation).Select(x => Path.Combine(root, x.Trim()));

			var processingTasks = filesNames.Select(async fileName =>
			{
				string generatedContent = await FileProcessorUtils.ProcessFile(fileName);
				var pathToSave = Path.Combine(new FileInfo(fileName).Directory.FullName, Path.GetFileNameWithoutExtension(fileName) + ".md");
				File.WriteAllText(pathToSave, generatedContent);
			});

			await Task.WhenAll(processingTasks.ToArray());
		}

		private static void HandleTopLevelException(Exception e)
		{
			Console.WriteLine();
			Console.WriteLine($"Erro: {e.Message}");
			Console.WriteLine();
		}
	}
}
