using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TetrisCore;
using TetrisTools;

public class MainGameMenu : MonoBehaviour {

    private List<GameObject> borderObj = new List<GameObject>();
    public float lastProgress = 0;
    public TetrisField tetris;

    public Button DebugLogStateBtn;
    public Button DebugLogMakeTurnBtn;
    public Button DoRotateBtn;
    public Button DoMoveRightBtn;
    public Button DoMoveLeftBtn;

    public void DrawMenuRect()
    {
        
        
        
        //cols:
        for (int x=-1; x<GameSettings.sizex+1; x++)
        {
            GameObject obj;
            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType(CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(x, -1, 0);

            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType(CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(x, GameSettings.sizey, 0);
        }

        //rows: 
        for (int y = 0; y < GameSettings.sizey; y++)
        {
            GameObject obj;
            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType(CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(-1, y, 0);

            obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType(CellTypes.borderBrick);
            borderObj.Add(obj);
            obj.transform.position = new Vector3(GameSettings.sizex, y, 0);
        }
    }

    // Use this for initialization
    void Start () {
        Debug.Log("MainGameMenu.Start()");
        DrawMenuRect();
        this.DebugLogMakeTurnBtn = transform.FindChild("DoTurn").gameObject.GetComponent<Button>();
        this.DebugLogStateBtn = transform.FindChild("DoDebugLog").gameObject.GetComponent<Button>();
        DoMoveLeftBtn = transform.FindChild("DoMoveLeft").gameObject.GetComponent<Button>();
        DoMoveRightBtn = transform.FindChild("DoMoveRight").gameObject.GetComponent<Button>();
        DoRotateBtn = transform.FindChild("DoRotate").gameObject.GetComponent<Button>();
        DebugLogMakeTurnBtn.onClick.AddListener(DebugDoTurnClick);
        DebugLogStateBtn.onClick.AddListener(DebugDoLogClick);

        //
        DoMoveLeftBtn.onClick.AddListener(DoMoveLeft);
        DoMoveRightBtn.onClick.AddListener(DoMoveRight);
        DoRotateBtn.onClick.AddListener(DoRotate);

        tetris = new TetrisField(GameCellPool.Instance);
    }
	
	// Update is called once per frame
	void Update () {
        //exit;

        lastProgress += Time.deltaTime;
        if (false) {
            tetris.DoControllProgress(TetrisControllMove.rotate);
            tetris.DoGameTickProgress();
            lastProgress = 0;
        }
        //tetris.DrawField();
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
        if (tetris==null)
        {
            tetris = new TetrisField(GameCellPool.Instance);
        }
    }

    void DebugDoTurnClick()
    {
        //tetris.DoControllProgress(TetrisControllMove.rotate);
        tetris.DoGameTickProgress();
        tetris.DrawField();
    }

    void DebugDoLogClick()
    {
        tetris.DebugLogState();
    }

    void DoRotate()
    {
        tetris.DoControllProgress(TetrisControllMove.rotate);
    }

    void DoMoveLeft()
    {
        tetris.DoControllProgress(TetrisControllMove.left);
    }

    void DoMoveRight()
    {
        tetris.DoControllProgress(TetrisControllMove.right);
    }

}
