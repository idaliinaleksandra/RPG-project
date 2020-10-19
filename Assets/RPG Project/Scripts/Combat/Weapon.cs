using RPG.Resources;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject prefab = null;
        [Space]
        [SerializeField] float damage;
        [SerializeField] float range = 2f;
        [Space]
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] bool leftHanded = false;
        [Header("Projectile")]
        [SerializeField] Projectile projectile = null;
        [SerializeField] float projectileSpeed = 0f;
        [SerializeField] bool homing = false;

        const string weaponName = "Weapon";

        public float Damage { get => damage; }
        public float Range { get => range; }
        public bool LeftHanded { get => leftHanded; }
        public float ProjectileSpeed { get => projectileSpeed; }

        public void Wield(Transform rightHand, Transform leftHand, Animator animator)
        {
            UnwieldOldWeapon(rightHand, leftHand);

            if (prefab != null)
            {
                GameObject wep = Instantiate(prefab, GetHand(rightHand, leftHand));
                wep.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
            else if(overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

        }

        private void UnwieldOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);

            if (oldWeapon == null)
                oldWeapon = leftHand.Find(weaponName);

            if (oldWeapon == null) return;

            oldWeapon.name = "Destroy";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHand(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetValues(target, instigator, damage, projectileSpeed, homing);
        }

        private Transform GetHand(Transform rightHand, Transform leftHand)
        {
            Transform hand;

            if (!leftHanded) hand = rightHand;
            else hand = leftHand;

            return hand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
    }
}
