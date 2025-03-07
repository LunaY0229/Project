using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action<Vector2> moveDir;
    public Action jumpAction;
    public Action attackActionDown;
    public Action attackActionUp;
    public Action dodgeAction;
    public Action runActionExit;
    public Action<float> scrollWheelAction;  // 新增的滚轮输入Action

    private float targetUp = 0f;
    private float targetRight = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        targetRight = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        targetUp = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        moveDir?.Invoke(new Vector2(targetRight, targetUp));

        // 处理鼠标滚轮输入
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue != 0f)  // 如果有滚动
            scrollWheelAction?.Invoke(scrollValue);  // 调用滚轮输入Action

        if (Input.GetMouseButtonDown(0)) attackActionDown?.Invoke();

        if (Input.GetMouseButtonUp(0))
            attackActionUp?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space)) jumpAction?.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            dodgeAction?.Invoke();

        if (Input.GetKeyUp(KeyCode.LeftShift))
            runActionExit?.Invoke();
    }

}
