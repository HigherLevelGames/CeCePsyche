using UnityEngine;
using System.Collections;

// Model class to manage emotion data
// Currently contains functionality for using abilities pertaining to emotions
// Will move later to separate Ability class
[RequireComponent(typeof(JNNController))]
public class Emotion : MonoBehaviour
{
	#region Variables: CurrentEmotion, isUsingAbility, ActiveTime, ElapsedTime
	public enum CharacterState
	{
		Neutral,
		Sad,
		Happy,
		Scared, // Afraid/Fear
		Mad // Angry
	}
	CharacterState CurrentEmotion = CharacterState.Neutral;

	bool isUsingAbility = false;
	float ActiveTime = 5.0f; // time until current ability turns off, 5 seconds?
	float ElapsedTime = 0.0f; // time elapsed since used ability
	#endregion

	#region Public Properties: isCrying, canBreak
	// flag for Plant.cs to know if Ceci is crying
	public bool isCrying
	{
		get
		{
			return isUsingAbility && (CurrentEmotion == CharacterState.Sad);
		}
	}

	// flag for BreakableWall.cs to know if Ceci is raging
	public bool canBreak
	{
		get
		{
			return isUsingAbility && (CurrentEmotion == CharacterState.Mad);
		}
	}
	#endregion

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update ()
	{
		SetEmotion(); // Dev Shortcut
		if(Input.GetButtonDown("Ability"))
		{
			UseAbility();
		}

		if(isUsingAbility)
		{
			ElapsedTime += Time.deltaTime;
			if(ElapsedTime > ActiveTime) // time runs out
			{
				Neutralize();
			}
		}
	}

	// Dev Shortcut
	void SetEmotion()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			CurrentEmotion = CharacterState.Sad;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			CurrentEmotion = CharacterState.Happy;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			CurrentEmotion = CharacterState.Scared;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			CurrentEmotion = CharacterState.Mad;
		}
		//UseAbility();
	}

	// set by Subconcious somehow
	void SetEmotion(CharacterState emotion)
	{
		if(isUsingAbility) // already using an ability
		{
			return;
		}
		CurrentEmotion = emotion;
		UseAbility();
	}

	void UseAbility()
	{
		//* // will modify accordingly to design during beta phase
		if(isUsingAbility) // already using an ability
		{
			return;
		}
		// */
		isUsingAbility = true;

		Debug.Log ("Using Ability");
		switch(CurrentEmotion)
		{
		case CharacterState.Neutral:
			// nothing to do
			break;
		case CharacterState.Sad:
			Cry ();
			break;
		case CharacterState.Happy:
			Float ();
			break;
		case CharacterState.Scared:
			Shrink();
			break;
		case CharacterState.Mad:
			Rage();
			break;
		default:
			Debug.Log ("Warning: Unknown Character State");
			break;
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(100,0,100,50), "Current emotion: " + CurrentEmotion + "\nTime: " + ElapsedTime);
	}

	// reset everything
	void Neutralize()
	{
		ElapsedTime = 0.0f;
		isUsingAbility = false;

		// return to normal animation

		switch(CurrentEmotion)
		{
		case CharacterState.Neutral:
			// nothing to do
			break;
		case CharacterState.Sad:
			// nothing to do
			break;
		case CharacterState.Happy:
			// stop ability to float/fly
			break;
		case CharacterState.Scared:
			// unshrink
			this.transform.localScale = Vector3.one;
			break;
		case CharacterState.Mad:
			// nothing to do
			break;
		default:
			Debug.Log ("Warning: Unknown Character State");
			break;
		}
		CurrentEmotion = CharacterState.Neutral;
	}

	// Sad Ability
	void Cry()
	{
		// play crying anim
		// create tear particle FX
		// see Plant.cs
	}

	// Happy Ability
	void Float()
	{
		// play giggling anim
		// create happy particle FX
		// set flag in controller to have CeCi float in the air
	}

	// Fear Ability
	void Shrink()
	{
		// play frightened anim
		// create frightened particle FX
		// half transform size
		this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
	}

	// Anger Ability
	void Rage()
	{
		// play raging anim
		// create firey particle FX
		// program Deion's dash ability
		float speed = -10.0f;
		if(this.GetComponent<JNNController>().isFacingRight)
		{
			speed *= -1.0f;
		}
		this.rigidbody2D.velocity = new Vector2(speed, this.rigidbody2D.velocity.y);
	}
}
