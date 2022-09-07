using System;
using TF.Runtime;
using TMPro;
using UnityEngine;

namespace TF.Console
{
    /// <summary>
    /// The component side of the Console.
    /// Gets instantiated by the Console object automatically.
    /// </summary>
    [AddComponentMenu("")]
    public class ConsoleView : MonoBehaviour
    {
        internal GameConsole console;
        internal bool _isConsoleLoaded = false;
        internal bool debug;

        internal static bool _isConsoleVisible
        {
            get => _instance.activeSelf;
        }

        private static ConsoleView _view;
        private static GameObject _instance;
        private ResourceRequest _loadRequest;

        private static TMP_InputField _inputField;
        private static TextMeshProUGUI _textField;

        internal event EventHandler onInitialized;
        
        private void Start()
        {
            _view = this;
            
            _loadRequest = Resources.LoadAsync<GameObject>("Prefabs/UI/console");

            _loadRequest.completed += OnConsoleLoaded;
            
            
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            debug = true;
#else
                _debug = false;
#endif
            
            LoadDefaultCommands();
        }

        // Where default global commands should be set.
        private void LoadDefaultCommands()
        {
            GameConsole.RegisterCommand("debug", SetDebugMode);
        }

        private void SetDebugMode(CommandArgs args)
        {
            if (TryParseBoolean(args.GetToken(0), out bool result))
            {
                _view.debug = result;
                Log(_view.debug ? "Debug mode activated." : "Debug mode deactivated.", LogSeverity.MESSAGE);
                return;
            }
            Log("Couldn't parse message.", LogSeverity.ERROR);
        }

        private static bool TryParseBoolean(string value, out bool output)
        {
            if (value.Equals("1"))
            {
                output = true;
                return true;
            }
            else if (value.Equals("0"))
            {
                output = false;
                return true;
            }
            else if (bool.TryParse(value, out output))
            {
                return true;
            }
            else
                return false;
        }

        private void OnConsoleLoaded(AsyncOperation obj)
        {
            _isConsoleLoaded = true;
            _instance = Instantiate((GameObject)_loadRequest.asset);

            var consolePanel = _instance.GetComponent<ConsolePanel>();
            _inputField = consolePanel.inputField;
            _textField = consolePanel.text;

            _inputField.onSubmit.AddListener(OnSubmit);
            _inputField.onValueChanged.AddListener(OnInputChanged);
            
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Show(true);
#else
            Show(false);
#endif

            EventHandler handler = onInitialized;
            handler?.Invoke(this, null);
        }

        public static void Show(bool value)
        {
            if (_instance is null)
                return;
            _instance.SetActive(value);
        }

        public static void Log(string msg, LogSeverity severity = LogSeverity.MESSAGE)
        {
            switch (severity)
            {
                case LogSeverity.DEBUG:
                    if (_view.debug) _view.AddMessage("[DEBUG]: " + msg + "\n] ");
                    break;
                case LogSeverity.ERROR:
                    _view.AddMessage("[ERROR]: " + msg + "\n] ");
                    Debug.LogError(msg);
                    break;
                case LogSeverity.MESSAGE:
                    _view.AddMessage(msg + "\n] ");
                    break;
            }
            if (_view.debug && !_isConsoleVisible)
                Show(true);
        }

        private void AddMessage(string msg)
        {
            _textField.text += msg;
        }

        // TODO: Prediction
        private void OnInputChanged(string newinput)
        {
            
        }

        private void OnSubmit(string input)
        {
            GameConsole.Run(input);
            _inputField.text = "";
        }
    }

    public enum LogSeverity
    {
        DEBUG,
        MESSAGE,
        ERROR
    }
}