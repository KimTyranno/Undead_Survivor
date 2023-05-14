using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
  // 프리팹들을 보관할 변수
  public GameObject[] prefabs;
  // 풀 담당을하는 리스트
  List<GameObject>[] pools;

  void Awake()
  {
    pools = new List<GameObject>[prefabs.Length];

    for (int i = 0; i < pools.Length; i++)
    {
      pools[i] = new List<GameObject>();
    }
  }

  public GameObject Get(int index)
  {
    GameObject select = null;

    // 선택한풀에서 사용되고 있지않은 오브젝트를 찾음
    foreach (GameObject item in pools[index])
    {
      // 발견되면 select에 할당
      if (!item.activeSelf)
      {
        select = item;
        select.SetActive(true);
        break;
      }
    }
    // 발견되지 않으면 새롭게 생성하여 select에 할당
    if (select == null)
    {
      select = Instantiate(prefabs[index], transform);
      pools[index].Add(select);
    }
    return select;
  }
}
