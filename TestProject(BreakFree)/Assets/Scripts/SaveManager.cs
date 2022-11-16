using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData AutoSave;

    public bool hasLoaded;

    private void Awake()
    {
        Instance = this;
        Load();
    }

    public void Save()
    {
        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/" + AutoSave.SaveName + ".save", FileMode.Create);
        serializer.Serialize(stream, AutoSave);
        stream.Close();

    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;

        if(System.IO.File.Exists(dataPath + "/" + AutoSave.SaveName + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/" + AutoSave.SaveName + ".save", FileMode.Open);
            AutoSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();
        }

        hasLoaded = true;
    }
}

[System.Serializable]
public class SaveData
{
    public string SaveName;

    public float Money;

    public int[] CurrentUnitForLoad; 
    public Vector3[] RespawnPositionUnitForLoad;
    public int[] CurrentLevelAmplifiers;
    public string LastData;
    public float LastIdleDamage;
}