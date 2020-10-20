using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool triggered;

        private void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                triggered = true;
            }
            else return;
        }

        public object CaptureState()
        {
            return triggered;
        }

        public void RestoreState(object state)
        { 
            triggered = (bool)state;
        }

    }
}
