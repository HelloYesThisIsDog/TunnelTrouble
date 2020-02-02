﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInfo : MonoBehaviour
{
    public Image Infographic1;
    public Image Infographic2;

    private void Start()
    {
        // start paused because of infographic
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Infographic1.enabled)
            {
                Infographic1.enabled = false;
                Infographic2.enabled = false;

                if (!GameManager.Get().m_GameEnded)
                {
                    Time.timeScale = 1.0f;
                }
            }
            else
            {
				Infographic1.enabled = true;
				Infographic2.enabled = true;

				Time.timeScale = 0.0f;
            }

        }
    }
}
