using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] public float expPoints = 0;

        public event Action onEXPGained;

        public void GainEXP(float experience)
        {
            expPoints += experience;
            onEXPGained();
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
