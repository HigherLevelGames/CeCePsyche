using UnityEngine;
using System.Collections;

public class SpawnableInventoryManager : MonoBehaviour
{

		public static SpawnableInventoryManager data;
		public GameObject SqueakyToy;
		public Transform SpawnPoint;

		void Awake ()
		{
				if (data == null)
						data = this;

		}

		void OnLevelWasLoaded (int level)
		{
				SpawnPoint = FindObjectOfType<PlayerInput> ().gameObject.transform;
		}
}
