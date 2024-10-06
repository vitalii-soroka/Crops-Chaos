using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private GameObject crop;

    public void Plant(GameObject cropPrefab)
    {
        if (cropPrefab == null || crop != null) return;

        crop = Instantiate(cropPrefab, this.transform, false);
        //if (crop)
        //{
        //    crop.transform.SetParent(this.transform, false);
        //}
    }

    public void Gather()
    {
        if(crop == null || !IsCropReady()) return;
        
        var dropComponent = crop.GetComponent<Dropable>();
        if(dropComponent) dropComponent.Drop();
        crop = null;
    }

    public void UnPlant()
    {
        if (crop != null)
        {
            // TODO 
            var cropComponent = crop.GetComponent<Crop>();
            if (cropComponent) cropComponent.BreakCrop();
        }

       crop = null;
    }

    public bool IsCropReady()
    {
        if (crop == null) return false;

        var cropComponent = crop.GetComponent<Crop>();

        return cropComponent && cropComponent.IsGrown();
    }

    public bool HasCrop() { return crop != null; }
}
