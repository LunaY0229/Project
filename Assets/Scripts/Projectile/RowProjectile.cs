using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowProjectile : BaseProjectile
{
    public float flightTime = 0.4f;  // 飞行时间
    public float height = 2.0f;  // 弯曲高度，可以调整弓箭的曲线弯曲程度

    private Vector3 startPos;  // 起始位置
    private float timeElapsed = 0.0f;  // 飞行经过的时间

    protected void Start()
    {
        startPos = transform.position;  // 获取初始位置
    }

    protected override void RowMove()
    {
        timeElapsed += Time.deltaTime;

        // 计算贝塞尔曲线的参数t，范围从0到1
        float t = Mathf.Clamp01(timeElapsed / flightTime);

        // 计算贝塞尔曲线的控制点
        Vector3 controlPoint = (startPos + targetPos) / 2;
        controlPoint.y += height;  // 通过调整y值使得控制点在曲线的顶部，形成抛物线

        // 计算当前位置
        Vector3 currentPos = CalculateBezier(startPos, controlPoint, targetPos, t);

        transform.forward = currentPos - transform.position;

        // 更新飞行轨迹
        transform.position = currentPos;
    }

    // 计算二次贝塞尔曲线的函数
    private Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float oneMinusT = 1 - t;
        return oneMinusT * oneMinusT * p0 + 2 * oneMinusT * t * p1 + t * t * p2;
    }

    // 命中目标时的处理（可以根据需要修改）
    private void OnHitTarget()
    {
        // 可以在这里实现命中目标时的效果，比如爆炸或伤害
        //Destroy(gameObject);  // 简单的销毁子弹
    }
}
