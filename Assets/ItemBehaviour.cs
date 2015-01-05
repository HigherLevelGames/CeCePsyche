using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour
{
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public ItemActions action;
    public void Use()
    {
        switch (action)
        {
            case ItemActions.Nothing:
            
                break;
            case ItemActions.DogBone:
                break;
            case ItemActions.Dynamite:
                break;
            case ItemActions.SqueakyToy:
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
    }
    public void SetData(ItemInfo info)
    {
        action = info.action;
        sprite = info.sprite;
        gameObject.GetComponent<Image>().sprite = sprite;
    }
}

