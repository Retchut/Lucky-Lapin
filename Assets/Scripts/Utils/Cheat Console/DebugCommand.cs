using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class DebugCommandAttribute : Attribute
{
    public string Id;
    public string Description;

    public DebugCommandAttribute(string idd, string description)
    {
        Id = idd;
        Description = description;
    }
}

public class DebugCommandBase
{
    string _commandId;
    string _commandDescription;
    //Creating Gettters
    public string commandId { get => _commandId; }

    public string commandDescription { get => _commandDescription; }

    public DebugCommandBase(string commandId, string commandDescription)
    {
        _commandId = commandId;
        _commandDescription = commandDescription;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand( string id, string Description, Action command) : base(id, Description)
    {
        this.command = command;
    }
    public void Invoke()
    {
        command.Invoke();
    }
}
public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;
    public DebugCommand(string id, string Description, Action<T1> command) : base(id, Description)
    {
        this.command = command;
    }
    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}
