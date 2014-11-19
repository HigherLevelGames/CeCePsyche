using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour 
{
	public void PlayAudioOnce()
	{
		audio.PlayOneShot (audio.clip);
	}
}
