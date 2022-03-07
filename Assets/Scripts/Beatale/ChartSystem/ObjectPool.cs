using System.Collections.Generic;
using UnityEngine;

namespace Beatale.ChartSystem
{
    public abstract class ObjectPool : MonoBehaviour
    { 
        public GameObject Prefab;
        private Queue<GameObject> objectPool;

        private void Awake()
        {
            objectPool = new Queue<GameObject>();
            InstantiateObject(500);
        }

        private void InstantiateObject(int count)
        {
            for(int index = 0; index < count; index++)
            {
                var gameObject = Instantiate(Prefab, transform);
                gameObject.SetActive(false);
                objectPool.Enqueue(gameObject);
            }
        }

        public GameObject GetObject()
        {
            if (objectPool.Count == 0)
            {
                InstantiateObject(10);
            }
            var gameObject = objectPool.Dequeue();
            return gameObject;
        }

        public void RestoreObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
            objectPool.Enqueue(gameObject);
        }
    }
}