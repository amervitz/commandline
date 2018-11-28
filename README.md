Clear - **C**ommand **L**in**e** **A**rgument **R**outer

# About

This project is an experimental attempt at making a class library that makes it trivial to have command line arguments get routed/mapped to a method to execute.

# Status

The code is very early, raw, messy, under active development, and not ready for use.

# Design

The argument routing logic is contained in the `\src\Clear` project which is also the main entry point for a console application. Eventually the argument routing logic will be in a project that is only a class library.

The project contains the following files:

* The `TopLevel.cs` class contains methods that represent multiple commands in the application that can be run from the command line.
* The `Program.cs` class contains the entry point for taking the command line arguments and using reflection to find and execute the appropriate method in the `TopLevel` class with its parameters.

# Command and Argument Routing
The first command line argument is the **command** that is compared to the method names in the `TopLevel` class to find the method to execute. The remaining parameters are the **arguments** to supply to each parameter in the found method.

The command line arguments are processed from left to right.

* When the next argument to process begins with `--` it is assumed to be a parameter name in the method, with the following argument being the value to assign to the parameter.
* When the next argument to process does not begin with `--` it is assumed to be a value to assign to the next parameter in the method.

For example, given an `Add` method that adds two numbers with this signature:

```c#
public static int Add(int first, int second)
```

Running the `add` *command* from the command line could be done using any of these command line argument combinations:

* ``dotnet run add --first 3 --second 5``
* ``dotnet run add --first 3 5``
* ``dotnet run add 3 --second 5``
* ``dotnet run add 3 5``

# Features / Todo

- [x] Command name to method name mapping
- [x] Argument long name to parameter name mapping
- [x] Argument value to parameter data type mapping
- [x] Optional arguments (use default parameter values, e.g. can exclude the `--second` parameter when the ``second`` parameter is defined as `int second = 0`; e.g. `dotnet run add --first 3`)
- [ ] Short names to parameter name mapping (e.g. `dotnet run add -f 3 -s 5`)
- [ ] Command parameter sets (multiple methods with the same name with different parameters)
- [ ] Nested commands (sub commands, e.g. `dotnet run calc add ...`)
- [ ] Command listing help display (e.g. `dotnet run help`)
- [ ] Command help display (e.g. `dotnet run help add`)
- [ ] Data annotation attributes for command metadata
- [ ] Attribute routing
- [ ] Unit tests
- [ ] NuGet package