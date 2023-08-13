using CommandLine;


namespace Snoosi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var currentUser = User.Load("default");
            if (!currentUser.HasValue)
            {
                Console.WriteLine("Current user is null");
                return;
            }
            SnoosiTask.PerformCleanup();
            Parser.Default.ParseArguments<StartOptions, StopOptions, ResetOptions, AddTaskOptions>(args)
                .WithParsed<StartOptions>(options =>
                {
                    currentUser.Value.Tasks.ForEach(t =>
                        {
                            if (t.Name == options.TaskName)
                            {
                                Console.WriteLine($"name={options.TaskName}, time={options.StartTime}, date={options.StartDate}");
                                t.Publish(currentUser.Value, options.StartTime, options.StartDate);
                            }
                            else
                            {
                                Console.WriteLine("No such task found. Are you on the right user?");
                            }
                        });
                })
                .WithParsed<AddTaskOptions>(options =>
                {
                    if (string.IsNullOrEmpty(options.TaskName) || string.IsNullOrEmpty(options.TaskCommand))
                    {
                        Console.WriteLine("Invalid task name or cmd");
                        return;
                    }
                    currentUser.Value.AddTask(SnoosiTask.Create(currentUser.Value, options.TaskName, options.TaskCommand, options.TaskCommandParams));
                })
                .WithParsed<StopOptions>(options => { })
                // .WithParsed<ConfigOptions>(options => { })
                .WithParsed<ResetOptions>(options =>
                {
                    var saveFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\snoosi";
                    var saveFilePath = $"{saveFolderPath}\\{currentUser.Value.Name}.json";
                    var folderExists = Directory.Exists(saveFolderPath);
                    var fileExists = File.Exists(saveFilePath);
                    Console.WriteLine("This operation will remove all user settings and tasks. Are you sure? (Y/N)");
                    var input = Console.ReadKey();
                    if (input.Key != ConsoleKey.Y)
                    {
                        return;
                    }
                    if (folderExists)
                    {
                        try
                        {
                            if (fileExists)
                            {
                                File.Delete(saveFilePath);
                            }
                            Directory.Delete(saveFolderPath);
                            Console.WriteLine("Successfully cleaned config files and tasks. The application can be cleanly removed.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Failed to clean config files!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Configs were not found, skipping.");
                    }
                })
                .WithNotParsed(errors => { });
        }
    }
}