using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCanvas : MonoBehaviour
{
    //Canvas UI
    public Transform canvas;

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    //Disable and Enable Canvas
    public void DisableCanvas()
    {
        if (canvas.gameObject.activeInHierarchy == false)
        {
            canvas.gameObject.SetActive(true);
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
