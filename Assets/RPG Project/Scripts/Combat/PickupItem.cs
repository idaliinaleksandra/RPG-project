using RPG.Combat;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class PickupItem : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon;
        [SerializeField] bool respawn = false;

        Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (other.CompareTag(player.tag))
            {
                Pickup(player.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);

            if(!respawn)
                Destroy(gameObject);
            else
                StartCoroutine(HideForSeconds(5f));
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

        public CursorType GetCursorType()
        {
            return CursorType.Item;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }
    }
}
