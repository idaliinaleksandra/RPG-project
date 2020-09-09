using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponReage = 2f;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float attackDamage = 10f;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity; 

        private void Update()
        {

            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetInRange())
                GetComponent<Move>().MoveTo(target.transform.position, 1f);
            else
            {
                GetComponent<Move>().Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweenAttack)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        /// <summary>
        /// Animation event
        /// </summary>
        void Hit()
        {
            if (target != null)
                target.TakeDamage(attackDamage);
        }

        private bool GetInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < weaponReage;
        }

        public bool CanAttack(GameObject cTarget)
        {
            if (cTarget == null || gameObject.name == cTarget.gameObject.name) return false;
            Health targetToTest = cTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
