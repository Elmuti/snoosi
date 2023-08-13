## Snoosi

Simple CLI interface for creating simple tasks for windows task scheduler

### Example usage

- Create a new task: ``snoosi addtask --name=alarm --cmd="C:/Programs/vlc/vlc.exe" --params="C:/Music/alarm.mp3"``


- Run the task: ``snoosi start alarm 05:00`` ([All .NET time/date formats](https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings) apply here
, for example ``2009-06-15T13:45:30``)

Windows will now play "alarm.mp3" with VLC Media player at 05:00.

If no date is specified, defaults to current day, unless that time already occured, in which case it is ran the next day.

- Cancel the task: ``snoosi stop alarm``


### All commands

``start``      Start a snoosi task

``stop``       Stop a snoosi task

``reset``      Reset snoosi options, delete configs

``addtask``    Create a new snoosi task

``help``       Display help

``version``    Display version information.

All available options for the commands can be found through ``snoosi <command> --help`` 