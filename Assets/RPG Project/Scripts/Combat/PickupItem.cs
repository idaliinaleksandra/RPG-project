using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] bool respawn = false;

        Collider collider;

        private void Start()
        {
            collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (other.CompareTag(player.tag))
            {
                player.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);

                if (respawn)
                    StartCoroutine(HideForSeconds(5f));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            collider.enabled = show;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(show);
            }
        }
    }
}
