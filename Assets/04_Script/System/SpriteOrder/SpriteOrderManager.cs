using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderManager : MonoBehaviour
{
    public static SpriteOrderManager Instance { get; private set; }
    private event Action OnSetMovingSprites;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    private void LateUpdate()
    {
        SetMovingSprite();
    }

    private void SetMovingSprite()
    {
        OnSetMovingSprites?.Invoke();
    }

    public void RegisterMovingSprite(MovingSpriteOrder movingSpriteOrder)
    {
        OnSetMovingSprites += movingSpriteOrder.SetSprite;
    }
    public void DestroyMovingSprite(MovingSpriteOrder movingSpriteOrder)
    {
        OnSetMovingSprites -= movingSpriteOrder.SetSprite;
    }

}
