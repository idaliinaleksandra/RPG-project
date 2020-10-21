using UnityEngine;
using System;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] bool useModifiers = false;

        LazyValue <int> currentLevel;
        Experience exp;

        public event Action onLevelUp;

        private void Awake()
        {
            exp = GetComponent<Experience>();

            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (exp != null)
                exp.onEXPGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (exp != null)
                exp.onEXPGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                print("LEVEL UP");
                onLevelUp();
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAddativeModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel.value < 1)
            {
                currentLevel.value = CalculateLevel();
            }
            return currentLevel.value;
        }

        private float GetAddativeModifier(Stat stat)
        {
            if (!useModifiers) return 0;

            float total = 0;

            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAddativeModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!useModifiers) return 0;

            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total = modifier;
                }
            }
            return total;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentEXP = experience.GetEXP();

            int levels = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int i = 1; i <= levels; i++)
            {
                float levelEXP = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, i);

                if (levelEXP > currentEXP)
                {
                    return i;
                }
            }

            return levels + 1;
        }
    }
}
