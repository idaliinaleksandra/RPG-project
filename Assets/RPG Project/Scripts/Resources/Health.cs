using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;
using System;
using GameDevTV.Utils;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {

        float regenerationPercentage = 70;

        LazyValue <float> healthPoints;
        float maxHealth;

        bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();

            // if (healthPoints < 0)
            //    healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += UpdateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= UpdateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (healthPoints.value <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
            //else if (healthPoints > 0)
                //GetComponent<Animator>().SetTrigger("takeDamage");
        }

        private void UpdateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetCurrentHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health)); 
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();

            if (exp == null) return;

            exp.GainEXP(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }


        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value <= 0)
                Die();
        }

    }
}
