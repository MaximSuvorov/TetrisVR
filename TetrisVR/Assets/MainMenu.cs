using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TetrisTools;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;

    public GameObject NewGameBtn;
    public GameObject ExitBtn;
    public GameObject MainGameCanvas;


	// Use this for initialization
	void Start () {
        NewGameBtn = transform.FindChild("NewGame").gameObject;
        ExitBtn = transform.FindChild("Exit").gameObject;
        NewGameBtn.GetComponent<Button>().onClick.AddListener(OnNewGameClick);
        ExitBtn.GetComponent<Button>().onClick.AddListener(OnExitClick);
        MainGameCanvas = GameObject.Find("MainGameCanvas");
        MainGameCanvas.SetActive(false);
        Debug.Log("Inited");
        Debug.Log(GameCellPool.Instance.GetPoolSize().ToString());
    }

    // Update is called once per frame
    void Update () {

    }

    void OnNewGameClick ()
    {
        gameObject.SetActive(false);
        MainGameCanvas.SetActive(true);
        //MainGameCanvas.Draw
        //MainGameCanvas.DrawMenuRect();
    }

    void OnExitClick()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
