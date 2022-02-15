using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;

namespace Base.Pattern
{
    public class ObjectPooler : BaseMono
    {
        #region Singleton

        private static ObjectPooler _sharedInstance;

        public static ObjectPooler SharedInstance
        {
            get
            {
                if (_sharedInstance == null)
                {
                    _sharedInstance = FindObjectOfType<ObjectPooler>();
                    if (!_sharedInstance)
                    {
                        GameObject poolMaster = new GameObject("PoolMaster");
                        _sharedInstance = poolMaster.AddComponent<ObjectPooler>();
                        _sharedInstance.Position = Vector3.zero;
                    }
                }

                return _sharedInstance;
            }
        }

        #endregion
        
        private Dictionary<string, BasePool> _poolDictionary = new Dictionary<string, BasePool>();
        
        public List<BasePool> listPool = new List<BasePool>();

        public bool isInitializeOnStart = false;

        #region Unity life cycle

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _sharedInstance = this;
        }

        private void Start()
        {
            if (isInitializeOnStart)
            {
                InitObjectPool();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _sharedInstance = null;
            listPool.Clear();
        }

        #endregion
        
        public static void InitObjectPool()
        {
            for (int i = 0; i < SharedInstance.listPool.Count; i++)
            {
                SharedInstance._poolDictionary[SharedInstance.listPool[i].name] = SharedInstance.listPool[i];
            }

            var list = SharedInstance._poolDictionary.Values.ToList();
            int length = SharedInstance._poolDictionary.Values.Count;

            for (int i = 0; i < length; i++)
            {
                list[i].InitBasePool(SharedInstance.transform);
            }
        }

        public static Transform Get(string name)
        {
            try
            {
                return SharedInstance._poolDictionary[name].Get();
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
                return null;
            }
        }

        public static Transform Get(string name, Vector3 position, Vector3 eulerAngle, Transform newParent = null, bool isWorld = false)
        {
            try
            {
                return SharedInstance._poolDictionary[name].Get(position, eulerAngle, newParent, isWorld);
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
                return null;
            }
        }

        public static void Return(string name, Transform obj, Transform newParent = null)
        {
            try
            {
                if (SharedInstance._poolDictionary.Count > 0)
                {
                    SharedInstance._poolDictionary[name].Return(obj, newParent);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception.Message);
            }
        }

    }
    
    [Serializable]
    public class BasePool
    {
        public string name;
        public Transform prefab;
        public int amount;
        public bool isAutoGen = false;
        
        [NonSerialized] public Queue<Transform> Pool = new Queue<Transform>();

        private Transform _thisTransform;

        public void InitBasePool(Transform root)
        {
            GameObject basePoolObject = new GameObject(name);
            basePoolObject.transform.SetParent(root);
            _thisTransform = basePoolObject.transform;
            _thisTransform.position = Vector3.zero;
            for (int i = 0; i < amount; i++)
            {
                Transform pooledObj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, _thisTransform);
                pooledObj.gameObject.SetActive(false);
                Pool.Enqueue(pooledObj);
            }
        }

        private void AdditionalInit(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Transform pooledObj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, _thisTransform);
                pooledObj.gameObject.SetActive(false);
                Pool.Enqueue(pooledObj);
            }
        }
        
        #region Get object from pool

        public Transform Get()
        {
            if (Pool.Count == 0 && isAutoGen)
            {
                AdditionalInit(10);
            }
            var obj = Pool.Dequeue();
            //Pool.Enqueue(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public Transform Get(Vector3 position, Vector3 eulerAngle, Transform newParent = null, bool isWorld = false)
        {
            var obj = Get();
            if (newParent && !isWorld)
            {
                obj.SetParent(newParent);
                obj.localPosition = position;
                obj.localRotation = Quaternion.Euler(eulerAngle);
            }
            else if (newParent && isWorld)
            {
                obj.SetParent(newParent);
                obj.position = position;
                obj.rotation = Quaternion.Euler(eulerAngle);
            }
            else if (!newParent)
            {
                obj.position = position;
                obj.rotation = Quaternion.Euler(eulerAngle);
            }

            return obj;
        }

        #endregion
        public void Return(Transform obj, Transform newParent = null)
        {
            if (newParent)
            {
                obj.SetParent(newParent);
            }
            else
            {
                obj.SetParent(_thisTransform);
            }
            obj.gameObject.SetActive(false);
            obj.position = Vector3.zero;
            obj.rotation = Quaternion.identity;
            
            Pool.Enqueue(obj);
        }
    }
}

