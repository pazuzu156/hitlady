# Hitlady

Hitlady is a Lastfm bot with music capabilities and some simple general purpose stuff.

## Install

There's a few things you need to do for installing and running Hitlady.

### Requirements

* [.NET Core 5 SDK]()
* MySQL server [XAMPP]() if you're on Windows

### Setup

To begin setting up, start your MySQL server, and import `tables.sql`. Once you do this, rename `config.yml.example` to `config.yml`. Add the required data into this config file.

### Running

Just run the `run.bat` file. As long as this is open, your bot is on.

## Extras

### Run in Background

Want to run the bot in the background? Try tmux

#### Linux

Install tmux on your system using it's package manager. Once you do that, run `tmux new -s bot`. This will create a new tmux instance named `bot`. Once in the instance, run `dotnet run` to run the bot. Use the keybind `Ctrl+B D` to detatch from the tmux instance.

When you want to return to the instance, run `tmux -a -t bot` to return to the bot instance.

#### Mac

Refer to [Linux](#linux). Same concept applies here.

#### Windows

First, install [msys2](). If you wish to use Windows Subsystem for Linux, you can do that to. After you do that, refer to [Linux](#linux).

## Contributing

Hitlady is built with .NET Core 5 using Visual Studio Code. You can use any editor you wish, but you need .NET Core 5. Other contributions include language packs (not inplemented yet), artwork, ideas/suggestions. Feel free to contribute to the project!
