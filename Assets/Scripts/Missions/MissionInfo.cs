using System;
using Missions.MissionImplementations;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class MissionInfo 
    {
        [SerializeReference] public ScriptableMission Mission; 
        [SerializeField] public float delayTime;
    }
}