using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneMnagement
{
    
    public class Portal : MonoBehaviour
    {

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoimt;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 1f;

        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                StartCoroutine(Transition());

        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Sccene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            print("YKSI");
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return new WaitForSeconds(fadeWaitTime);
            print("YKSIKOLMAS");
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            print("YKSIPUOL");
            print("KAKSI");
            yield return fader.FadeIn(fadeInTime);
            print("KOLME");
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoimt.position);
            player.transform.rotation = otherPortal.spawnPoimt.rotation;

        }
    }
}
