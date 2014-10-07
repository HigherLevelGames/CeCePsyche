using UnityEngine;
using System.Collections;

public class DoggieStartle : MonoBehaviour {
	public int conditionState = 0;
	// 0 for unconditioned
	// 1 for being conditioned
	// 2 for is conditioned
	public float responseLvl;
	public bool isResponding;


	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(conditionState > 2)
		{
			conditionState = 0;
		}
		anim.SetInteger("ConditionState", conditionState);
	}

	void Respond()
	{

	}
}
