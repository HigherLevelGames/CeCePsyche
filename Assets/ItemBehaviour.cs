using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{
    [HideInInspector] public GameObject Prefab;
    [HideInInspector] public ItemActions Action;
    public void Use()
    {
        switch (Action)
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
        Action = info.action;
        Prefab = info.prefab;
    }
}

