using System;
using UnityEngine;

namespace _Scripts
{
    public class ExpManager : MonoBehaviour
    {
        public int currentLevel = 1;
        public int currentExp = 0;
        public int requiredExp = 20;

        public void GetExp(int value)
        {
            currentExp += value;
            if(currentExp >= requiredExp)
                LevelUp();
        }

        private void LevelUp()
        {
            currentExp = Math.Clamp(currentExp-requiredExp, 0, Int32.MaxValue);
            currentLevel++;
            requiredExp = (int)Math.Floor(requiredExp*1.5);
        }
    }
}