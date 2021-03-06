# About

This project is an in-progress effort to implement a .NET Standard class library that makes it trivial to write .NET console applications without having to implement argument parsing logic. It provides an API to automatically parse command line arguments and route them to a method to be executed.

# Status

The code is under active development and not ready for production use.

# Design

The argument routing logic is in the `\src\commandline` class library. This project is a NuGet package that can be downloaded from MyGet.

[![MyGet](https://img.shields.io/myget/amervitz/v/amervitz.commandline.svg?label=MyGet)](https://www.myget.org/feed/amervitz/package/nuget/amervitz.commandline)

* `\src\commandline\Router.cs` contains multiple `Invoke` methods to locate and execute commands.

A sample console application that uses the routing logic to execute commands is in the `\src\samples\app` project.

* `\src\samples\app\Program.cs` contains the entry point for taking the command line arguments and using the `\src\commandline` class library to execute a command that is a static method contained in a specific class or to be found within a namespace.

Unit tests are  in the `\src\commandline.tests` project. The tests are another good way of seeing how to use the routing API.

# Class Routing (Commands)
Given an `App` console application, containing a `Calculator` class with an `Add` method that adds two numbers:

```c#
public static class Calculator
{
    public static int Add(int first, int second)
    {
        return first + second;
    }
}
```

The `Add` method, and any other methods within the `Calculator` class, are made available within the `App` console application using the following code:

```c#
class Program
{
    static void Main(string[] args)
    {
        var output = Router.Invoke(typeof(Calculator), args);
    }
}
```

Running the `add` *command* from the command line is then possible using any of these command line argument combinations:

* `app.exe add --first 1 --second 2`
* `app.exe add --second 2 --first 1`
* `app.exe add --first 1 2`
* `app.exe add --second 2 1`
* `app.exe add 1 --second 2`
* `app.exe add 1 2`

# Namespace Routing (Sub Commands)

Given an `App` console application, containing an `App.Commands` namespace with multiple classes within it:

```c#
namespace App.Commands
{
    public static class Calculator
    {
        public static void Add (int first, int second)
    }
}

namespace App.Commands.Blog
{
    public static class Comment
    {
        public static void Add (string name)
        public static void Delete (string name)
    }
    public static class Post
    {
        public static void Add (string name)
        public static void Delete (string name)
    }
}
````
These classes and their methods within the `App.Commands` namespace are made available to be executed using the following code:

```c#
class Program
{
    static void Main(string[] args)
    {
        var output = Router.Invoke("App.Commands", args);
    }
}
````

To run the `calculator` command:

* `app.exe calculator add --first 1 --second 2`

To run the `blog` command and its `comment` and `post` sub commands:

* `app.exe blog comment add --name "My comment"`
* `app.exe blog comment delete --name "My Comment"`
* `app.exe blog post add --name "My post"`
* `app.exe blog post delete --name "My post"`

# Go-live features roadmap

- [x] Command name to method name mapping
- [x] Argument long name to parameter name mapping
- [x] Argument value to parameter data type mapping
- [x] Optional arguments (use default parameter values, e.g. can exclude the `--second` parameter when the ``second`` parameter is defined as `int second = 0`; e.g. `app.exe add --first 3`)
- [x] Nested commands (sub commands, e.g. `app.exe calc add ...`)
- [x] Command parameter sets (multiple methods with the same name with different parameters)
- [x] Parameter name override using `DisplayAttribute`
- [x] Parameter short name override using `DisplayAttribute` (e.g. `app.exe add -f 3 -s 5`)
- [x] NuGet package [![MyGet](https://img.shields.io/myget/amervitz/v/amervitz.commandline.svg?label=MyGet)](https://www.myget.org/feed/amervitz/package/nuget/amervitz.commandline)
- [ ] Command name override using `DisplayNameAttribute` 
- [ ] Command listing help display (e.g. `app.exe help`)
- [ ] Command help display (e.g. `app.exe help add`)
- [ ] Comprehensive unit tests
