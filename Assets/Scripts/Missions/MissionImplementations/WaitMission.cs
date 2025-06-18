using UnityEngine;

namespace Missions.MissionImplementations
{
    [CreateAssetMenu(fileName = "New WaitMission", menuName = "Missions/WaitMission")]
    public class WaitMission : ScriptableMission
    {
        [SerializeField] private int timeInSeconds;

        private int secondsLeft;
        private Timer _timer;
        
        public override void Start()
        {
            isCompleted = false;
            secondsLeft = timeInSeconds;
            MissionProgress = $"Time remaining: {secondsLeft}";
            _timer = new Timer();
            // discard task, we dont want to exit start and treat this as event
            _ = _timer.StartAsync(1000, OnComplete);
            InvokeStart();
        }

        private void OnComplete()
        {
            secondsLeft--;
            MissionProgress = $"Time remaining: {secondsLeft}";
            InvokePointReached();
            if (secondsLeft <= 0)
            {
                isCompleted = true;
                InvokeFinish();
            }
            else
            {
                _ = _timer.StartAsync(1000, OnComplete);
            }
        }
    }
}