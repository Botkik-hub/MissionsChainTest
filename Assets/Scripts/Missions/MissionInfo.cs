using System;
using UnityEngine;

namespace Missions
{
    [Serializable]
    public class MissionInfo
    {
        public IMission Mission => mission;
        [SerializeReference] private ScriptableMission mission; 
        [SerializeField] public float delayTime;
    }
}