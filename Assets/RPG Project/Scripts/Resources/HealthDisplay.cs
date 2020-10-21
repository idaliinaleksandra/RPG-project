using RPG.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Fighter fighter;

        [SerializeField] Text playerHPText;
        [SerializeField] Text enemyHPText;

        private void Awake()
        {
            health = GameObject.Find("Player").GetComponent<Health>();
            fighter = GameObject.Find("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            playerHPText.text = String.Format("PLAYER HP: {0:0} / {1:0}", health.GetCurrentHealthPoints(), health.GetMaxHealthPoints());

            if (fighter.GetTarget() != null && fighter.GetTarget().GetCurrentHealthPoints() != 0f)
                enemyHPText.text = String.Format("ENEMY HP: {0:0} / {1:0}", fighter.GetTarget().GetCurrentHealthPoints(), fighter.GetTarget().GetMaxHealthPoints());
            else
                enemyHPText.text = "";
        }
    }
}
