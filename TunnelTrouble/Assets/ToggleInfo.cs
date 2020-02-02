using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInfo : MonoBehaviour
{
    public Transform Infographic;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Infographic.gameObject.activeSelf)
            {
                Infographic.gameObject.SetActive(false);
            }
            else
            {
                Infographic.gameObject.SetActive(true);

            }

        }
    }
}
