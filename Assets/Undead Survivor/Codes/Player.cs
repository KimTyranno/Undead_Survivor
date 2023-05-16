using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
  public Vector2 inputVec;
  public Scanner scanner;
  Rigidbody2D rigid;
  SpriteRenderer spriter;
  Animator anim;
  public float speed;
  void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
    spriter = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();
    scanner = GetComponent<Scanner>();
  }
  // Start is called before the first frame update
  void Start()
  {

  }


  void FixedUpdate()
  {
    // 이동방식 3가지
    // 1. 힘 주기
    // rigid.AddForce(inputVec);

    // 2. 속도제어
    // rigid.velocity = inputVec;

    Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
    // 3. 위치이동 (MovePosition은 위치이동이므로 현재위치(rigid.position)도 더해줘야함)
    rigid.MovePosition(rigid.position + nextVec);
  }

  void OnMove(InputValue value)
  {
    // 아마 Player Input에서 control type을 Vector2로 설정해서 그거랑 맞춘듯
    inputVec = value.Get<Vector2>();
  }

  void LateUpdate()
  {
    // magnitude는 어떤방향을 눌렀든지 그 크기만 가져옴
    anim.SetFloat("Speed", inputVec.magnitude);
    if (inputVec.x != 0)
    {
      // inputVec.x는 입력시, 왼쪽인경우 -1, 오른쪽인경우 1을 반환
      spriter.flipX = inputVec.x < 0;
    }
  }
}
