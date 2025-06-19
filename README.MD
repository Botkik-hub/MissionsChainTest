# Test task create Mission chains.

Each mission in the chain starts after previous was finished, possibly with some delay.

Multiple chains can be started simultaneously. 

Must use UniTask to wait between missions. 

Mission chains must be set up through 

## General logic:

1. IMission
   - I have replaced Action with Action<IMission> so the subscribers would know which mission has finished
   - I have added few properties to be able to show mission information to the screen, it includes:
      MissionName and MissionProgress
   - I have added Chain so mission would know what chain it is assigned to
   - I have added IDisposable to the interface to clean up if the object implemented object is not MonoBehaviour
2. MissionManager:
   - Class that handles logic of starting mission, keeping track and showing them to screen.
   - This class starts chains and separate missions. 
   - this class also shows missions on screen with labels, this probably shouldn't be here and moved to some UI manager. But for demo purposes I felt it is fine
   - One mission cannot be added multiple times to the manager. I have made it  so each mission can be started or added only once.
3. ScriptableMission:
   - Scriptable Object that implements IMission interface
   - I have implemented IMission in Scriptable object to be able to assign it to chains
4. MissionChain:
   - Scriptable object that holds List of pairs of scriptableMission and delay before its start.
   - Mission can only be assigned one chain.
   - If mission is assigned to multiple chains at once it will give debug error.
5. MissionInfo:
   - class that represents pair of mission and delay before it.

## Testing:
- You can find test scene and test functionality
- I have added two types of missions
   - Wait some time
   - Collect items
- I have created two chains with those types of missions
- On the scene there is two buttons to start chains, button to cycle between all active missions and button to add items (to complete collection missions)
