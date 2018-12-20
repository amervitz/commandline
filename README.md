Clear - **C**ommand **L**in**e** **A**rgument **R**outer

# About

This project is an early in-progress effort to implement a .NET Standard class library that makes it trivial to write .NET console applications without having to implement argument parsing logic, by providing an API to automatically parse and route command line arguments to methods and parameters to be executed.

# Status

The code is under active development and not ready for production use.

# Design

The argument routing logic is in the `\src\Clear` class library. This project will eventually become a NuGet package. To use it now, clone this repository, and add a reference to this project to your console application.

A sample console application that uses the routing logic to execute commands is in the `\src\App` project.

The `Clear` library contains the following files:

* `\src\Clear\Router.cs` contains multiple `Run` methods to find and execute commands using reflection.

The `App` sample console application contins the following files:

* `\src\App\Program.cs` contains the entry point for taking the command line arguments and using the `\src\Clear` class library to execute a command that is a static method contained in a specific class or to be found within a namespace.

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
        var output = Clear.Router.Run(typeof(Calculator), args);
    }
}
```

Running the `add` *command* from the command line is then possible using any of these command line argument combinations:

* ``app.exe add --first 3 --second 5``
* ``app.exe add --first 3 5``
* ``app.exe add 3 --second 5``
* ``app.exe add 3 5``

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
        var output = Clear.Router.Run("App.Commands", args);
    }
}
````

To run the `calculator` command:

* ``app.exe calculator add --first 1 --second 2``

To run the `blog` command and its `comment` and `post` sub commands:

* ``app.exe blog comment add --name "My comment"``
* ``app.exe blog comment delete --name "My Comment"``
* ``app.exe blog post add --name "My post"``
* ``app.exe blog post delete --name "My post"`` 
# Features / Todo

- [x] Command name to method name mapping
- [x] Argument long name to parameter name mapping
- [x] Argument value to parameter data type mapping
- [x] Optional arguments (use default parameter values, e.g. can exclude the `--second` parameter when the ``second`` parameter is defined as `int second = 0`; e.g. `app.exe add --first 3`)
- [x] Nested commands (sub commands, e.g. `app.exe calc add ...`)
- [x] Command parameter sets (multiple methods with the same name with different parameters)
- [ ] Short names to parameter name mapping (e.g. `app.exe add -f 3 -s 5`)
- [ ] Command listing help display (e.g. `app.exe help`)
- [ ] Command help display (e.g. `app.exe help add`)
- [ ] Data annotation attributes for command metadata
- [ ] Attribute routing
- [ ] Unit tests
- [ ] NuGet package