using UnityEngine;

public class PaintTest : MonoBehaviour
{
    public RenderTexture renderTexture;  // ���� ���� �׸� RenderTexture
    public Material materialToModify;    // ������ material
    private Texture2D texture2D;         // �ؽ�ó�� ������ Texture2D
    public Color paintColor = Color.red; // ����Ʈ ���� ����
    public int brushRadius = 2;          // �귯���� �ݰ� (�ȼ�)
    public GameObject canvasPlane;       // �������� Plane ������Ʈ

    void Start()
    {
        // RenderTexture ũ�⿡ �´� Texture2D ����
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // RenderTexture�� �ʱ�ȭ (��� �������)
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        // Plane�� MeshCollider�� ���ٸ� �߰�
        if (canvasPlane.GetComponent<MeshCollider>() == null)
        {
            canvasPlane.AddComponent<MeshCollider>();
        }
    }

    void Update()
    {
        // RenderTexture���� �����͸� �о Texture2D�� ����
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // ���콺�� Ŭ���� ���¿����� ������ ����
        if (Input.GetMouseButton(0))
        {
            PaintOnTexture();
        }

        // ������ Texture2D�� RenderTexture�� �ٽ� ����
        Graphics.Blit(texture2D, renderTexture);

        // RenderTexture�� material�� ����
        materialToModify.mainTexture = renderTexture;

        RenderTexture.active = null;
    }

    void PaintOnTexture()
    {
        // ���콺 �������� ��ġ�� ������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Plane���� �浹 ���� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == canvasPlane)
            {
                // �浹�� ������ UV ��ǥ ���
                Vector2 textureCoord = hit.textureCoord;

                // UV ��ǥ�� �ؽ�ó ��ǥ�� ��ȯ
                int texX = (int)(textureCoord.x * texture2D.width);
                int texY = (int)(textureCoord.y * texture2D.height);

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
                                texture2D.SetPixel(px, py, paintColor);
                            }
                        }
                    }
                }

                texture2D.Apply(); // ������ �ؽ�ó�� ����
            }
        }
    }
}
