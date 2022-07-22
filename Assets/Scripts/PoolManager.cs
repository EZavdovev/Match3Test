using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��� ��� ��������� � ����������� ��������
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
    /// ��������� ������� ������ �� ����
    /// </summary>
    /// <param name="prefab">������ �������, ������� ���������� �������</param>
    /// <param name="position">�������, ���� ����� ��������� ������</param>
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
    /// ���������� ������ � ���
    /// </summary>
    /// <param name="target">������, ������� ����� �������</param>
    public static void PutGameObjectToPool(GameObject target)
    {
        target.SetActive(false);
        poolObjects[target.name].Add(target);
    }
}

