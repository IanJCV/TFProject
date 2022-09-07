using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace TF.Console
{
    public class GameConsole
    {
        public bool isInitialized;
        
        private static GameConsole _instance;

        private ConsoleView _view;
        
        internal List<ConVar> conVars;
        internal List<ConCommand> commands;

        public UnityEvent onFinishedLoading;

        public static GameConsole CreateInstance()
        {
            if (_instance is null)
                return new GameConsole();
            return null;
        }

        private GameConsole()
        {
            if (_instance != null)
                return;
            
            _instance = this;
            conVars = new List<ConVar>();
            commands = new List<ConCommand>();

            _view = new GameObject("Console").AddComponent<ConsoleView>();
            _view.console = this;
            _view.onInitialized += onViewFinishedLoading;
            onFinishedLoading = new UnityEvent();
        }

        ~GameConsole()
        {
            Object.Destroy(_view.gameObject);
        }

        private void onViewFinishedLoading(object sender, EventArgs e)
        {
            isInitialized = true;
            onFinishedLoading.Invoke();
        }
        
        /// <summary>
        /// Runs a passed command.
        /// </summary>
        public static void Run(string input)
        {
            if (input == "" || input == null)
            {
                ConsoleView.Log("");
                return;
            }
            
            string[] splitInputs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (TryGetConVar(splitInputs[0], out var convar))
            {
                if (splitInputs[1] == null)
                {
                    ConsoleView.Log($"convar {convar.GetName()} - default value: " + 
                        convar.GetDefaultValue().ToString("0.0"));
                }
                
                if (float.TryParse(splitInputs[1], out float value))
                {
                    convar.SetFloat(value);
                    convar.Call();
                    return;
                }
                ConsoleView.Log("Could not parse argument to command!", LogSeverity.ERROR);
                return;
            }

            if (TryGetCommand(splitInputs[0], out var command))
            {
                var cmd = new CommandArgs(splitInputs.Skip(1).ToArray());
                command.Call(cmd);
                return;
            }
            
            ConsoleView.Log("Input does not match any command or convar.", LogSeverity.ERROR);
        }

        internal static bool TryGetConVar(string name, out ConVar convar)
        {
            for (int i = 0; i < _instance.conVars.Count; i++)
            {
                if (_instance.conVars[i].GetName() == name)
                {
                    convar = _instance.conVars[i];
                    return true;
                }
            }

            convar = null;
            return false;
        }
        
        internal static bool TryGetCommand(string name, out ConCommand command)
        {
            for (int i = 0; i < _instance.commands.Count; i++)
            {
                if (_instance.commands[i].GetName() == name)
                {
                    command = _instance.commands[i];
                    return true;   
                }
            }

            command = null;
            return false;
        }

        public static void RegisterCommand(string name, Action<CommandArgs> onCall)
        {
            if (_instance is null)
            {
                Debug.LogError($"Console not initialized. Could not register command by name {name}");
                return;
            }

            for (int i = 0; i < _instance.commands.Count; i++)
            {
                if (_instance.commands[i].GetName() == name)
                {
                    // Already exists.
                    return;
                }
            }

            ConCommand command = new ConCommand(name, onCall);
            
            _instance.commands.Add(command);
        }

        public static void RegisterConVarSetFloat(string name, float defaultValue, Action onSet, out ConVar variable)
        {
            if (_instance is null)
            {
                Debug.LogError($"Console not initialized. Could not register convar {name}.");
                variable = null;
                return;
            }

            for (int i = 0; i < _instance.conVars.Count; i++)
            {
                if (_instance.conVars[i].GetName() == name)
                {
                    // Already exists in the list. Do not add duplicates.
                    variable = null;
                    return;
                }
            }

            ConVar command = new ConVar(name, defaultValue, onSet);

            variable = command;
            
            _instance.conVars.Add(command);
        }

        public static void RegisterConVar(ConVar convar)
        {
            if (_instance is null)
            {
                Debug.LogError($"Console not initialized. Could not register convar {convar.GetName()}.");
                return;
            }
            
            for (int i = 0; i < _instance.conVars.Count; i++)
            {
                if (_instance.conVars[i].GetName() == convar.GetName())
                {
                    // Already exists in the list. Do not add duplicates.
                    return;
                }
            }
            
            _instance.conVars.Add(convar);
        }

        internal static void UnregisterConvar(string convarName)
        {
            if (_instance is null)
            {
                Debug.LogError($"Console not initialized. Could not unregister convar {convarName}.");
                return;
            }

            for (int i = _instance.conVars.Count - 1; i >= 0; i--)
            {
                if (_instance.conVars[i].GetName() == convarName)
                {
                    _instance.conVars.RemoveAt(i);
                    return;
                }
            }
            
        }
    }

    [Flags]
    public enum ConVarFlags : short
    {
        Cheat = 0,
        Debug = 1 // 2, 4, 8, 16, 32, 64, etc.
    }

    /// <summary>
    /// Contains unparsed args passed in the command.
    /// <para> Use GetLength() and GetToken(index). </para>
    /// </summary>
    public struct CommandArgs
    {
        private string[] Tokens;

        public int GetLength() => Tokens.Length;
        public string GetToken(int index) => Tokens[index];

        public CommandArgs(string args)
        {
            Tokens = args.Split(' ');
        }

        public CommandArgs(string[] args)
        {
            Tokens = args;
        }
    }

    public class ConVar
    {
        private string _name;
        private float _value;
        private float _defaultValue;
        private Action _onSet;

        public ConVar(string name, float defValue, Action callback)
        {
            _name = name;
            _value = defValue;
            _defaultValue = defValue;
            _onSet = callback;
            
            GameConsole.RegisterConVar(this);
        }

        ~ConVar()
        {
            GameConsole.UnregisterConvar(_name);
        }

        internal void SetFloat(float value) => _value = value;
        public float GetFloat() => _value;
        public float GetDefaultValue() => _defaultValue;
        public string GetName() => _name;
        public void Call() => _onSet.Invoke();
    }

    public class ConCommand
    {
        private string _name;
        private Action<CommandArgs> _onSet;

        public ConCommand(string name, Action<CommandArgs> onSet)
        {
            _name = name;
            _onSet = onSet;
        }

        public string GetName() => _name;
        public void Call(CommandArgs args) => _onSet.Invoke(args);
    }
}