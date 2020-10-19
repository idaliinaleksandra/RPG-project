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
            playerHPText.text = String.Format("PLAYER HP: {0:0.0}%", health.GetPercentage());

            if (fighter.GetTarget() != null && fighter.GetTarget().GetPercentage() != 0f)
                enemyHPText.text = String.Format("ENEMY HP: {0:0.0}%", fighter.GetTarget().GetPercentage());
            else
                enemyHPText.text = "";
        }
    }
}
