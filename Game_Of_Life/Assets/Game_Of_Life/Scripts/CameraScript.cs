using System;
using System.Collections;
using System.Collections.Generic;
using Game_Of_Life.Scripts;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private void Start()
    {
        GameEvents.Current.onStartUserInput += AdjustCameraOrthographicSize;
    }

    private void AdjustCameraOrthographicSize(int gridHeight)
    {
        GetComponent<Camera>().orthographicSize = (gridHeight + 0.5f) / 2;
        float position = (gridHeight / 2) - 0.5f;
        //transform.position = new Vector3(gridSize.x / 2, transform.position.y, transform.position.z);
        transform.position = new Vector3(position, position, transform.position.z);
    }
}
