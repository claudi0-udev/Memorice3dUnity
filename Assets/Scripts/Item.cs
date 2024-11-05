using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int itemValue;

    public Sprite spriteImage;

    public GameObject box;
    public GameObject questionMark;
    public GameObject itemFrame;

    public SpriteRenderer spriteContainer;

    public bool canBeSelected = true;

    void Start()
    {
        itemFrame.SetActive(false);
        spriteContainer.gameObject.SetActive(false);
        spriteContainer.sprite = spriteImage;
        RandomColorItemFrame();
    }

    void RandomColorItemFrame()
    {
        MeshRenderer meshRenderer = itemFrame.GetComponent<MeshRenderer>();
        meshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
    
}
