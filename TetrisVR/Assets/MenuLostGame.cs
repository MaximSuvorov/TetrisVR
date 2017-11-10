using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TetrisTools;

public class MenuLostGame : MonoBehaviour {
    public Button ExitBtn;
    public Button RetryBtn;

    // Use this for initialization
    void Awake()
    {
        RetryBtn = transform.FindChild("RetryBtn").gameObject.GetComponent<Button>();
        ExitBtn = transform.FindChild("Exit").gameObject.GetComponent<Button>();
        RetryBtn.onClick.AddListener(OnReTryClick);
        ExitBtn.onClick.AddListener(OnExitClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnExitClick()
    {
        Application.Quit();
    }

    void OnReTryClick()
    {
        GameStateMachine.Instance.SwitchToGame();
    }
}

