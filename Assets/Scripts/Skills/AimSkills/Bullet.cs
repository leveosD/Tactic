using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float step;
    bool isChar;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            isChar = true;
        }
        else
            Destroy(this);
    }

    public IEnumerator Shot(Vector3 direction)
    {
        while (!isChar)
        {
            this.transform.position += direction.normalized * Time.deltaTime * step;
            yield return null;
        }
        Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        isChar = false;
        step = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
