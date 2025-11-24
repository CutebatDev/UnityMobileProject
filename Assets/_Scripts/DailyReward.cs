using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class DailyReward : MonoBehaviour
    {
        [SerializeField] private GameObject off;
        [SerializeField] private GameObject ready;
        [SerializeField] private GameObject on;

        [SerializeField] private TextMeshProUGUI rewardAmountText;
        [SerializeField] private TextMeshProUGUI dayNum;
        [SerializeField] private int amountDays = 1;

        private readonly System.DateTime _lastDayEntered = System.DateTime.Today;
        private int today;

        private int _rewardAmount = 10;
        private bool _isAvailable = false;

        private int _coins = 0;

        private void Start()
        {
            off.SetActive(true);
            ready.SetActive(false);
            on.SetActive(false);

            _rewardAmount *= amountDays;
            rewardAmountText.text = _rewardAmount.ToString();
            dayNum.text = "Day " + amountDays;

            CheckDay();
            SetRewardDay();
        }

        private void CheckDay()
        {
            today = System.DateTime.Today.Day;
        }

        private void SetRewardDay()
        {
            if (today == amountDays)
            {
                _isAvailable = true;
                off.SetActive(false);
                ready.SetActive(true);
                on.SetActive(false);
            }
            else
            {
                _isAvailable = false;
                off.SetActive(true);
                ready.SetActive(false);
                on.SetActive(false);
            }
        }

        private void OnClicked()
        {
            if (_isAvailable)
            {
                off.SetActive(false);
                ready.SetActive(false);
                on.SetActive(true);

                _coins += _rewardAmount;
            }
        }
    }
}