using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpriteScaler : MonoBehaviour
{
    SpriteRenderer sr;
    float _screenHeight, _screenWidth;
    void Start()
    {
        ObjectScaler();

    }


    //updating obj scale according to screen size 
    private void Update()
    {
        if (_screenHeight != Screen.height || _screenWidth != Screen.width)
        {
            ObjectScaler();
        }
    }


    void ObjectScaler()
    {
        Vector3 screenMid = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, transform.position.z + 10));

        transform.position = screenMid;

        sr = GetComponent<SpriteRenderer>();
       // gameObject.AddComponent<TestMeshPro>(); 

        if (sr == null)
            return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        //float worldScreenHeight = Camera.main.scaledPixelHeight ;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float localScaleX = worldScreenWidth / width;
        float localScaleY = worldScreenHeight / height;

        transform.localScale = new Vector3(localScaleX, localScaleY, (localScaleX + localScaleY) / 2);

        transform.position = new Vector3(transform.position.x, transform.position.y, 3.05f);
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }
}