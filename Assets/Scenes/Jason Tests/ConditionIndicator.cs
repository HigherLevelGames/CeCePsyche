using UnityEngine;
using System.Collections;

public class ConditionIndicator : MonoBehaviour {
	public Animator anim;
	public enum Conditions {
		Conditioning = 0,
		Conditioned = 1,
		Unconditioned = 2

	}
	public Conditions Condition;
	void Start () {
		Condition = Conditions.Unconditioned;
		anim.SetInteger ("Condition", (int)Condition);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			int n = (int)(Condition);
			n = (n + 1) % 3;
			Condition = (Conditions)n;
			anim.SetInteger ("Condition", n);
				}

	}
}
