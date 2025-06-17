using System;
using DefaultNamespace;
using Missions;

public interface IMission
{
    // I have added IMission to actions so we would know which mission has been finished
    // I have also added a way to display those missions in ui 
    string MissionName { get; }
    string MissionProgress { get; }
    bool IsCompleted { get; }
    event Action<IMission> OnStarted;
    event Action<IMission> OnMissionPointReached;
    event Action<IMission> OnFinished;
    void Start();

    MissionChainScriptable Chain { get; }
}