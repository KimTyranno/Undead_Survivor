using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
  RectTransform rect;
  void Awake()
  {
    rect = GetComponent<RectTransform>();
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    // WorldToScreenPoint란 월드상의 오브젝트위치를 스크린좌표로 변환함 (플레이어 오브젝트(월드좌표)와 스크린좌표가 다르기 때문) 
    // 반대의 경우는 ScreenToWorldPoint를 사용
    rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
  }
}
