/*
作者名称:YHB

脚本作用:准星获取3D物体

建立时间:2016.8.2.15.41
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    #region 字段
    public LayerMask targetMask = -1;
    public float scale = 1.6f;

    private Transform targetPos;
    private RectTransform rectTransform;
    private Text text;
    #endregion

    #region Start赋值  LateUpdate执行逻辑
    void Start()
    {
        text = this.transform.FindChild("text").GetComponent<Text>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    void LateUpdate()
    {
        RayDetection();
    }
    #endregion

    #region  射线检测
    void RayDetection()
    {
        RaycastHit hit;

        bool isOK = Physics.Raycast(Camera.main.transform.position,
                                                      Camera.main.transform.forward,
                                                      out hit, 300, targetMask);

        if (isOK)
        {
            //射线碰到了物体---记录位置
            targetPos = hit.transform;
        }

        if (targetPos != null)
        {
            //这只有在Canvas的RenderMode为第一个（Screenspace-Overlay）的模式下才能这样
            //将屏幕坐标赋值给世界坐标
            this.transform.position = Camera.main.WorldToScreenPoint(targetPos.position);

            //AABB(矩形包围框,不旋转,外形会随着物体的旋转变大)
            //OBB（会跟着旋转，所以外形是不变的）
            Rect bound = CalculationAABB(targetPos);

            //宽高赋值
            rectTransform.sizeDelta = new Vector2(bound.width, bound.height) * scale;

            //字
            text.text = targetPos.name + "\n" + "X:" + targetPos.position.x.ToString("F2") + "\n" +
                               "Y:" + targetPos.position.y.ToString("F2") + "\n" +
                               "Distance:" + (Camera.main.transform.position - targetPos.position).magnitude.ToString("F3");
        }

    }
    #endregion

    #region 计算AABB
    Rect CalculationAABB(Transform targetPos)
    {
        Vector3 center = targetPos.GetComponent<Renderer>().bounds.center;//中心点
        Vector3 extents = targetPos.GetComponent<Renderer>().bounds.extents;//大小

        Vector2[] points = new Vector2[8] {
Camera.main.WorldToScreenPoint(new Vector3(center.x - extents.x,center.y - extents.y,center.z - extents.z)),
Camera.main.WorldToScreenPoint(new Vector3(center.x + extents.x,center.y - extents.y,center.z - extents.z)),
Camera.main.WorldToScreenPoint(new Vector3(center.x - extents.x,center.y - extents.y,center.z + extents.z)),
Camera.main.WorldToScreenPoint(new Vector3(center.x + extents.x,center.y - extents.y,center.z + extents.z)),

Camera.main.WorldToScreenPoint(new Vector3(center.x - extents.x,center.y + extents.y,center.z - extents.z)),
Camera.main.WorldToScreenPoint(new Vector3(center.x + extents.x,center.y + extents.y,center.z - extents.z)),
Camera.main.WorldToScreenPoint(new Vector3(center.x - extents.x,center.y + extents.y,center.z + extents.z)),
Camera.main.WorldToScreenPoint(new Vector3(center.x + extents.x,center.y + extents.y,center.z + extents.z))
        };

        //记录初始化的点
        Vector2 min = points[0];
        Vector2 max = points[0];

        foreach (Vector2 v in points)
        {
            min = Vector2.Min(min, v);//比较返回最小值
            max = Vector2.Max(max, v);
        }

        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }
    #endregion
}
