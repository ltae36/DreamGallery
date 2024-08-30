using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RealTimeTexturePainter : MonoBehaviour
{
     public enum BrushStyle
    {
        Airbrush,
        HardType,
    }

    public BrushStyle brush;

    public Material materialToModify;    // ������ material
    private Texture2D texture2D;         // �ؽ�ó�� ������ Texture2D
    public Color paintColor = Color.red; // ����Ʈ ���� ����
    public int brushRadius = 2;          // �귯���� �ݰ� (�ȼ�)
    public GameObject canvasPlane;       // �������� Plane ������Ʈ

    //���� ���� �����̴�
    public Slider alpha;
    //������ ����� �����̴�
    public Slider size;
    //���� �����
    public InputField colorHex;

    private Vector2? previousUVPosition = null; // ���� �������� UV ��ǥ

    void Start()
    {
        // Plane�� ���� ��ǥ�迡���� ũ�⸦ ���
        Vector3 planeSize = GetPlaneSizeInWorldUnits(canvasPlane);

        // Plane ũ�⿡ ���� �ؽ�ó ũ�⸦ ����
        int textureWidth = Mathf.CeilToInt(planeSize.x * 150);
        int textureHeight = Mathf.CeilToInt(planeSize.z * 150);

        // Texture2D ����
        texture2D = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        // ������� �ؽ�ó �ʱ�ȭ
        Color[] fillColorArray = texture2D.GetPixels();

        for (int i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = Color.white;
        }
        texture2D.SetPixels(fillColorArray);
        texture2D.Apply();

        // Material�� �ʱ� �ؽ�ó ����
        materialToModify.mainTexture = texture2D;

        // Plane�� MeshCollider�� ���ٸ� �߰�
        if (canvasPlane.GetComponent<MeshCollider>() == null)
        {
            canvasPlane.AddComponent<MeshCollider>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) brush = BrushStyle.HardType;
        if (Input.GetKeyDown(KeyCode.Alpha2)) brush = BrushStyle.Airbrush;
        switch (brush)
        {
            case BrushStyle.Airbrush:

                if (Input.GetMouseButton(0))
                {
                PaintOnTexture_Airbrush();
                }
                else
                {
                    // ���콺�� Ŭ������ �ʾ��� ���, ���� ��ġ�� �ʱ�ȭ
                    previousUVPosition = null;
                }

                break;
            case BrushStyle.HardType:
                // ���콺�� Ŭ���� ���¿����� ������ ����
                if (Input.GetMouseButton(0))
                {
                    PaintOnTexture();
                }
                else
                {
                    // ���콺�� Ŭ������ �ʾ��� ���, ���� ��ġ�� �ʱ�ȭ
                    previousUVPosition = null;
                }
                break;
            default:
                break;
        }
       
        //print(colorHex.text);
    }

    void PaintOnTexture()//�ϵ�Ÿ��

    {
        // ���콺 �������� ��ġ�� ������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Plane���� �浹 ���� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
                Vector2 currentUVPosition = hit.textureCoord;

            if (hit.collider.gameObject == canvasPlane)
            {
                // �浹�� ������ UV ��ǥ ���

                // ���� UV ��ǥ�� �ִٸ� �� ��ǥ ���̸� ä����
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;

                    // �� ��ǥ ������ �Ÿ��� ����
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance/brushRadius)*10; // ������ ���� ��

                    for (int i = 0; i <= steps; i++)
                    {
                        // �� ��ǥ ���̸� ����
                        Vector2 lerpUV = Vector2.Lerp(previousUV, currentUVPosition, (float)i / steps);

                        // ������ ��ġ���� ����Ʈ ĥ�ϱ�
                        int texX = (int)(lerpUV.x * texture2D.width);
                        int texY = (int)(lerpUV.y * texture2D.height);

                        // �귯�� �ݰ游ŭ�� �ȼ��� ĥ��
                        for (int x = -brushRadius; x <= brushRadius; x++)
                        {
                            for (int y = -brushRadius; y <= brushRadius; y++)
                            {
                                int px = texX + x;
                                int py = texY + y;

                                // ���� �귯�� ���� (�Ÿ� üũ)
                                if (x * x + y * y <= brushRadius * brushRadius)
                                {
                                    // �ؽ�ó ������ ����� �ʴ��� Ȯ��
                                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                                    {
                                        // ���� ����� ���ο� ������ ȥ��
                                        Color originalColor = texture2D.GetPixel(px, py);
                                        Color blendedColor = Color.Lerp(originalColor, paintColor, paintColor.a); // ȥ�� ������ ���ο� ������ ���İ��� ���� ����

                                        // ȥ�յ� ������ �ؽ�ó�� ����
                                        texture2D.SetPixel(px, py, blendedColor);
                                       // texture2D.SetPixel(px, py, paintColor);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    PaintAtUV(currentUVPosition);
                }

                texture2D.Apply(); // ������ �ؽ�ó�� ����

                // ���� ��ġ�� ���� ��ġ�� ������Ʈ
                previousUVPosition = currentUVPosition;
            }
        }
        else
        {
            //ĵ������ ����� ������ġ �ʱ�ȭ
            previousUVPosition = null;

        }
    }

    void PaintOnTexture_Airbrush()//�־�귯��

    {
        // ���콺 �������� ��ġ�� ������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Plane���� �浹 ���� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 currentUVPosition = hit.textureCoord;

            if (hit.collider.gameObject == canvasPlane)
            {
                // �浹�� ������ UV ��ǥ ���

                // ���� UV ��ǥ�� �ִٸ� �� ��ǥ ���̸� ä����
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;

                    // �� ��ǥ ������ �Ÿ��� ����
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance / brushRadius) * 10; // ������ ���� ��

                    for (int i = 0; i <= steps; i++)
                    {
                        // �� ��ǥ ���̸� ����
                        Vector2 lerpUV = Vector2.Lerp(previousUV, currentUVPosition, (float)i / steps);

                        // ������ ��ġ���� ����Ʈ ĥ�ϱ�
                        int texX = (int)(lerpUV.x * texture2D.width);
                        int texY = (int)(lerpUV.y * texture2D.height);

                        // �귯�� �ݰ游ŭ�� �ȼ��� ĥ��
                        // �귯�� �ݰ游ŭ�� �ȼ��� ĥ��
                        for (int x = -brushRadius; x <= brushRadius; x++)
                        {
                            for (int y = -brushRadius; y <= brushRadius; y++)
                            {
                                int px = texX + x;
                                int py = texY + y;

                                distance = Mathf.Sqrt(x * x + y * y);
                                if (distance <= brushRadius)
                                {
                                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                                    {
                                        // �Ÿ� ������� ���İ� ��� (����� ���ϰ�, �����ڸ��� ������ ������)
                                        float alpha = 1 - (distance / brushRadius);

                                        Color originalColor = texture2D.GetPixel(px, py);
                                        Color blendedColor = Color.Lerp(originalColor, paintColor, alpha * paintColor.a);

                                        texture2D.SetPixel(px, py, blendedColor);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    PaintAtUV_Airbrush (currentUVPosition);
                }

                texture2D.Apply(); // ������ �ؽ�ó�� ����

                // ���� ��ġ�� ���� ��ġ�� ������Ʈ
                previousUVPosition = currentUVPosition;
            }
        }
        else
        {
            //ĵ������ ����� ������ġ �ʱ�ȭ
            previousUVPosition = null;

        }
    }

    void PaintAtUV(Vector2 uvPosition)//�ϵ�Ÿ��
    {
        // UV ��ǥ�� �ؽ�ó ��ǥ�� ��ȯ
        int texX = (int)(uvPosition.x * texture2D.width);
        int texY = (int)(uvPosition.y * texture2D.height);

        // �귯�� �ݰ游ŭ�� �ȼ��� ĥ��
        for (int x = -brushRadius; x <= brushRadius; x++)
        {
            for (int y = -brushRadius; y <= brushRadius; y++)
            {
                int px = texX + x;
                int py = texY + y;

                // ���� �귯�� ���� (�Ÿ� üũ)
                if (x * x + y * y <= brushRadius * brushRadius)
                {
                    // �ؽ�ó ������ ����� �ʴ��� Ȯ��
                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                    {
                        // ���� ����� ���ο� ������ ȥ��
                        Color originalColor = texture2D.GetPixel(px, py);
                        Color blendedColor = Color.Lerp(originalColor, paintColor, paintColor.a); // ȥ�� ������ ���ο� ������ ���İ��� ���� ����

                        // ȥ�յ� ������ �ؽ�ó�� ����
                        texture2D.SetPixel(px, py, blendedColor);
                        // texture2D.SetPixel(px, py, paintColor);
                    }
                }
            }
        }
    }




    void PaintAtUV_Airbrush(Vector2 uvPosition)//����귯����
    {
        // UV ��ǥ�� �ؽ�ó ��ǥ�� ��ȯ
        int texX = (int)(uvPosition.x * texture2D.width);
        int texY = (int)(uvPosition.y * texture2D.height);

        // �귯�� �ݰ游ŭ�� �ȼ��� ĥ��
        for (int x = -brushRadius; x <= brushRadius; x++)
        {
            for (int y = -brushRadius; y <= brushRadius; y++)
            {
                int px = texX + x;
                int py = texY + y;

                float distance = Mathf.Sqrt(x * x + y * y);
                if (distance <= brushRadius)
                {
                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                    {
                        // �Ÿ� ������� ���İ� ��� (����� ���ϰ�, �����ڸ��� ������ ������)
                        float alpha = 1 - (distance / brushRadius);

                        Color originalColor = texture2D.GetPixel(px, py);
                        Color blendedColor = Color.Lerp(originalColor, paintColor, alpha * paintColor.a);

                        texture2D.SetPixel(px, py, blendedColor);
                    }
                }
            }
        }
    }

    public void ColorChange()
    {
       // print("�����");
        if (ColorUtility.TryParseHtmlString(colorHex.text.ToString(), out Color newColor))
        {
            paintColor = newColor;
            paintColor.a = alpha.value;

            print(colorHex.text);
        }
        else
        {

        }
    }

    public void SaveTexture()
    {
        // Texture2D�� ������ PNG �������� ��ȯ
        byte[] bytes = texture2D.EncodeToPNG();

        // ������ ���� ��� ����
        string filePath = Path.Combine(Application.dataPath, "SavedTexture.png");

        // ���Ϸ� ����
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Texture saved to {filePath}");
    }

    public void SizeChange()
    {
        brushRadius =(int) size.value;
    }
    Vector3 GetPlaneSizeInWorldUnits(GameObject plane)
    {
        // Plane�� �⺻ ũ��� 10x10 �����Դϴ�. �̸� ������� Plane�� ���� ũ�⸦ ����մϴ�.
        Vector3 scale = plane.transform.localScale;
        float planeWidth = scale.x * 10.0f;
        float planeHeight = scale.z * 10.0f;

        return new Vector3(planeWidth, 1.0f, planeHeight);
    }
    public void AlphaChange()
    {
        paintColor.a = alpha.value;
    }

}
