using UnityEngine;

//This could not be incorporated into the RangeFinder script because OnPostRender() only works when it is on a camera target.

public class SessionPhotos : MonoBehaviour
{
    public RangeFinder rangeFinder;

    void Awake()
    {
        rangeFinder = GetComponentInParent<RangeFinder>();
    }

    void OnPostRender()
    {
        if (rangeFinder.grabScreenshot == true)
        {
            rangeFinder.tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            rangeFinder.tex.Apply();
            rangeFinder.preview.material.mainTexture = rangeFinder.tex;
            rangeFinder.grabScreenshot = false;
        }
    }
}