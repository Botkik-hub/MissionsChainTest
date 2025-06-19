using System;
using DefaultNamespace;
using Missions;

public interface IMission : IDisposable
{
    // I have added IMission to actions so we would know which mission has been finished
    // I have also added a way to display those missions in ui 
    // Missions known what chain they are belong to
    // Missions Implements IDisposable to clean up after its done or Game closing
    // I went with using scriptable objects to implement IMissions and because of that I need 
    string MissionName { get; }
    string MissionProgress { get; }
    bool IsCompleted { get; }
    event Action<IMission> OnStarted;
    event Action<IMission> OnMissionPointReached;
    event Action<IMission> OnFinished;
    void Start();
    MissionChainScriptable Chain { get; }
}