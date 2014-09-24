using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

namespace nTools
{
	public class ProjectEditor : MonoBehaviour 
	{
		static string[] basicFolderPaths = new string[] 
		{
			"Editor",
			"Game",
			"Game/Audio",
			"Game/Audio/Music",
			"Game/Audio/SFX",
			"Game/Prefabs",
			"Game/Rendering",
			"Game/Rendering/Animations",
			"Game/Rendering/Atlases",
			"Game/Rendering/Fonts",
			"Game/Rendering/Geometry",
			"Game/Rendering/Materials",
			"Game/Rendering/Shaders",
			"Game/Rendering/Substances",
			"Game/Rendering/Textures",
			"Game/Scenes",
			"Game/Scripts",
			"Plugins",
			"Resources",
			"Standard Assets",
			"Standard Assets/Editor",
			"Stock Assets", // Typically contains Unity Asset Store packages.
		};

		static int numOfFoldersCreated = 0;

		[MenuItem ("Tools/Create Basic Folders",false,0)]
		static void CreateBasicFolderStructure ()
		{
			numOfFoldersCreated = 0;

			Debug.Log("Creating the basic folder structure...");

			foreach (string path in basicFolderPaths)
			{
				CreateFolderPath(path);
			}

			if (numOfFoldersCreated == 0) Debug.Log("Basic folder structure already exists.");
			else Debug.Log(string.Format("Done: created {0} new folders.",numOfFoldersCreated));
		}

		static void CreateFolderPath (string path)
		{
			string[] dir = path.Split('/');
			string dataPath = Application.dataPath;

			for (int i = 0; i < dir.Length; i++)
			{
				dataPath += "/" + dir[i];

				if (!Directory.Exists(dataPath))
				{
					string parent = "Assets";

					for (int j = 0; j < i; j++)
					{
						parent += "/" + dir[j];
					}

					AssetDatabase.CreateFolder(parent,dir[i]);

					Debug.Log(string.Format("Created directory: {0}/{1}",parent,dir[i]));

					numOfFoldersCreated++;
				}
			}
		}
	}
}