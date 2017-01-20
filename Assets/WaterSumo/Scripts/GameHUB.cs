﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSumo
{
    /**
     * Central HUB for the game components.
     * */
    public class GameHUB : MonoBehaviour
    {

        public GameHUB Instance
        {
            get
            {
                if (sharedInstance == null)
                {
                    foreach (var hub in FindObjectsOfType<GameHUB>())
                    {
                        if (hub != null && hub.isShared)
                        {
                            sharedInstance = hub;
                        }
                    }
                }
                return sharedInstance;
            }
        }

        public GameObject NullModule;

        protected void Awake()
        {
            //Look if we are the shared one, and if not we destroy ourselves
            bool otherIsShared = false;
            foreach (var hub in FindObjectsOfType<GameHUB>())
            {
                if (hub != this & hub != null)
                {
                    otherIsShared = true;
                    break;
                }
            }

            if (otherIsShared)
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);

            sharedInstance = this;
            isShared = true;
        }

        private bool isShared = false;
        private static GameHUB sharedInstance = null;
    }
}