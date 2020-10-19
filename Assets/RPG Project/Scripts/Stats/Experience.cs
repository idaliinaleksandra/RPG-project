using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] public float level = 1;
        [SerializeField] public float expPoints = 0;

        public void GainEXP(float experience)
        {
            expPoints += experience;
        }

        public int GetLevel()
        {
            return GetComponent<BaseStats>().GetLevel();
        }

        public float GetEXP()
        {
            return expPoints;
        }

        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            expPoints = (float)state;
        }

    }
}
