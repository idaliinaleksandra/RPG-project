using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float attackDamage = 10f;

        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon;

        private void Start()
        {
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

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
            float damage = attackDamage + currentWeapon.Damage;

            if (target == null) return;

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHand, leftHand, target);
            else
                target.TakeDamage(damage);
        }

        /// <summary>
        /// Animation event
        /// </summary>
        void FootR()
        {

        }

        /// <summary>
        /// Animation event
        /// </summary>
        void FootL()
        {

        }

        private bool GetInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < currentWeapon.Range;
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            var animator = GetComponent<Animator>();
            weapon.Wield(rightHand, leftHand, animator);
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

        public object CaptureState()
        {
            Debug.Log("HALOO " + currentWeapon.name);
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
