using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;

        Health target;

        float damage = 0f;
        float speed = 1f;

        bool homing = false;

        private void Start()
        {
            transform.LookAt(GetAimLocation());

            Destroy(gameObject, maxLifeTime);
        }

        void Update()
        {
            if (target == null) return;

            if(homing)
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetValues(Health target, float damage, float speed, bool homing)
        {
            this.target = target;
            this.damage = damage;
            this.speed = speed;
            this.homing = homing;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();

            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(damage);

            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);

            Destroy(gameObject);
        }
    }
}
