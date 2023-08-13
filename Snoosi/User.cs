using Newtonsoft.Json;

namespace Snoosi;

public struct User
{
    public string Name { get; set; } = "default";
    public List<SnoosiTask> Tasks { get; set; } = new();
    public void AddTask(SnoosiTask task)
    {
        Tasks.Add(task);
    }

    public static User? Load(string name)
    {
        var saveFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\snoosi";
        var saveFilePath = $"{saveFolderPath}\\{name}.json";
        var folderExists = Directory.Exists(saveFolderPath);
        var fileExists = File.Exists(saveFilePath);
        var content = "";
        if (!folderExists)
        {
            try
            {
                Directory.CreateDirectory(saveFolderPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("FATAL: Failed to create config directory");
            }
        }
        if (!fileExists)
        {
            try
            {
                var s = File.Create(saveFilePath);
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("FATAL: Failed to create config file");
            }
        }
        try
        {
            content = File.ReadAllText(saveFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Failed to read config file at {saveFilePath}");
        }

        if (fileExists)
        {
            try
            {
                var usr = JsonConvert.DeserializeObject<User?>(content);
                return usr;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Failed to read config from {saveFilePath}");
            }
        }
        // first time or config reset, create new default user
        return new User();
    }

    public void Save()
    {
        var saveFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\snoosi";
        var saveFilePath = $"{saveFolderPath}\\{Name}.json";
        var folderExists = Directory.Exists(saveFolderPath);
        var fileExists = File.Exists(saveFilePath);
        if (!folderExists)
        {
            try
            {
                Directory.CreateDirectory(saveFolderPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("FATAL: Failed to create config directory");
            }
        }
        if (!fileExists)
        {
            try
            {
                var s = File.Create(saveFilePath);
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("FATAL: Failed to create config file");
            }
        }

        try
        {
            var usr = JsonConvert.SerializeObject(this);
            File.WriteAllText(saveFilePath, usr);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Failed to write config into {saveFilePath}");
        }
    }
    
    
    
    public User()
    {
    }
}