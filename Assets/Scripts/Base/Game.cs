using System;
using System.IO;
using TF.Console;
using TF.Audio;
using UnityEngine;

namespace TF.Base
{
    /// <summary>
    /// Entry point for the game. Initializes all systems and spawns the necessary entities.
    /// <para>
    /// This is done way closer to the Source Engine than necessary, so it's kind of going against
    /// best Unity practices. This project was made for learning anyway, so it doesn't really matter.
    /// </para>
    /// </summary>
    public class Game
    {
        private string cfgPath
        {
            get => Application.dataPath + "/cfg/";
        }

        public static bool Pause
        {
            get;
            private set;
        }

        public static float DTime
        {
            get => Pause ? 0f : Time.deltaTime;
        }

        public static float GameTime
        {
            get;
            private set;
        }

        private GameView _view;
        
        private Sound _sound;
        private GameConsole _console;
        private LoadingScreen _loadingScreen;

        private static LevelBase _currentLevel;

        internal float loadingProgress;
        internal string latestMessage;
        internal event EventHandler onFinishedInitializing;

        public Game(GameView view)
        {
            // Create statics
            // Spawn Entities
            // Initialize game systems (UI, Sound)

            // Init the loading screen here
            _loadingScreen = LoadingScreen.CreateInstance(this);
            
            _view = view;
            
            RegisterLoadingProgress("Initializing console...");
            _console = GameConsole.CreateInstance();
            _console.onFinishedLoading.AddListener(OnConsoleInitialized);
            
            RegisterLoadingProgress("Initializing sounds...");
            _sound = Sound.CreateInstance();
            
            // Execute CFGs only after every system has booted up, but before we go to the main menu.
            
            // Go to the main menu here
        }

        private void OnConsoleInitialized()
        {
            _console.onFinishedLoading.RemoveListener(OnConsoleInitialized);
            RegisterLoadingProgress("Reading configs...");
            LoadCFG();
            var handler = onFinishedInitializing;
            handler?.Invoke(this, null);
        }

        private void RegisterLoadingProgress(string message)
        {
            loadingProgress += 0.25f;
            loadingProgress = Mathf.Min(loadingProgress, 1f);
            latestMessage = message;
            _loadingScreen?.Update(loadingProgress, message);
        }

        private void LoadCFG()
        {
            Directory.CreateDirectory(cfgPath);
            string[] files = Directory.GetFiles(cfgPath, "*.cfg");
            
            if (files.Length < 1)
                return;

            foreach (var fileName in files)
            {
                RegisterLoadingProgress($"Executing config...");
                var stream = File.OpenText(fileName);
                while (stream.Peek() >= 0)
                {
                    GameConsole.Run(stream.ReadLine());
                }
            }
        }

        public static LevelBase GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void SetPause(bool pause) => Pause = pause;
    }
}