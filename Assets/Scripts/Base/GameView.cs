using UnityEngine;

namespace TF.Base
{
    public class GameView : MonoBehaviour
    {
        private Game _game;
        
        private void Awake()
        {
            _game = new Game(this);
        }

        private void Update()
        {
            
        }
    }
}