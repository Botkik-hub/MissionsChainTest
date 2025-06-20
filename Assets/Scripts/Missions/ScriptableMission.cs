﻿using System;
using UnityEngine;

namespace Missions
{
    public abstract class ScriptableMission : ScriptableObject, IMission
    {
        protected bool isCompleted = false;
        [SerializeField] private string missionName;
        
        public string MissionName => missionName;
        public bool IsCompleted => isCompleted;
        public string MissionProgress { get; protected set; }
        public event Action<IMission> OnStarted;
        public event Action<IMission> OnMissionPointReached;
        public event Action<IMission> OnFinished;
        
        public abstract void Start();
        
        public MissionChainScriptable Chain { get; set; }

        protected void InvokeStart()
        {
            OnStarted?.Invoke(this);
        }

        protected void InvokePointReached()
        {
            OnMissionPointReached?.Invoke(this);
        }

        protected void InvokeFinish()
        {
            OnFinished?.Invoke(this);
        }

        public virtual void Dispose()
        {
            
        }

        [ContextMenu("Remove chain")]
        private void RemoveChain()
        {
            Chain?.RemoveMission(this);
            Chain = null;
        }
    }
}