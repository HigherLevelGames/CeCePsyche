using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Sprite
        sprite;
    [HideInInspector]
    public ItemActions
        action;

    public void Use()
    {
        GameObject o = null;
        switch (action)
        {
            case ItemActions.Nothing:
                // this item does nothing!
                break;
            case ItemActions.DogBone:
                break;
            case ItemActions.Dynamite:
                break;
            case ItemActions.SqueakyToy:
                o = Instantiate(SpawnableInventoryLibrary.data.SqueakyToy) as GameObject;
                break;
            case ItemActions.Squirrel:
                break;
            case ItemActions.StinkyPerfume:
                break;
            case ItemActions.ZapNectar:
                break;
            default:
                break;
        }
        if (o != null)
        {
            o.transform.position = SpawnableInventoryLibrary.data.SpawnTransform.position;
            FlyingTextManager.data.SpawnTextAt(o.transform.position, "Stimulus");
        }
    }

    public void SetData(ItemInfo info)
    {
        action = info.action;
        sprite = info.menusprite;
        gameObject.GetComponent<Image>().sprite = sprite;
    }
}

