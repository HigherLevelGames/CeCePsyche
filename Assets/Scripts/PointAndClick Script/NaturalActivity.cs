using UnityEngine;
using System.Collections;

public class NaturalActivity : MonoBehaviour {
    public PointNClickGame Game;
    public GameObject PerformerOfActivity;
    public ItemActions Action = ItemActions.Squirrel;
    float timer = 3f;
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Game.PerformItemAction(0,0, Action);
            timer += 3;
        }
    }
}
