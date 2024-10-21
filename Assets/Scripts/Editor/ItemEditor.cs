using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    return;

    //    //DrawDefaultInspector();

    //    //Item item = (Item)target;

    //    //switch (item.type)
    //    //{
    //    //    case Item.ItemType.Seed:
    //    //        item.cropPrefab 
    //    //            = (GameObject)EditorGUILayout.ObjectField("Crop Prefab", item.cropPrefab, typeof(GameObject), true);
    //    //        break;

    //    //    case Item.ItemType.Crop:
    //    //        ClearReferences(item, Item.ItemType.Crop);

    //    //        break;

    //    //    case Item.ItemType.None:
    //    //        ClearReferences(item, Item.ItemType.None);

    //    //        break;
    //    //}

    //    //if (GUI.changed)
    //    //{
    //    //    EditorUtility.SetDirty(item);
    //    //}
    //}

    //public void ClearReferences(Item item, Item.ItemType type)
    //{
    //    switch (item.type)
    //    {
    //        case Item.ItemType.Seed:
                
    //            break;

    //        case Item.ItemType.Crop:

    //            item.cropPrefab = null;

    //            break;

    //        case Item.ItemType.None:

    //            item.cropPrefab = null;

    //            break;
    //    }
    //}
}
