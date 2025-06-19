using System.Collections.Generic;
using Testing;
using TMPro;
using UnityEngine;

namespace Missions
{
    public class MissionManager : MonoBehaviour
    {
        private readonly List<IMission> _currentMissions = new List<IMission>();
        private readonly List<IMission> _completedMissions = new List<IMission>();

        private IMission _trackedMission;

        public IReadOnlyList<IMission> ActiveMissions => _currentMissions;
        public IReadOnlyList<IMission> CompletedMission => _completedMissions;
        
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private TMP_Text missionText;

        [SerializeField] private NotificationsQueue notificationsManager;
        
        private List<MissionChainScriptable> _missionChains = new List<MissionChainScriptable>();
        
        public void AddMissionsChain(MissionChainScriptable chain)
        {
            if (_missionChains.Contains(chain))
            {
                Debug.LogError("This mission chain is already started/completed");
                return;
            }
            
            _missionChains.Add(chain);
            var startingMission  = chain.Missions[0];
            StartMission(startingMission.Mission);
        }

        public void AddMission(IMission mission)
        {
            if (mission.Chain != null)
            {
                Debug.LogError("This mission has chain and should be started from there!");
                return;
            }

            StartMission(mission);
        }
        
        private void StartMission(IMission mission)
        {
            if (_completedMissions.Contains(mission) || _currentMissions.Contains(mission))
            {
                Debug.LogError("This mission is already started/completed");
                return;
            }
            Debug.Log($"Mission added: {mission.MissionName}");
            mission.OnFinished += MissionOnFinished;
            _currentMissions.Add(mission);
            mission.Start();
            if (_trackedMission == null)
            {
                SetTrackedMission(mission);
            }
        }

        public void SetTrackedMission(IMission mission)
        {
            if (_trackedMission != null)
            {
                RemoveTrackedMission();
            }
            mission.OnMissionPointReached += OnMissionPointReached;
            _trackedMission = mission;
            missionName.text = mission.MissionName;
            missionText.text = mission.MissionProgress;
        }

        public void ShowNextTrackedMission()
        {
            if (ActiveMissions.Count < 2)
            {
                return;
            }

            int missionId;
            if (_trackedMission == null)
            {
                missionId = 0;
            }
            else
            {
                missionId = _currentMissions.IndexOf(_trackedMission);
                missionId = (missionId + 1) % _currentMissions.Count;
            }
            SetTrackedMission(_currentMissions[missionId]);
        }
        
        private void Awake()
        {
            // possibly load mission progresses and completed missions
            RemoveTrackedMission();
        }
 
        private void OnDestroy()
        {
            if (_trackedMission != null)
                RemoveTrackedMission();
            
            foreach (var mission in _currentMissions)
            {
                mission.OnFinished -= MissionOnFinished;
                mission.Dispose();
            }
            // Possibly serialize progress of missions here, serialize completed missions
            _currentMissions.Clear();
            _completedMissions.Clear();
        }
        private void RemoveTrackedMission()
        {
            missionText.text = string.Empty;
            missionName.text = string.Empty;
            if (_trackedMission == null)
                return;
            _trackedMission.OnMissionPointReached -= OnMissionPointReached;
            _trackedMission = null;
        }
        private void OnMissionPointReached(IMission mission)
        {
            if (_trackedMission == mission)
            {
                missionText.text = mission.MissionProgress;
            }
        }
        private void MissionOnFinished(IMission mission)
        {
            mission.OnFinished -= MissionOnFinished;
            
            _currentMissions.Remove(mission);
            _completedMissions.Add(mission);

            if (notificationsManager != null)
            {
                notificationsManager.AddNotification(mission);
            }
            
            if (_trackedMission == mission)
            {
                RemoveTrackedMission();
            }
            
            if (mission.Chain != null)
            {
                ContinueMissionChain(mission);
            }
        }

        private void ContinueMissionChain(IMission mission)
        {
            if (mission == null) return;
            if (mission.Chain == null) return;
            var chain = mission.Chain;
            MissionInfo nextMission = null;
            for (int i = 0; i < chain.Missions.Count - 1; i++)
            {
                if (ReferenceEquals(chain.Missions[i].Mission, mission))
                {
                    nextMission = chain.Missions[i + 1];
                    break;
                }
            }

            if (nextMission == null)
                return;

            if (nextMission.delayTime != 0)
            {
                var timer = new Timer();
                _ = timer.StartAsync((int)(nextMission.delayTime * 1000), ()=>StartMission(nextMission.Mission));
            }
            else
            {
                StartMission(nextMission.Mission);
            }
        }
    }
}