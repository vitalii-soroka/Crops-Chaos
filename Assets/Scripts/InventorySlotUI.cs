using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image baseImage;
    [SerializeField] private Image itemImage;


    public void Start()
    {
        if (baseImage == null && gameObject.TryGetComponent<Image>(out var imageComponent))
        {
            baseImage = imageComponent;
        }
    }

    public void SetAmount(int number)
    {
        amountText.text = number.ToString();
    }
    public void SetItemImage(Sprite sprite)
    {
        if (itemImage != null && sprite != null)
        {
            itemImage.sprite = sprite;
        }
    }
    public void SetItemImage(Image newImage)
    {
        if (newImage != null)
        {
            itemImage = newImage;
        }
    }

    public void MakeItemImageVisible()
    {
        Color tempColor = itemImage.color;
        tempColor.a = 1f; // Set alpha to 1 (fully visible)
        itemImage.color = tempColor;
    }

    public void MakeImageInvisible(Image image)
    {
        Color tempColor = image.color;
        tempColor.a = 0f; // Set alpha to 0 (invisible)
        image.color = tempColor;
    }

    public void MakeImageVisible(Image image)
    {
        Color tempColor = image.color;
        tempColor.a = 1f; // Set alpha to 1 (fully visible)
        image.color = tempColor;
    }
}
