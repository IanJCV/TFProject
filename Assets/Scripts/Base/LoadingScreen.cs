using System;
using TF.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TF.Base
{
    public class LoadingScreen
    {
        private static LoadingScreen _instance;

        private LoadingScreenPanel _panel;
        private Image _bar;
        
        private Game _game;

        public static LoadingScreen CreateInstance(Game game)
        {
            if (_instance is null)
                return new LoadingScreen(game);
            return null;
        }
        
        public LoadingScreen(Game game)
        {
            _game = game;
            _instance = this;
            _game.onFinishedInitializing += OnFinishedInitializing;
            LoadPanel();
        }

        private void OnFinishedInitializing(object sender, EventArgs e)
        {
            _game.onFinishedInitializing -= OnFinishedInitializing;
            Destroy();
        }

        public void Destroy()
        {
            _instance = null;
            _bar = null;
            _game = null;
            Object.Destroy(_panel.gameObject);
        }
        
        private void LoadPanel()
        {
            var panelAsset = Resources.Load<GameObject>("Prefabs/UI/loadingscreen");
            _panel = Object.Instantiate(panelAsset).GetComponent<LoadingScreenPanel>();
            _bar = _panel.fillBar;
        }

        internal void Update(float progress, string message)
        {
            _bar.fillAmount = progress;
        }
        
    }
}