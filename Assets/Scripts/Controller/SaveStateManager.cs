using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveStateManager : MonoBehaviour
{
    public static SaveStateManager data;
    public int levelnum;
    public string levelname;
    
    public void GoToNextArea()
    {
        if (levelnum > -1)
        {
            Application.LoadLevel(levelnum);
            return;
        } else if (levelname != string.Empty)
        {
            Application.LoadLevel(levelname);
            return;
        }
        Debug.Log("The level was unable to be loaded due to impossible load name or number");
    }
    [HideInInspector]
    public bool loadQueued;
    public PlayerDataCheckin
        CeciData;
    [SerializeField]
    private string
        CheckpointPath = "/checkpoint.ccd";

    float timeout;
    CheckpointData queuedata;


    void Awake()
    {
        if (data == null)
            data = this;
        timeout = -1;
    }

    void Update()
    {
        if (timeout > 0)
            timeout -= Time.deltaTime;
    }

    public bool SaveCheckpoint()
    {
        bool ok = false;
        if (timeout < 0)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + CheckpointPath;
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            CheckpointData cpd = new CheckpointData();
            cpd.level = Application.loadedLevel;
            cpd.xlocation = CeciData.PlayerTransform.position.x;
            cpd.ylocation = CeciData.PlayerTransform.position.y;
            cpd.CollectFlags = InventoryManager.data.GetFlags();
            int[] items = new int[CeciData.inventory.Items.Count];
            for (int i = 0; i < items.Length; i++)
                items [i] = (int)CeciData.inventory.Items [i];
            cpd.Items = items;
            bf.Serialize(file, cpd);
            file.Close();
            Debug.Log("Checkpoint at " + path);
            timeout = 10;
            ok = true;
        }
        return ok;
    }

    public void LoadLastCheckpoint()
    {
        string path = Application.persistentDataPath + CheckpointPath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            CheckpointData cpd = (CheckpointData)bf.Deserialize(file);
            levelnum = cpd.level;
            ItemActions[] items = new ItemActions[cpd.Items.Length];
            for (int i = 0; i < items.Length; i++)
                items[i] = (ItemActions)cpd.Items [i];
            CollectedFlags[] flags = new CollectedFlags[cpd.CollectFlags.Length];
            for (int i = 0; i < flags.Length; i++)
                flags [i] = (CollectedFlags)cpd.CollectFlags [i];
            InventoryManager.data.ReplaceInventory(items, flags);
            file.Close();
            queuedata = cpd;
            loadQueued = true;
            CanvasManager.data.StartTransition();
            timeout = 10;
        }
    }

    public void LoadQueuedData()
    {
        if (loadQueued)
        {
            string path = Application.persistentDataPath + CheckpointPath;
            Game.Ceci.GetComponent<PlayerDataCheckin>().LoadIn(queuedata);
            Debug.Log("Loaded " + path);
            loadQueued = false;
        }
    }
}

[Serializable]
public class CheckpointData
{
    public int level;
    public float xlocation, ylocation;
    public int[] Items;
    public int[] CollectFlags;
}
