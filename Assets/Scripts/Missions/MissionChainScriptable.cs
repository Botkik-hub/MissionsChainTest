using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Missions
{
    [CreateAssetMenu(fileName = "New Missions Chain", menuName = "Missions/Chain")]
    public class MissionChainScriptable : ScriptableObject 
    {
        [SerializeField] private string chainName;
        [SerializeField] private List<MissionInfo> missions;

        public IReadOnlyList<MissionInfo> Missions => missions;
        public string ChainName => chainName;

        private void OnValidate()
        {
            foreach (var missionInfo in missions)
            {
                if (missionInfo.Mission != null)
                {
                    if (missionInfo.Mission.Chain == null)
                    {
                        missionInfo.Mission.Chain = this;
                    }
                    else if (missionInfo.Mission.Chain != this)
                    {
                        Debug.LogError("Mission is already assigned to the other chain", missionInfo.Mission.Chain);
                    }
                }
            }
        }

        
        /// <summary>
        ///  This code is for editor only, it should not be called during runtime
        /// </summary>
        /// <param name="scriptableMission"></param>
        public void RemoveMission(ScriptableMission scriptableMission)
        {
            var info = missions.FirstOrDefault(m => ReferenceEquals(m.Mission, scriptableMission));
            if (info != null)
            {
                missions.Remove(info);
            }
        }
    }
}