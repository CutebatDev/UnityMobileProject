using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class DailyReward : MonoBehaviour
    {
        [System.Serializable]
        private class RewardSlot
        {
            public GameObject offState;
            public GameObject activeState;
            public GameObject completedState;
            public int dayState;

            public void SetState(int state)
            {
                offState.SetActive(state == 0);
                activeState.SetActive(state == 1);
                completedState.SetActive(state == 2);
            }
        }

        [SerializeField] private RewardSlot[] rewardSlots = new RewardSlot[REWARD_DAYS];
        private const int REWARD_DAYS = 6;
        private const string LAST_DATE_KEY = "LastDate";
        private const string DAY_STATE_KEY = "Day_{0}";
        public event Action<bool> OnRewardAvailabilityChanged;


        private int _lastDate;

        private void Start()
        {
            LoadData();
            CheckDailyReset();
            RefreshDisplay();
            CheckAndNotifyRewardAvailability();
        }

        private void LoadData()
        {
            _lastDate = PlayerPrefs.GetInt(LAST_DATE_KEY, 0);

            for (int i = 0; i < rewardSlots.Length; i++)
            {
                int state = PlayerPrefs.GetInt(string.Format(DAY_STATE_KEY, i + 1), 0);
                rewardSlots[i].dayState = state;
            }
        }

        private void CheckDailyReset()
        {
            if (_lastDate == DateTime.Now.Day)
                return;

            _lastDate = DateTime.Now.Day;
            PlayerPrefs.SetInt(LAST_DATE_KEY, _lastDate);

            for (int i = 0; i < rewardSlots.Length; i++)
            {
                if (rewardSlots[i].dayState == 0)
                {
                    rewardSlots[i].dayState = 1;
                    SaveState(i);
                    break;
                }
            }
        }

        private void RefreshDisplay()
        {
            foreach (var slot in rewardSlots)
                slot.SetState(slot.dayState);
        }

        private void CheckAndNotifyRewardAvailability()
        {
            bool hasRewardToday = HasAvailableReward();
            OnRewardAvailabilityChanged?.Invoke(hasRewardToday);

            if (hasRewardToday)
            {
                SendRewardNotification();
            }
        }

        public bool HasAvailableReward()
        {
            foreach (var slot in rewardSlots)
            {
                if (slot.dayState == 1)
                    return true;
            }

            return false;
        }

        private void SendRewardNotification()
        {
#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject notification = new AndroidJavaObject(
                    "android.app.Notification");

                Debug.Log("Android notification sent: Daily reward available!");
            }
#elif UNITY_IOS
            Debug.Log("iOS notification sent: Daily reward available!");
#endif
        }

        public void ClaimReward(int rewardIndex)
        {
            if (!IsValidIndex(rewardIndex))
            {
                Debug.LogWarning($"Invalid reward index: {rewardIndex}");
                return;
            }

            rewardSlots[rewardIndex].dayState = 2;
            SaveState(rewardIndex);
            RefreshDisplay();

            Debug.Log($"Reward {rewardIndex + 1} claimed");
        }

        private void SaveState(int rewardIndex)
        {
            PlayerPrefs.SetInt(
                string.Format(DAY_STATE_KEY, rewardIndex + 1),
                rewardSlots[rewardIndex].dayState
            );
        }

        private bool IsValidIndex(int index) => index >= 0 && index < rewardSlots.Length;

        public void GetReward_1() => ClaimReward(0);
        public void GetReward_2() => ClaimReward(1);
        public void GetReward_3() => ClaimReward(2);
        public void GetReward_4() => ClaimReward(3);
        public void GetReward_5() => ClaimReward(4);
        public void GetReward_6() => ClaimReward(5);
    }
}