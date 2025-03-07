using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowProjectile : BaseProjectile
{
    public float flightTime = 0.4f;  // ����ʱ��
    public float height = 2.0f;  // �����߶ȣ����Ե������������������̶�

    private Vector3 startPos;  // ��ʼλ��
    private float timeElapsed = 0.0f;  // ���о�����ʱ��

    protected void Start()
    {
        startPos = transform.position;  // ��ȡ��ʼλ��
    }

    protected override void RowMove()
    {
        timeElapsed += Time.deltaTime;

        // ���㱴�������ߵĲ���t����Χ��0��1
        float t = Mathf.Clamp01(timeElapsed / flightTime);

        // ���㱴�������ߵĿ��Ƶ�
        Vector3 controlPoint = (startPos + targetPos) / 2;
        controlPoint.y += height;  // ͨ������yֵʹ�ÿ��Ƶ������ߵĶ������γ�������

        // ���㵱ǰλ��
        Vector3 currentPos = CalculateBezier(startPos, controlPoint, targetPos, t);

        transform.forward = currentPos - transform.position;

        // ���·��й켣
        transform.position = currentPos;
    }

    // ������α��������ߵĺ���
    private Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float oneMinusT = 1 - t;
        return oneMinusT * oneMinusT * p0 + 2 * oneMinusT * t * p1 + t * t * p2;
    }

    // ����Ŀ��ʱ�Ĵ������Ը�����Ҫ�޸ģ�
    private void OnHitTarget()
    {
        // ����������ʵ������Ŀ��ʱ��Ч�������籬ը���˺�
        //Destroy(gameObject);  // �򵥵������ӵ�
    }
}
