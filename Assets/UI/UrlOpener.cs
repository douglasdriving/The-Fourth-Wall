using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlOpener : MonoBehaviour
{
    public string url = "https://yourwebsite.com";

    public void OpenLink()
    {
        Application.OpenURL(url);
    }
}
