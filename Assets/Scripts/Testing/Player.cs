using System;
using UnityEngine;

namespace Testing
{
    public class Player : MonoBehaviour
    {
        public event Action<string> OnItemCollected;

        public void CollectItem(string itemName)
        {
            OnItemCollected?.Invoke(itemName);
        }
    }
}