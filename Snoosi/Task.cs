using System.Dynamic;
using System.Text.RegularExpressions;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;

namespace Snoosi;

public struct SnoosiTask
{
    public string Name { get; set; } = "SnoosiTask";
    private string Command { get; set; } = "";
    private string Arguments { get; set; } = "";
    private DateTime Date { get; set; } = new();


    public static void PerformCleanup()
    {
        // find snoosi tasks that have already run. Remove them.
        using var ts = new TaskService();
        try
        {
            var snoosiTasks = ts.FindAllTasks(new Regex("^SnoosiTask"), false);
            if (snoosiTasks != null)
            {
                snoosiTasks.ToList().ForEach(t =>
                {
                    if (t.NextRunTime < DateTime.Now && t.LastRunTime < DateTime.Now) //run has already happened
                    {
                        ts.RootFolder.DeleteTask(t.Name);
                    }
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to clean up tasks");
        }
    }


    private static (DateTime, bool) DateTimeFromString(User user, string time, string? date)
    {
        var parsedDate = DateTime.Now;
        date ??= "";
        
        try
        {
            var validTime = DateTime.TryParse(time.ToCharArray(), out var dTime);
            if (!validTime)
            {
                Console.WriteLine("Invalid time format, see https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings");
                return (parsedDate, false);
            }
            
            var validDate = DateTime.TryParse(date.ToCharArray(), out var dDate);
            if (!validDate)
            {
                var today = DateTime.Today;
                //if HHMM has already passed today, set date as tomorrow
                if (dTime.TimeOfDay < DateTime.Now.TimeOfDay)
                {
                    dDate = DateTime.Today.Add(new TimeSpan(1, 0, 0, 0));
                }
                else
                {   //else set as today
                    dDate = DateTime.Today;
                }
            }
            parsedDate = new DateTime(dDate.Year, dDate.Month, dDate.Day, dTime.Hour, dTime.Minute, dTime.Second);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("DateTimeFromString Null reference exception. Please report this!");
            return (parsedDate, false);
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("Must specify a time (date is optional)");
            return (parsedDate, false);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid time format, see https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings");
            return (parsedDate, false);
        }
        return (parsedDate, true);
    }


    public void Publish(User user, string time, string? date)
    {
        var (dt, success) = DateTimeFromString(user, time, date);

        if (success)
        {
            try
            {
                using var ts = new TaskService();
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "Snoosi Task";
                var trigger = new TimeTrigger();
                trigger.StartBoundary = dt;
                //trigger.Repetition = new RepetitionPattern();
                td.Triggers.Add(trigger);
                td.Actions.Add(Command, Arguments);
                ts.RootFolder.RegisterTaskDefinition($"SnoosiTask-{dt.ToString()}", td);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to start task. TimeTrigger was null! Why???? Please report this");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to start task. Please report this!");
            }
        }
        else
        {
            Console.WriteLine("Failed to start task");
        }
    }


    public static SnoosiTask Create(User user, string name, string cmd, string args)
    {
        var task = new SnoosiTask(cmd, args, new DateTime())
        {
            Name = name
        };
        user.AddTask(task);
        user.Save();
        return task;
    }
    
    
    public SnoosiTask(string cmd, string args, DateTime date)
    {
        Command = cmd;
        Arguments = args;
        Date = date;
    }
}