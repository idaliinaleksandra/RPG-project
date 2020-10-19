using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {

        float healthPoints = -1f;
        float maxHealth;

        bool isDead = false;

        private void Start()
        {
            if (healthPoints < 0)
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
            //else if (healthPoints > 0)
                //GetComponent<Animator>().SetTrigger("takeDamage");
        }

        public float GetPercentage()
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

            return 100 * (healthPoints / maxHealth); 
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
            healthPoints = (float)state;

            if (healthPoints <= 0)
                Die();
        }

    }
}
