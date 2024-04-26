using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPositionData
{
    public float[] position;

    public SpawnPositionData(PlayerCheckpoint checkpoint)
    {
        position = new float[2];

        position[0] = checkpoint.transform.position.x;
        position[1] = checkpoint.transform.position.y;
    }
}
