using RPG.Core;
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

        public float Damage { get => damage; }
        public float Range { get => range; }
        public bool LeftHanded { get => leftHanded; }
        public float ProjectileSpeed { get => projectileSpeed; }

        public void Wield(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (prefab != null)
            {
                Instantiate(prefab, GetHand(rightHand, leftHand));
            }

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHand(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetValues(target, damage, projectileSpeed);
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
