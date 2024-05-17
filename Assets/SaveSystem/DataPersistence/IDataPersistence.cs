using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface para que outros scripts acessem os metodos de salvar e carregar
public interface IDataPersistence
{
    void LoadData(GameData gameData);
    void SaveData(GameData gameData);
}
