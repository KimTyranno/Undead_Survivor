using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
  public float scanRange;
  public LayerMask targetLayer;
  public RaycastHit2D[] targets;
  public Transform nearestTarget;

  void FixedUpdate()
  {
    /**
        CircleCastAll이란 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        CircleCastAll( 캐스팅 시작위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상레이어)
        지금은 어차피 플레이어 주위를 원형으로 하는거라 방향과 길이는 딱히 필요없음)
    */
    targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
    // 지속적으로 가장 가까운목표를 업데이트
    nearestTarget = GetNearest();
  }

  // 가장 가까운것을 가져옴
  Transform GetNearest()
  {
    Transform result = null;
    // 기본거리를 100으로 설정
    float diff = 100;

    foreach (RaycastHit2D target in targets)
    {
      Vector3 myPos = transform.position;
      Vector3 targetPosition = target.transform.position;
      // 두개의 거리를 가져옴
      float curDiff = Vector3.Distance(myPos, targetPosition);
      if (curDiff < diff)
      {
        diff = curDiff;
        result = target.transform;
      }
    }
    return result;
  }
}
