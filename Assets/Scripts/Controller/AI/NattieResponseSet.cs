using UnityEngine;
using System.Collections;

public class NattieResponseSet : ResponseSet {

	private Animator anim;

	void Start()
	{
		anim = this.GetComponent<Animator>();
	}
	void Update()
	{
		TimerUpdate();
	}

	public override void Respond(ItemActions action)
    {
		print ("RESPOND!");
        switch (action)
        {
            case ItemActions.SqueakyToy:
                // DOG BARKS
				//bark animation
			anim.SetTrigger("Bark");
                break;
        }
    }
}
