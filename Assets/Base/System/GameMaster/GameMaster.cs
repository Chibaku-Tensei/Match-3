using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Base
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected List<int> listId = new List<int>();

        public List<int> ListId => listId;
        
        #region Singleton

        private static GameMaster _instance;

        public static GameMaster Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameMaster = new GameObject("Game Master");
                    gameMaster.transform.position = Vector3.zero;
                    _instance = gameMaster.AddComponent<GameMaster>();
                }

                return _instance;
            }
        }
        #endregion

        private void Awake()
        {
            _instance = this;
        }

        public static int GenerateId()
        {
            int id;
            do
            {
                id = Random.Range(1, 9999);
            } while (Instance.listId.Contains(id));
            Instance.listId.Add(id);
            return id;
        }

        public static void RemoveId(int id)
        {
            if (Instance.listId.Contains(id))
            {
                Instance.listId.Remove(id);
            }
        }
    }
}
