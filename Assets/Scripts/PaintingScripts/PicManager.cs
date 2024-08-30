using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicManager : MonoBehaviour
{
    // �÷��̾ �׸� �׸���(�ؽ�ó)�� �����ؼ� �ҷ��´�.
    // ui�� ���ʴ�� ������ ǥ�õȴ�.

    #region �迭�� �ֱ�
    // �׸� ���
    public RawImage[] images;
    public List<Texture> paintings;

    private void Awake()
    {
        // �÷����ϸ� �� ĵ���� ui�� �ؽ�ó �迭�� ����ִ´�.
        for (int i = 0; i < images.Length; i++)
        {
            paintings.Add(Resources.Load("pic" + i.ToString()) as Texture); // Resouces �������� �׸����� �����´�.

            //paintings[i] = Resources.Load("pic" + i.ToString()) as Texture; // Resouces �������� �׸����� �����´�.

            images[i].texture = paintings[i]; // Rawimages�� �ؽ�ó�� �׸����� �ִ´�.

            images[i].SetNativeSize(); // �׸� �ؽ�ó ũ�⿡ Rawimage ũ�⸦ �����.
        }
    }
    #endregion
}
