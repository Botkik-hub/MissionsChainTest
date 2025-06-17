using Testing;
using UnityEngine;

namespace Missions.MissionImplementations
{
    [CreateAssetMenu(fileName = "New CollectItemsMission", menuName = "Missions/CollectItemsMission")]
    public class CollectItemsMission : ScriptableMission
    {
        [SerializeField] private string itemName;
        [Min(1)] [SerializeField] private int itemAmount;
        private int _collectedItems;
        private Player _player;
        public override void Start()
        {
            isCompleted = false;
            _collectedItems = 0;
            _player = FindFirstObjectByType<Player>();
            if (_player != null)
            {
                _player.OnItemCollected += PlayerOnOnItemCollected;
            }
            else
            {
                Debug.LogError("Player not found");
            }
            InvokeStart();
        }

        private void OnDestroy()
        {
            UnsubscribePlayer();
        }

        private void PlayerOnOnItemCollected(string collectedItem)
        {
            if (isCompleted)
                return;
            if (collectedItem == itemName)
            {
                _collectedItems++;
            }
            MissionProgress = $"Collected Items: {_collectedItems}/{itemAmount}";
            InvokePointReached();
            if (_collectedItems >= itemAmount)
            {
                UnsubscribePlayer();
                isCompleted = true;
                InvokeFinish();
            }
        }

        private void UnsubscribePlayer()
        {
            if (_player == null)
                return;
            
            _player.OnItemCollected -= PlayerOnOnItemCollected;
            _player = null;
        }
    }
}