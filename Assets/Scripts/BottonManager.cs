using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonManager : MonoBehaviour
{
    public GameObject frame;
    public Transform pos;   

    MeshRenderer mr;
    public RawImage[] images;
    Texture[] tex;

    void Start()
    {
        mr = frame.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void SelectPic() 
    {
        for(int i = 0; i < images.Length; i++) 
        {
            tex[i] = images[i].GetComponent<Texture>();
        }

        // 해당하는 자리에 액자를 생성한다.
        Instantiate(frame, pos);
    }
}
