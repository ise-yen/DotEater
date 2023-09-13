using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public float interval = 1.5f; // ���� �ð�����
    public float range = 3;
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
            Vector3 pos = transform.position;
            pos.y += Random.Range(-range, range);
            Instantiate(wallPrefab, pos, transform.rotation);
            term -= interval;
		}
    }
}
