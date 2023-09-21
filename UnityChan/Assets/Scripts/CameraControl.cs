using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;

    GameObject parent; // 변수 선언

    [SerializeField] float posSensitive;
    [SerializeField] float rotSensitive;
    [SerializeField] float wheelSensitive;

    Vector3 defPosition;
    Quaternion defRotation;
    float defZoom;

    void Start ()
	{
        parent = transform.parent.gameObject;

        // 기본 위치 저장
        defPosition = target.transform.position;
        defRotation = target.transform.rotation;
        defZoom = Camera.main.fieldOfView;
    }

    void Update()
    {
        // 왼쪽 드래그로 카메라 이동
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("좌클릭 = " + Input.mousePosition);
			target.transform.Translate (
                -Input.GetAxis("Mouse X") / posSensitive,
                Input.GetAxis("Mouse Y") / posSensitive,
                0);
        }

        // 오른쪽 드래그로 카메라 이동
        if (Input.GetMouseButton(1))
        {
            //Debug.Log("우클릭 = " + Input.mousePosition);
            target.transform.Translate(
                -Input.GetAxis("Mouse X") * rotSensitive,
                -Input.GetAxis("Mouse Y") * rotSensitive,
                0);
        }

        // 휠 클릭으로 설정 초기화
        if (Input.GetMouseButton(2))
        {
            target.transform.position = defPosition;
            target.transform.rotation = defRotation;
            Camera.main.fieldOfView = defZoom;
        }

        // 휠 회전으로 확대/축소
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
            Camera.main.fieldOfView -= (wheelSensitive * Input.GetAxis("Mouse ScrollWheel"));

            if (Camera.main.fieldOfView < 10) Camera.main.fieldOfView = 10;
            else if (Camera.main.fieldOfView > 100) Camera.main.fieldOfView = 100;
		}
    }
}
