using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private SpriteAtlas spriteAtlas;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
     
        if (spriteAtlas == null) Debug.LogWarning("No SpriteAtlas assigned to SpriteSwapperComponent.");
    }

    public void ChangeSprite(string spriteName)
    {
        if (_spriteRenderer == null) return; 

        if (spriteAtlas == null)
        {
            Debug.LogWarning("SpriteAtlas is missing.");
            return;
        }

        Sprite newSprite = spriteAtlas.GetSprite(spriteName);
        if (newSprite != null)
        {
            _spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning($"Sprite '{spriteName}' not found in the SpriteAtlas.");
        }
    }
}
