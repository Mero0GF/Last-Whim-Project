using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.CompilerServices;


//script que faz o save e load dos arquivos
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private Stream stream;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullpath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;
        if(File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullpath + "\n" + e);
            }
        }
        return loadedData;

    }

    public void Save(GameData data)
    {
        string fullpath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            //cria o diretorio caso nao exista
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            //string para armazenar os dados
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        { 
            Debug.LogError("Error occured when trying to save data to file: " + fullpath + "\n" + e);
        }
    }
}
