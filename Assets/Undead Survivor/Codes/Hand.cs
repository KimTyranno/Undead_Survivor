using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
  public bool isLeft;
  public SpriteRenderer spriter;

  SpriteRenderer player;

  // 수치는 Hand Right에서 내가 임의로 position을 설정한거와 동일하게 설정
  Vector3 rightPos = new Vector3(0.3f, -0.15f, 0);
  Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
  Quaternion leftRot = Quaternion.Euler(0, 0, -33);
  Quaternion leftRotReverse = Quaternion.Euler(0, 0, -133);
  void Awake()
  {
    // 자기자신은 [0]이므로 부모를 가져오려면 [1]로 가져옴
    player = GetComponentsInParent<SpriteRenderer>()[1];
  }

  void LateUpdate()
  {
    bool isReverse = player.flipX;

    if (isLeft)
    { // 근접무기 (내가볼땐 오른손인데 왜 왼쪽이라 하는진 모르겠음)
      transform.localRotation = isReverse ? leftRotReverse : leftRot;
      spriter.flipY = isReverse;
      spriter.sortingOrder = isReverse ? 4 : 6;
    }
    else
    { // 원거리무기
      transform.localPosition = isReverse ? rightPosReverse : rightPos;
      spriter.flipX = isReverse;
      spriter.sortingOrder = isReverse ? 6 : 4;
    }
  }
}
