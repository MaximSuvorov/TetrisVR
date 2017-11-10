using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TetrisTools;

public class MenuPaused : MonoBehaviour {
    public Button ExitBtn;
    public Button ContineBtn;

    // Use this for initialization
    void Awake()
    {
        ContineBtn = transform.FindChild("ContineBtn").gameObject.GetComponent<Button>();
        ExitBtn = transform.FindChild("Exit").gameObject.GetComponent<Button>();
        ContineBtn.onClick.AddListener(OnContinueClick);
        ExitBtn.onClick.AddListener(OnExitClick);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnExitClick()
    {
        Application.Quit();
    }

    void OnContinueClick()
    {
        GameStateMachine.Instance.SwitchToGame();
    }
}
