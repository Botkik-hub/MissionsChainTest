using System;
using UnityEngine;

namespace Missions
{
    [Serializable]
    public class MissionInfo 
    {
        [SerializeReference] public ScriptableMission Mission; 
        [SerializeField] public float delayTime;
    }
}