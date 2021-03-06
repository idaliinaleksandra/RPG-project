﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        PlayableDirector director;
        GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            director = GetComponent<PlayableDirector>();

            director.played += DisableControl;
            director.stopped += EnableControl;
        }

        private void OnDisable()
        {
            director = GetComponent<PlayableDirector>();

            director.played -= DisableControl;
            director.stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
            print("DisableControl");
        }

        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
            print("EnableControl");
        }
    }
}
