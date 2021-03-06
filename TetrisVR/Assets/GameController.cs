﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using TetrisCore;

namespace TetrisTools
{

    public static class GameControllsSettings
    {
        public static int MaxLeaderBoardSize = 5;
    }

    public class GameCellPoolPrefabFactory
    {
        public static GameObject GetMeshByType(CellTypes meshType)
        {
            string meshName;
            switch (meshType)
            {
                case CellTypes.baseType:
                    meshName = "NormalTetrisBrick";
                    break;
                case CellTypes.borderBrick:
                    meshName = "BorderBrick";
                    break;
                default:
                    meshName = "None";
                    Debug.LogError("Mesh type not found. Mesh type: " + meshType.ToString());
                    break;
            }
            if (string.Compare(meshName, "None", true) == 0)
            {
                return null;
            }
            else {
                return (GameObject)GameObject.Instantiate(Resources.Load(meshName, typeof(GameObject)));
            }
        }
    }

    public static class GameFieldViewModel
    {
        public static void ReDrawField(TetrisField field)
        {
            GameCellPool.Instance.ClearObjects();
            for (int i = 0; i < GameSettings.sizex; i++)
            {
                for (int j = 0; j < GameSettings.sizey; j++)
                {
                    GameObject obj;
                    obj = GameCellPool.Instance.GetFreeMesh(field.gameField[i, j].CellType);
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        obj.transform.position = new Vector3(i, GameSettings.sizey - j, 0);
                    }
                }
            }
        }

        public static void DrawFigure(TetrisFigure figure, int offsetx, int offsety)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!figure.gameFigure[i, j].IsCellEmpty())
                    {
                        GameObject obj;
                        obj = GameCellPool.Instance.GetFreeMesh(figure.gameFigure[i, j].CellType);
                        if (obj != null)
                        {
                            obj.SetActive(true);
                            obj.transform.position = new Vector3(figure.posx + offsetx + i, GameSettings.sizey - (j + figure.posy + offsety), 0);
                        }
                    }
                }
            }
        }
    }

    public class GameCellPool : MonoBehaviour
    {
        private static GameCellPool _instance;
        //private GameSettings _settings;

        public static GameCellPool Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    GameObject gm = GameObject.Find("SceneRoot");
                    if (gm != null)
                    {
                        _instance = gm.AddComponent<GameCellPool>();
                        _instance.InitPool();
                        return _instance;
                    }
                    else
                    {


                        _instance = new GameObject("SceneRoot").AddComponent<GameCellPool>();
                        _instance.InitPool();
                        return _instance;
                    }
                }
            }
        }

        private Dictionary<int, List<GameObject>> objectPoolFree = new Dictionary<int, List<GameObject>>();
        private Dictionary<int, List<GameObject>> objectPollInUse = new Dictionary<int, List<GameObject>>();


        public GameObject GetFreeMesh(CellTypes meshType)
        {
            //Since PullSize = rows*coloms+4
            //We always have FreeObj 
            GameObject obj = null;
            if (Instance.objectPoolFree.ContainsKey((int)meshType))
            {
                int lastIndex = Instance.objectPoolFree[(int)meshType].Count - 1;
                obj = Instance.objectPoolFree[(int)meshType][lastIndex];
                Instance.objectPoolFree[(int)meshType].RemoveAt(lastIndex);
                if (Instance.objectPollInUse.ContainsKey((int)meshType))
                {
                    Instance.objectPollInUse[(int)meshType].Add(obj);
                }
                else
                {
                    Instance.objectPollInUse.Add((int)meshType, new List<GameObject>());
                    Instance.objectPollInUse[(int)meshType].Add(obj);
                }
            }
            return obj;
        }

        public void AddFreeObject(CellTypes meshType, GameObject mesh)
        {
            if (!Instance.objectPoolFree.ContainsKey((int)meshType))
            {
                Instance.objectPoolFree.Add((int)meshType, new List<GameObject>());
            }
            mesh.name = meshType.ToString() + Instance.objectPoolFree[(int)meshType].Count.ToString();
            mesh.SetActive(false);
            Instance.objectPoolFree[(int)meshType].Add(mesh);
        }

        private void FillFreePool(CellTypes meshType)
        {
            for (int i = 0; i < GameSettings.sizex * GameSettings.sizey; i++)
            {
                GameObject obj = GameCellPoolPrefabFactory.GetMeshByType(meshType);
                if (obj != null)
                {
                    Instance.AddFreeObject(meshType, obj);
                }
            }
        }

        private void InitPool()
        {
            //foreach (CellTypes meshType in Enum.GetValues(typeof(CellTypes)))
            //{
            //    if (meshType > 0) 
            //    {
            Instance.FillFreePool(CellTypes.baseType);
            //    }
            //}
        }

        public int GetPoolSize()
        {
            int count = 0;
            foreach (int key in Instance.objectPoolFree.Keys)
            {
                count += Instance.objectPoolFree[key].Count;
            }
            return count;
        }

        void OnApplicationQuit()
        {
            foreach (List<GameObject> list in Instance.objectPoolFree.Values)
            {
                list.Clear();
            }
        }

        public void ClearObjects()
        {
            foreach (CellTypes meshType in Enum.GetValues(typeof(CellTypes)))
            {
                if (Instance.objectPollInUse.ContainsKey((int)meshType))
                {
                    foreach (GameObject obj in Instance.objectPollInUse[(int)meshType])
                    {
                        AddFreeObject(meshType, obj);
                    }
                    Instance.objectPollInUse[(int)meshType].Clear();
                }
            }
        }
    }

    public enum GameStates
    {
        mainmenu, 
        playgame,
        pausegame,
        replaygame
    }

    public class GameStateMachine : MonoBehaviour
    {
        private static GameStateMachine _instance;
        private GameStates _state; 

        public GameStates CurState
        {
            get
            {
                return Instance._state;
            }
        }

        public GameObject MainMenuObj;
        public GameObject GameMenuObj;
        public GameObject PauseMenuObj;
        public GameObject ReplayMenuObj;
        //private GameSettings _settings;

        public static GameStateMachine Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    GameObject gm = GameObject.Find("SceneRoot");
                    if (gm != null)
                    {
                        _instance = gm.AddComponent<GameStateMachine>();
                        _instance.InitStateMachine();
                        return _instance;
                    }
                    else
                    {


                        _instance = new GameObject("SceneRoot").AddComponent<GameStateMachine>();
                        _instance.InitStateMachine();
                        return _instance;
                    }
                }
            }
        }

        public void InitStateMachine()
        {
            Instance.MainMenuObj = GameObject.Find("MainMenuCanvas");
            Instance.GameMenuObj = GameObject.Find("MainGameCanvas");
            Instance.PauseMenuObj = GameObject.Find("MenuPaused");
            Instance.ReplayMenuObj = GameObject.Find("MenuLostGame");
        }

        public void SwitchToMainMenu()
        {
            Instance.MainMenuObj.SetActive(true);
            Instance.GameMenuObj.SetActive(false);
            Instance.PauseMenuObj.SetActive(false);
            Instance.ReplayMenuObj.SetActive(false);

            Instance._state = GameStates.mainmenu;
        }

        public void SwitchToGame()
        {
            Instance.MainMenuObj.SetActive(false);
            Instance.GameMenuObj.SetActive(true);
            Instance.PauseMenuObj.SetActive(false);
            Instance.ReplayMenuObj.SetActive(false);

            if (Instance._state==GameStates.replaygame)
            {
                Instance.GameMenuObj.GetComponent<MainGameMenu>().tetris.ResetGame();
            }
            Instance._state = GameStates.playgame;
        }

        public void SwitchToPause()
        {
            Instance.MainMenuObj.SetActive(false);
            Instance.GameMenuObj.SetActive(false);
            Instance.PauseMenuObj.SetActive(true);
            Instance.ReplayMenuObj.SetActive(false);

            Instance._state = GameStates.pausegame;
        }

        public void SwitchToReplay()
        {
            Instance.MainMenuObj.SetActive(false);
            Instance.GameMenuObj.SetActive(false);
            Instance.PauseMenuObj.SetActive(false);
            Instance.ReplayMenuObj.SetActive(true);

            Instance._state = GameStates.replaygame;
            GameScoreTable.Instance.scoreList.CheckScore(TetrisPlayerModel.Instance.score, DateTime.Now.ToLongDateString());
        }

    }

    public class GameController : MonoBehaviour
    {

        private static GameController _instance;

        public static GameController Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    return new GameObject("SceneRoot").AddComponent<GameController>();
                }
            }
        }
        // Use this for initialization
        void Start()
        {
            _instance = this;
        }

    }

    public class GameScore
    {
        public int score;
        public string name;

        public GameScore()
        {
            score = 0;
            name = string.Empty;
        }

        public GameScore(int Score, string Name)
        {
            score = Score;
            name = Name;
        }
    }

    public class GameScoreList
    {
        public List<GameScore> scoreList = new List<GameScore>();

        public void CheckScore(int newScore, string newName)
        {
            newScore = UnityEngine.Random.Range(1, 100) * 10;
            for (int i = 0; i < scoreList.Count; i++)
            {
                if (newScore > scoreList[i].score)
                {
                    for (int j = scoreList.Count-1; j > i; j--)
                    {
                        scoreList[j - 1] = scoreList[j];  
                    }
                    scoreList[i] = new GameScore(newScore, newName);
                    break;
                }
            }
        }
    }

    public class GameScoreTable : MonoBehaviour
    {
        private static GameScoreTable _instance;
        public GameScoreList scoreList;

        public static GameScoreTable Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    GameObject gm = GameObject.Find("SceneRoot");
                    if (gm != null)
                    {
                        _instance = gm.AddComponent<GameScoreTable>();
                        _instance.InitGameScoreTable();
                        return _instance;
                    }
                    else
                    {


                        _instance = new GameObject("SceneRoot").AddComponent<GameScoreTable>();
                        _instance.InitGameScoreTable();
                        return _instance;
                    }
                }
            }
        }

        public void InitGameScoreTable()
        {
            Instance.scoreList = new GameScoreList();
            Instance.LoadTable();
        }

        public void LoadTable ()
        {
            // Using a FileStream, create an XmlTextReader.
            string filename = Application.dataPath + "/Game/ScoreTable.xml";
            Stream fs = new FileStream(filename, FileMode.Open);
            XmlReader reader = new XmlTextReader(fs);
            XmlSerializer serializer = new XmlSerializer(typeof(GameScoreList));
            try
            {
                Instance.scoreList = (GameScoreList)serializer.Deserialize(reader);
            }
            catch (XmlException e)
            {
                //throw;
                //Debug.Log("New table was created. Reason: "+Environment.NewLine+e.Message);
                for (int i = 0; i < GameControllsSettings.MaxLeaderBoardSize; i++)
                {
                    Instance.scoreList.scoreList.Add(new GameScore(0, string.Empty));
                }                
            }
            fs.Close();
        }

        public void SaveTable ()
        {
            XmlSerializer xml = new XmlSerializer(typeof(GameScoreList));
            string path = Application.dataPath + "/Game/ScoreTable.xml";
            File.SetAttributes(path, FileAttributes.Normal);
            TextWriter stream = new StreamWriter(path);
            xml.Serialize(stream, Instance.scoreList);
            stream.Close();
        }

    }
}
