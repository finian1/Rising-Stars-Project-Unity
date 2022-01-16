using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Segment : MonoBehaviour
{
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    public void Fill()
    {
        GetComponent<Image>().sprite = activeSprite;
    }

    public void Empty()
    {
        GetComponent<Image>().sprite = inactiveSprite;
    }
}
