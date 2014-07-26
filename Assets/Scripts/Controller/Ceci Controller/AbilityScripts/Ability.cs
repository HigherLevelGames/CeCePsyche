using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
	public virtual void UseAbility()
	{
		Debug.Log ("Using Ability");
	}

	public virtual void EndAbility()
	{
		Debug.Log ("No longer using Ability");
	}
}