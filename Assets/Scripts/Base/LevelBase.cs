using System;
using System.Collections.Generic;
using TF.Console;
using UnityEngine;
using UnityEngine.Events;

namespace TF.Base
{
    public class LevelBase : MonoBehaviour
    {
        public UnityEvent onLevelStart = new UnityEvent();
        public UnityEvent onLevelStartLoading = new UnityEvent();
        public UnityEvent onLevelLoaded = new UnityEvent();
        public UnityEvent onLevelEvent = new UnityEvent();

        private List<AsyncOperation> _loadingOperations = new List<AsyncOperation>();

        private ConVar lvRoundDuration;
        private ConVar lvMaxPlayers;

        private void Awake()
        {
            lvRoundDuration = new ConVar("lv_roundduration", 180f, UpdateRoundDuration);
            lvMaxPlayers = new ConVar("lv_maxplayers", 8, null);
        }

        public void UpdateRoundDuration()
        {
            
        }
    }
}