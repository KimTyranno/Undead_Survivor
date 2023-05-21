using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터별 고유특성을 갖는 프로퍼티
public class Character : MonoBehaviour
{
  public static float Speed
  {
    get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
  }

  // 근접무기 공격속도
  public static float WeaponSpeed
  {
    get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
  }

  // 원거리무기 공격속도
  public static float WeaponRate
  {
    get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
  }
  public static float Damage
  {
    get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
  }
  public static int Count
  {
    get { return GameManager.instance.playerId == 3 ? 1 : 0; }
  }

}
