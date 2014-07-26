using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
	#region Variables: CurEmotion, isUsingAbility, abilities, ActiveTime, ElapsedTime
	public enum Emotion
	{
		Happy,
		Sad,
		Scared, // Afraid/Fear
		Mad, // Angry
		Neutral,
	}
	Emotion CurEmotion = Emotion.Neutral;

	bool isUsingAbility = false;
	public Ability[] abilities;
	public float ActiveTime = 5.0f; // time until current ability turns off, 5 seconds?
	float ElapsedTime = 0.0f; // time elapsed since used ability
	#endregion

	#region Public Properties: isCrying, canBreak
	// flag for Plant.cs to know if Ceci is crying
	public bool isCrying
	{
		get
		{
			return isUsingAbility && (CurEmotion == Emotion.Sad);
		}
	}

	// flag for BreakableWall.cs to know if Ceci is raging
	public bool canBreak
	{
		get
		{
			return isUsingAbility && (CurEmotion == Emotion.Mad);
		}
	}
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		abilities = new Ability[4];
		abilities[0] = this.GetComponent<Happiness>();
		abilities[1] = this.GetComponent<Sadness>();
		abilities[2] = this.GetComponent<Fear>();
		abilities[3] = this.GetComponent<Anger>();
	}
	
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
			CurEmotion = Emotion.Sad;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			CurEmotion = Emotion.Happy;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			CurEmotion = Emotion.Scared;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			CurEmotion = Emotion.Mad;
		}
		//UseAbility();
	}

	// set/called by Subconcious.cs somehow...
	void SetEmotion(Emotion emotion)
	{
		if(isUsingAbility) // already using an ability
		{
			return;
		}
		CurEmotion = emotion;
		UseAbility();
	}

	void OnGUI()
	{
		GUI.Label(new Rect(100,0,100,50), "Current emotion: " + CurEmotion + "\nTime: " + ElapsedTime);
	}

	void UseAbility()
	{
		if(isUsingAbility || CurEmotion == Emotion.Neutral) // already using an ability
		{
			return;
		}
		// not really a good idea to use mod...
		abilities[((int)CurEmotion)%abilities.Length].UseAbility();
		isUsingAbility = true;
	}

	// reset everything
	void Neutralize()
	{
		// not really a good idea to use mod...
		abilities[((int)CurEmotion)%abilities.Length].EndAbility();
		CurEmotion = Emotion.Neutral;
		isUsingAbility = false;
		ElapsedTime = 0.0f;
	}
}
