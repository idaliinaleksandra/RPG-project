using UnityEngine;
using RPG.Stats;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();

            Experience exp = GetComponent<Experience>();

            if (exp != null)
                exp.onEXPGained += UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("LEVEL UP");
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
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
