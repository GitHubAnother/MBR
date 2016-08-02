/*
作者名称:YHB

脚本作用:相机控制

建立时间:2016.8.2.14.24
*/

using UnityEngine;
using System.Collections;

public class MouseLookAt : MonoBehaviour
{
    #region 字段
    public float xSpeed = 10f;
    public float ySpeed = 10f;
    public float minY = -60f;
    public float maxY = 60f;

    private float axisX;
    private float axisY;
    #endregion

    #region 控制
    void Update()
    {
        MouseController();
    }
    #endregion

    #region 控制方法
    void MouseController()
    {
        axisX = Input.GetAxis("Mouse X") * ySpeed + this.transform.localEulerAngles.y;
        axisY -= Input.GetAxis("Mouse Y") * xSpeed;

        axisY = Mathf.Clamp(axisY, minY, maxY);

        this.transform.localEulerAngles = new Vector3(axisY, axisX, 0);
    }
    #endregion
}
