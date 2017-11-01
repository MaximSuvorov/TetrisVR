using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TetrisCore;

public class MainGameMenu : MonoBehaviour {

    private List<GameObject> borderObj = new List<GameObject>();

    public void DrawMenuRect()
    {
        
        
        
        //cols:
        for (int x=-1; x<GameSettings.sizex+1; x++)
        {
            GameObject obj;
            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType((int)CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(x, -1, 0);

            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType((int)CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(x, GameSettings.sizey, 0);
        }

        //rows: 
        for (int y = 0; y < GameSettings.sizey; y++)
        {
            GameObject obj;
            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType((int)CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(-1, y, 0);

            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType((int)CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(GameSettings.sizex, y, 0);
        }
    }

    // Use this for initialization
    void Start () {
        DrawMenuRect();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationQuit()
    {
        borderObj.Clear();
    }

    void OnDisable()
    {
        foreach (GameObject obj in borderObj)
        {
            obj.SetActive(false);
        }
    }

    void OnEnable()
    {
        foreach (GameObject obj in borderObj)
        {
            obj.SetActive(true);
        }
    }
}
