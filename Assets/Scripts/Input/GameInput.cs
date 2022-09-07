using System;
using TF.Console;
using UnityEngine;

namespace TF.Input
{
    public class GameInput
    {
        private static GameInput _instance;
        public static PlayerCommands cmd;

        private GameInput()
        {
            _instance = this;
            cmd = new PlayerCommands();
            
            CreateBindCommands();
        }

        private void CreateBindCommands()
        {
            GameConsole.RegisterCommand("bind",BindCommand);
        }

        private void BindCommand(CommandArgs args)
        {
            if (args.GetLength() < 2)
            {
                ConsoleView.Log("Wrong number of parameters.", LogSeverity.ERROR);
                return;
            }
            
            string binding = args.GetToken(0);
            string key = args.GetToken(1);
            if (TryGetKeyCode(key, out KeyCode code))
            {

            }
            ConsoleView.Log($"Couldn't parse key '{key}'.");
        }

        private bool TryGetKeyCode(string input, out KeyCode key)
        {
            return Enum.TryParse(input, out key);
        }

        public static GameInput CreateInstance()
        {
            if (_instance is null)
                return new GameInput();
            return null;
        }
        

    }
}