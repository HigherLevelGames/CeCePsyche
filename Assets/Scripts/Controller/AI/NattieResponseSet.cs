using UnityEngine;
using System.Collections;

public class NattieResponseSet : ResponseSet {

	public override void Respond(ItemActions action)
    {
        switch (action)
        {
            case ItemActions.SqueakyToy:
                // DOG BARKS
                break;
        }
    }
}
