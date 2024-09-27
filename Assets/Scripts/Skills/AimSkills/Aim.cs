using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    LineRenderer line;

    Vector3 currentPos;
    float curTg;
    IEnumerator coroutine;

    IEnumerator Rotation(Vector2 mouse)
    {
        float x = mouse.x - this.transform.position.x;
        float y = mouse.y - this.transform.position.y;
        float angle = (float)Math.Atan(y / x);
        while(Input.GetMouseButtonDown(0))
        {
            currentPos = new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0.0f);
            line.SetPosition(0, currentPos);
            yield return null;
        }
    }

    void Start()
    {
        line = GetComponent<LineRenderer>();
        currentPos = Vector3.zero;
        curTg = 0f;
    }

    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mouse != currentPos)
        {
            //float tg = (currentPos.y - this.transform.position.y) / (currentPos.x - this.transform.position.x);
            //float angle = Math.Atan(tg);
            /*if(coroutine!= null)
                StopCoroutine(coroutine);
            coroutine = Rotation((Vector2)mousePos);
            StartCoroutine(coroutine);*/

            float x = mouse.x - this.transform.position.x;
            float y = mouse.y - this.transform.position.y;
            //Debug.Log(x + " " + y);
            float angle = (float)Math.Abs(Math.Atan(y / x));
            //float len = (float)Math.Sqrt(x * x + y * y);
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            cos = x > 0 ? cos : -1 * cos;
            sin = y > 0 ? sin : -1 * sin;
            //sin = sin < 0.0f ? 0 : sin;
            //cos = sin == 0.0f ? 1 : cos;
            currentPos = new Vector3(cos, sin, 0.0f);
            line.SetPosition(0, currentPos);
        }
    }

}
