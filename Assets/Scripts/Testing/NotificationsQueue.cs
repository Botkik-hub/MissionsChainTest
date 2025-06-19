using System;
using System.Collections.Generic;
using Missions;
using TMPro;
using UnityEngine;

namespace Testing
{
    public class NotificationsQueue : MonoBehaviour
    {
        private Queue<IMission> _missionsNotifications;


        private bool _isShowing = false;

        private Timer _timer;

        [SerializeField] private int showTimeMilliseconds = 1000;
        [SerializeField]private TMP_Text notificationText;
        
        private void Awake()
        {
            _timer = new Timer();
            notificationText.enabled = false;
            _missionsNotifications = new Queue<IMission>();
        }

        private void OnDestroy()
        {
            _timer.Cancel();
            _timer = null;
            _missionsNotifications.Clear();
            _missionsNotifications = null;
        }

        public void AddNotification(IMission mission)
        {
            _missionsNotifications.Enqueue(mission);
            if (!_isShowing)
            {
                ShowNextNotification();
            }
        }
        
        private void ShowNextNotification()
        {
            if (_missionsNotifications.Count == 0)
                return;
            if (_isShowing)
                return;
            _isShowing = true;
            var mission = _missionsNotifications.Dequeue();
            notificationText.text = $"Mission Completed: {mission.MissionName}";
            notificationText.enabled = true;
            _ = _timer.StartAsync(showTimeMilliseconds, OnComplete);
        }

        private void OnComplete()
        {
            _isShowing = false;
            notificationText.enabled = false;
            if (_missionsNotifications.Count != 0)
            {
                ShowNextNotification();
            }
        }
    }
}