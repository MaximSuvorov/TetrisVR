using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TetrisCore;
using TetrisTools;

public class MainGameMenu : MonoBehaviour {

    private List<GameObject> borderObj = new List<GameObject>();
    public float lastProgress = 0;
    private float controlProgress = 0;
    public TetrisField tetris;
    private TetrisControllMove curMove = TetrisControllMove.none;

    public Text scoreText;
    public Button PauseBtn;

    public void DrawMenuRect()
    {
        Debug.Log("Draw Rect");
        if (borderObj.Count == 0)
        {
            //cols:
            for (int x = -1; x < GameSettings.sizex + 1; x++)
            {
                GameObject obj;
                obj = TetrisTools.GameCellPoolPrefabFactory.GetMeshByType(CellTypes.borderBrick);
                borderObj.Add(obj);
                obj.transform.position = new Vector3(x, 0, 0);

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
    }

    // Use this for initialization
    void Awake() {
        Debug.Log("MainGameMenu.Start()");
        DrawMenuRect();

        scoreText = transform.FindChild("Score").gameObject.GetComponent<Text>();
        PauseBtn = transform.FindChild("PauseButton").gameObject.GetComponent<Button>();
        PauseBtn.onClick.AddListener(OnPauseClick);

        //this.DebugLogMakeTurnBtn = transform.FindChild("DoTurn").gameObject.GetComponent<Button>();
        //this.DebugLogStateBtn = transform.FindChild("DoDebugLog").gameObject.GetComponent<Button>();
        //DoMoveLeftBtn = transform.FindChild("DoMoveLeft").gameObject.GetComponent<Button>();
        //DoMoveRightBtn = transform.FindChild("DoMoveRight").gameObject.GetComponent<Button>();
        //DoRotateBtn = transform.FindChild("DoRotate").gameObject.GetComponent<Button>();
        //DebugLogMakeTurnBtn.onClick.AddListener(DebugDoTurnClick);
        //DebugLogStateBtn.onClick.AddListener(DebugDoLogClick);

        ////
        //DoMoveLeftBtn.onClick.AddListener(DoMoveLeft);
        //DoMoveRightBtn.onClick.AddListener(DoMoveRight);
        //DoRotateBtn.onClick.AddListener(DoRotate);

        tetris = new TetrisField();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameStateMachine.Instance.CurState == GameStates.playgame)
        {
            //exit;
            //progress input/keys: 
            //curMove = TetrisControllMove.none;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                curMove = TetrisControllMove.left;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                curMove = TetrisControllMove.right;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                curMove = TetrisControllMove.rotate;
            }

            //input from touch
            int fingerCount = 0;
            float posx = 0;
            float posy = 0;
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    posx += touch.position.x;
                    posy += touch.position.y;
                    fingerCount++;
                }
            }

            if (fingerCount > 0)
            {
                // posx /= fingerCount;
                // posy /= fingerCount;
                // posy = Screen.height - posy;

                //posx = (Screen.width / posx)*100;
                //posy = (Screen.width / posy)*100;

                //top left 130/1800
                //top right 900/1800
                //botton left 130/130
                //botton right 900/130

                Rect MoveLeftZone = new Rect(0, 300, 450, 400);
                Rect MoveRightZone = new Rect(1080 - 450, 300, 450, 400);
                //Rect MoveDownZone = new Rect(150, 0, 800, 200);
                Rect MoveRotate = new Rect(150, 1920 - 600, 800, 400);

                if (MoveLeftZone.Contains(Input.touches[0].position)) curMove = TetrisControllMove.left;
                if (MoveRightZone.Contains(Input.touches[0].position)) curMove = TetrisControllMove.right;
                if (MoveRotate.Contains(Input.touches[0].position)) curMove = TetrisControllMove.rotate;

                //scoreText.text = string.Format("NumFngr: {0} X: {1} Y: {2}", fingerCount, posx, posy); wtf?!
                //scoreText.text = string.Format("NumFngr: {0}", fingerCount) + Environment.NewLine + string.Format("X {0::0.##} Y {1:0.##}", posx, posy);
            }

            lastProgress += Time.deltaTime;
            controlProgress += Time.deltaTime;

            if (controlProgress > .15f)
            {
                tetris.DoControllProgress(curMove);
                controlProgress = 0;
                curMove = TetrisControllMove.none;
            }
            //
            if (lastProgress > 1.0f)
            {
                //tetris.DoControllProgress(TetrisControllMove.rotate);
                tetris.DoGameTickProgress();
                scoreText.text = string.Format("Score: {0}", TetrisPlayerModel.Instance.score);
                lastProgress = 0;
            }
            Rect MoveDownZone = new Rect(150, 0, 800, 200);
            if (((Input.GetKeyDown(KeyCode.DownArrow)) || ((fingerCount > 0) && ((MoveDownZone.Contains(Input.touches[0].position))))) && (controlProgress > .10f))
            {
                tetris.DoGameTickProgress();
                controlProgress = 0;
            }
            tetris.DrawField();
        }
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
            tetris = new TetrisField();
        }
    }

    void DebugDoTurnClick()
    {
        //tetris.DoControllProgress(TetrisControllMove.rotate);
        tetris.DoGameTickProgress();
        //tetris.DrawField();
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

    void OnPauseClick()
    {
        GameStateMachine.Instance.SwitchToPause();
    }
}
