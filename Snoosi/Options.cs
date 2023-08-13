using CommandLine;

namespace Snoosi;


[Verb("start", HelpText="start a new snoosi task")]
public class StartOptions
{
    [Value(0, MetaName = "name", Required = true, HelpText = "name of task to start")]
    public string TaskName { get; set; }
    
    [Value(1, MetaName = "time", Required = true, HelpText = "time to start task at")]
    public string StartTime { get; set; }

    [Option("date", Separator = '=', HelpText = "date to start task at")]
    public string StartDate { get; set; }
    
    [Option("rep", Separator = '=', HelpText = "minutes between repeats (dont use for no repeat)")]
    public int MinutesRepeatAfter { get; set; }
    
    // [Option("user", Separator = '=', HelpText = "NIY")]
    // public string Username { get; set; }
}

[Verb("addtask", HelpText="create a new snoosi task")]
public class AddTaskOptions
{
    [Option("name", Required = true, Separator = '=', HelpText = "name of task to run")]
    public string? TaskName { get; set; }
    
    [Option("cmd", Required = true, Separator = '=', HelpText = "cmd to run")]
    public string? TaskCommand { get; set; }
    
    [Option("params", Separator = '=', HelpText = "parameters for the task")]
    public string TaskCommandParams { get; set; }

    [Option("offset", Separator = '=', HelpText = "i dont know, NIY")]
    public int Offset { get; set; }
}

[Verb("stop", HelpText="stop snoosi task")]
public class StopOptions
{
    // [Option("user", Separator = '=', HelpText = "")]
    // public string Username { get; set; }
}

// [Verb("configure", HelpText="configure snoosi options")]
// public class ConfigOptions
// {
//
//     [Option("u", Separator = '=', HelpText = "")]
//     public string Username { get; set; }
// }

[Verb("reset", HelpText="reset snoosi options, delete configs")]
public class ResetOptions
{
}