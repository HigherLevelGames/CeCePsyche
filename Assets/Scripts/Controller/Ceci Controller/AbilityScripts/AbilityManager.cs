using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Happiness))]
[RequireComponent(typeof(Sadness))]
[RequireComponent(typeof(Fear))]
[RequireComponent(typeof(Anger))]
public class AbilityManager : MonoBehaviour
{
	#region Variables: CurEmotion, abilities, isUsingAbility, ActiveTime, ElapsedTime
	public enum Emotion
	{
		Happy,
		Sad,
		Scared, // Afraid/Fear
		Mad, // Angry
		Neutral,
	}
	private Emotion CurEmotion = Emotion.Neutral;
	private Ability[] abilities;
	private bool isUsingAbility = false;

	public float ActiveTime = 5.0f; // time until current ability turns off, 5 seconds?
	private float ElapsedTime = 0.0f; // time elapsed since used ability
	#endregion

	#region Public Properties: isCrying, canBreak, isFloating, isFrightened
	public bool isNeutral
	{
		get
		{
			return CurEmotion == Emotion.Neutral;
		}
	}

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

	// flag for CeciAnimControl.cs
	public bool isFloating
	{
		get
		{
			return isUsingAbility && (CurEmotion == Emotion.Happy);
		}
	}

	// flag for CeciAnimControl.cs
	public bool isFrightened
	{
		get
		{
			return isUsingAbility && (CurEmotion == Emotion.Scared);
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
		if(RebindableInput.GetKeyDown("Ability"))
		{
			if(isUsingAbility)
			{
				EndAbility();
			}
			else
			{
				UseAbility();
			}
		}

		/*
		if(isUsingAbility)
		{
			ElapsedTime += Time.deltaTime;
			if(ElapsedTime > ActiveTime) // time runs out
			{
				Neutralize();
			}
		}//*/
	}

	// set/called by Subconcious.cs somehow...
	public void SetEmotion(Emotion emotion)
	{
		if(isUsingAbility) // already using an ability
		{
			return;
		}
		CurEmotion = emotion;
		UseAbility();
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
		this.SendMessage("TriggerEmotionAnim", ((int)CurEmotion), SendMessageOptions.DontRequireReceiver);
		abilities[((int)CurEmotion)%abilities.Length].UseAbility();
		isUsingAbility = true;
	}

	void EndAbility()
	{
		if(CurEmotion == Emotion.Scared)
		{
			this.SendMessage("TriggerEmotionAnim", ((int)CurEmotion), SendMessageOptions.DontRequireReceiver);
		}
		// not really a good idea to use mod...
		abilities[((int)CurEmotion)%abilities.Length].EndAbility();
		isUsingAbility = false;
		ElapsedTime = 0.0f;
	}

	// reset everything
	void Neutralize()
	{
		if(CurEmotion == Emotion.Scared)
		{
			this.SendMessage("TriggerEmotionAnim", ((int)CurEmotion), SendMessageOptions.DontRequireReceiver);
		}
		// not really a good idea to use mod...
		abilities[((int)CurEmotion)%abilities.Length].EndAbility();
		CurEmotion = Emotion.Neutral;
		isUsingAbility = false;
		ElapsedTime = 0.0f;
	}
}
