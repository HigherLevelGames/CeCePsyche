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
        switch (action)
        {
            case ItemActions.SqueakyToy:
				anim.SetTrigger("Bark");
                break;

			case ItemActions.DogBone:
			//Dog Salivates, eats bone and follows Ceci

				break;
        }
    }
}
