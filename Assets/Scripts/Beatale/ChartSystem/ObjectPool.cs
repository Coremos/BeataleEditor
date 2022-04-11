using System.Collections.Generic;
using UnityEngine;

namespace Beatale.ChartSystem
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        public T Prefab;
        private Queue<T> objectPool;
        
        private void Awake()
        {
            objectPool = new Queue<T>();
            InstantiateObject(5);
        }

        private void InstantiateObject(int count)
        {
            for (int index = 0; index < count; index++)
            {
                var gameObject = Instantiate(Prefab, transform);
                gameObject.gameObject.SetActive(false);
                objectPool.Enqueue(gameObject);
            }
        }

        public T GetObject()
        {
            if (objectPool.Count == 0)
            {
                InstantiateObject(10);
            }
            var gameObject = objectPool.Dequeue();
            return gameObject;
        }

        public void RestoreObject(T gameObject)
        {
            gameObject.gameObject.SetActive(false);
            objectPool.Enqueue(gameObject);
        }
    }
}
