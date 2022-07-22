using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Пул для появления и уничтожения объектов
/// </summary>
public class PoolManager : MonoBehaviour
{
    private static Dictionary<string, List<GameObject>> poolObjects;

    private void OnEnable()
    {
        poolObjects = new Dictionary<string, List<GameObject>>();
    }

    private void OnDisable()
    {
        poolObjects.Clear();
    }
    /// <summary>
    /// Позволяет достать объект из пула
    /// </summary>
    /// <param name="prefab">префаб объекта, который необходимо достать</param>
    /// <param name="position">позиция, куда нужно доставить объект</param>
    /// <returns></returns>
    public static GameObject GetGameObjectFromPool(GameObject prefab, Transform position)
    {
        if (!poolObjects.ContainsKey(prefab.name))
        {
            poolObjects[prefab.name] = new List<GameObject>();
        }

        int index = poolObjects[prefab.name].Count;
        GameObject result;

        if (index > 0)
        {
            result = poolObjects[prefab.name][index - 1];
            poolObjects[prefab.name].RemoveAt(index - 1);
            result.transform.position = position.position;
            result.transform.rotation = position.rotation;
            result.SetActive(true);
            return result;
        }
        else
        {
            result = Instantiate(prefab, position);
            result.name = prefab.name;
            return result;
        }
    }
    /// <summary>
    /// Возвращает объект в пул
    /// </summary>
    /// <param name="target">объект, который нужно вернуть</param>
    public static void PutGameObjectToPool(GameObject target)
    {
        target.SetActive(false);
        poolObjects[target.name].Add(target);
    }
}

