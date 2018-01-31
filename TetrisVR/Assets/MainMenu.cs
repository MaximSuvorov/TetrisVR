using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TetrisTools;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;

    public Button NewGameBtn;
    public Button ExitBtn;
    public GameObject MainGameCanvas;


	// Use this for initialization
	void Start () {
        NewGameBtn = transform.FindChild("NewGame").gameObject.GetComponent<Button>();
        ExitBtn = transform.FindChild("Exit").gameObject.GetComponent<Button>();
        NewGameBtn.onClick.AddListener(OnNewGameClick);
        ExitBtn.GetComponent<Button>().onClick.AddListener(OnExitClick);
        MainGameCanvas = GameObject.Find("MainGameCanvas");
        GameStateMachine.Instance.SwitchToMainMenu();
    }

    // Update is called once per frame
    void Update () {

    }

    void OnNewGameClick ()
    {
        GameStateMachine.Instance.SwitchToGame();
        GameScoreTable.Instance.scoreList.CheckScore(101, "test");
        GameScoreTable.Instance.SaveTable();
    }

    void OnExitClick()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
