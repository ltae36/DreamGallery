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

    //void Start()
    void CanvasChange()
    {
        // Material�� �Ҵ���� ���� ��� �� Material ����
        if (materialToModify == null)
        {
            // Universal Render Pipeline�� Unlit ���̴��� ����ϴ� �� Material ����
            materialToModify = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            // ���� ������ Material�� Plane�� ����
            Renderer renderer = canvasPlane.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialToModify;
            }
        }

        // Material�� �̹� �ؽ�ó�� �ִ��� Ȯ��
        if (materialToModify.mainTexture != null)
        {
            // ���� �ؽ�ó�� �ִ� ��� �̸� Texture2D�� ĳ����
            texture2D = materialToModify.mainTexture as Texture2D;
        }
        else
        {
            // �ؽ�ó�� ���� ��� ���� ����
            Vector3 planeSize = GetPlaneSizeInWorldUnits(canvasPlane);
            int textureWidth = Mathf.CeilToInt(planeSize.x * 150);
            int textureHeight = Mathf.CeilToInt(planeSize.z * 150);
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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Plane"))
                {
                    if (hit.transform.gameObject != canvasPlane)
                    {
                        canvasPlane = hit.transform.gameObject;
                        materialToModify = hit.transform.gameObject.GetComponent<MeshRenderer>().material;
                        CanvasChange();
                    }
                    //print("ĵ������");
                }

            }
        }


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
        }
    }

    void PaintOnTexture() // �ϵ�Ÿ��
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
                // ���� UV ��ǥ�� �ִٸ� �� ��ǥ ���̸� ä����
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance / brushRadius) * 10; // ������ ���� ��

                    for (int i = 0; i <= steps; i++)
                    {
                        Vector2 lerpUV = Vector2.Lerp(previousUV, currentUVPosition, (float)i / steps);
                        int texX = (int)(lerpUV.x * texture2D.width);
                        int texY = (int)(lerpUV.y * texture2D.height);

                        for (int x = -brushRadius; x <= brushRadius; x++)
                        {
                            for (int y = -brushRadius; y <= brushRadius; y++)
                            {
                                int px = texX + x;
                                int py = texY + y;

                                if (x * x + y * y <= brushRadius * brushRadius)
                                {
                                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                                    {
                                        Color originalColor = texture2D.GetPixel(px, py);
                                        Color blendedColor = Color.Lerp(originalColor, paintColor, paintColor.a);

                                        texture2D.SetPixel(px, py, blendedColor);
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
                previousUVPosition = currentUVPosition; // ���� ��ġ�� ���� ��ġ�� ������Ʈ
            }
        }
        else
        {
            previousUVPosition = null; // ĵ������ ����� ���� ��ġ �ʱ�ȭ
        }
    }

    void PaintOnTexture_Airbrush() // ����귯��
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
                // ���� UV ��ǥ�� �ִٸ� �� ��ǥ ���̸� ä����
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance / brushRadius) * 10; // ������ ���� ��

                    for (int i = 0; i <= steps; i++)
                    {
                        Vector2 lerpUV = Vector2.Lerp(previousUV, currentUVPosition, (float)i / steps);
                        int texX = (int)(lerpUV.x * texture2D.width);
                        int texY = (int)(lerpUV.y * texture2D.height);

                        for (int x = -brushRadius; x <= brushRadius; x++)
                        {
                            for (int y = -brushRadius; y <= brushRadius; y++)
                            {
                                int px = texX + x;
                                int py = texY + y;

                                float distanceFromCenter = Mathf.Sqrt(x * x + y * y);
                                if (distanceFromCenter <= brushRadius)
                                {
                                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                                    {
                                        float alpha = 1 - (distanceFromCenter / brushRadius);
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
                    PaintAtUV_Airbrush(currentUVPosition);
                }

                texture2D.Apply(); // ������ �ؽ�ó�� ����
                previousUVPosition = currentUVPosition; // ���� ��ġ�� ���� ��ġ�� ������Ʈ
            }
        }
        else
        {
            previousUVPosition = null; // ĵ������ ����� ���� ��ġ �ʱ�ȭ
        }
    }

    void PaintAtUV(Vector2 uvPosition) // �ϵ�Ÿ��
    {
        int texX = (int)(uvPosition.x * texture2D.width);
        int texY = (int)(uvPosition.y * texture2D.height);

        for (int x = -brushRadius; x <= brushRadius; x++)
        {
            for (int y = -brushRadius; y <= brushRadius; y++)
            {
                int px = texX + x;
                int py = texY + y;

                if (x * x + y * y <= brushRadius * brushRadius)
                {
                    if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                    {
                        Color originalColor = texture2D.GetPixel(px, py);
                        Color blendedColor = Color.Lerp(originalColor, paintColor, paintColor.a);

                        texture2D.SetPixel(px, py, blendedColor);
                    }
                }
            }
        }
    }

    void PaintAtUV_Airbrush(Vector2 uvPosition) // ����귯����
    {
        int texX = (int)(uvPosition.x * texture2D.width);
        int texY = (int)(uvPosition.y * texture2D.height);

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
        if (ColorUtility.TryParseHtmlString(colorHex.text.ToString(), out Color newColor))
        {
            paintColor = newColor;
            paintColor.a = alpha.value;
        }
    }

    public void SaveTexture()
    {
        byte[] bytes = texture2D.EncodeToPNG();
        string filePath = Path.Combine(Application.dataPath, "SavedTexture.png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Texture saved to {filePath}");
    }

    public void SizeChange()
    {
        brushRadius = (int)size.value;
    }

    public void AlphaChange()
    {
        paintColor.a = alpha.value;
    }

    Vector3 GetPlaneSizeInWorldUnits(GameObject plane)
    {
        Vector3 scale = plane.transform.localScale;
        float planeWidth = scale.x * 10.0f;
        float planeHeight = scale.z * 10.0f;

        return new Vector3(planeWidth, 1.0f, planeHeight);
    }
}
