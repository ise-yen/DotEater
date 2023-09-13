using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public float interval = 1.5f; // ���� �ð�����
    float term;

    void Start()
    {
        term = interval; // ���ۺ��� ���� �ϳ� ������ ����
    }

    // Update is called once per frame
    void Update()
    {
        term += Time.deltaTime;
        if(term >= interval)
		{
            Instantiate(wallPrefab, transform.position, transform.rotation);
            term -= interval;
		}
    }
}
