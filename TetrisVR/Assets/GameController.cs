using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TetrisCore;

namespace TetrisTools
{
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
            if (string.Compare(meshName, "None", true)==0) 
            {
                return null;
            }
            else {
                return (GameObject)GameObject.Instantiate(Resources.Load(meshName, typeof(GameObject)));
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
        //private Dictionary<int, List<GameObject>> objectPollInUse = new Dictionary<int, List<GameObject>>(); not sure


        public GameObject GetFreeMesh(int meshType)
        {
            //Since PullSize = rows*coloms+4
            //We always have FreeObj 
            int lastIndex = Instance.objectPoolFree[meshType].Count-1;
            GameObject obj = Instance.objectPoolFree[meshType][lastIndex];
            Instance.objectPoolFree[meshType].RemoveAt(lastIndex);
            return obj;
        }

        public void AddFreeObject(CellTypes MeshType, GameObject mesh)
        {
            if (!Instance.objectPoolFree.ContainsKey((int)MeshType))
            {
                Instance.objectPoolFree.Add((int)MeshType, new List<GameObject>());
            }
            mesh.SetActive(false);
            Instance.objectPoolFree[(int)MeshType].Add(mesh);
        }

        private void FillFreePool(CellTypes meshType)
        {
            for (int i=0; i<GameSettings.sizex*GameSettings.sizey; i++)
            {
                GameObject obj = GameCellPoolPrefabFactory.GetMeshByType(meshType);
                if (obj != null)
                {
                    Instance.AddFreeObject(meshType, GameCellPoolPrefabFactory.GetMeshByType(meshType));
                }
            }
        }

        private void InitPool()
        {
            foreach (CellTypes meshType in Enum.GetValues(typeof(CellTypes)))
            {
                if (meshType > 0) 
                {
                    Instance.FillFreePool(meshType);
                }
            }
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
            foreach(List<GameObject> list in Instance.objectPoolFree.Values)
            {
                list.Clear(); 
            }
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

}
