using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveStateManager : MonoBehaviour
{
    public static SaveStateManager data;
    [HideInInspector]
    public PlayerDataCheckin
        CeciData;
    [SerializeField]
    private string
        CheckpointPath = "/checkpoint.ccd";

    void Awake()
    {
        if (data == null)
            data = this;
    }

    void OnLevelWasLoaded(int level)
    {

    }

    public void SaveCheckpoint()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + CheckpointPath;
        FileStream file = File.Open(path, FileMode.OpenOrCreate);
        CheckpointData cpd = new CheckpointData();
        cpd.level = Application.loadedLevel;
        cpd.playerlocation = CeciData.PlayerTransform.position.ToVector2();
        int[] items = new int[CeciData.inventory.Items.Count];
        for (int i = 0; i < items.Length; i++)
            items [i] = (int)CeciData.inventory.Items [i];
        cpd.Items = items;
        bf.Serialize(file, cpd);
        file.Close();
        Debug.Log("Checkpoint at " + path);
    }

    public void LoadLastCheckpoint()
    {
        string path = Application.persistentDataPath + CheckpointPath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            CheckpointData cpd = (CheckpointData)bf.Deserialize(file);

            file.Close();
            if (cpd.level == Application.loadedLevel)
            {
                Game.Ceci.GetComponent<PlayerDataCheckin>().LoadIn(cpd);   
                Debug.Log("Loaded " + path);
            } else 
                Application.LoadLevel(cpd.level);

        }
    }
}

[Serializable]
public class CheckpointData
{
    public int level;
    public Vector2 playerlocation;
    public int[] Items;
}
