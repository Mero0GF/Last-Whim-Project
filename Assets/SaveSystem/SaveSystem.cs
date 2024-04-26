using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveSpawnPosition(PlayerCheckpoint position)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerSpawnPosition.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        SpawnPositionData data = new SpawnPositionData(position);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Arquivo de Save salvado em: " + path);
    }

    public static SpawnPositionData LoadSpawnPosition(PlayerCheckpoint position)
    {
        string path = Application.persistentDataPath + "/playerSpawnPosition.bin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SpawnPositionData data = formatter.Deserialize(stream) as SpawnPositionData;
            stream.Close();
            Debug.Log("Arquivo de Save carregado em: " + path);
            return data;
            
        }
        else
        {
            Debug.LogError("Arquivo de Save não foi encontrado em: " + path);
            return null;
        }
    }

}
