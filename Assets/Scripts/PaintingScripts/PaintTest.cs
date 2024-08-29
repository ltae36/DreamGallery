using UnityEngine;

public class PaintTest : MonoBehaviour
{
    public RenderTexture renderTexture;  // 현재 씬을 그릴 RenderTexture
    public Material materialToModify;    // 수정할 material
    private Texture2D texture2D;         // 텍스처를 저장할 Texture2D
    public Color paintColor = Color.red; // 페인트 색상 설정
    public int brushRadius = 2;          // 브러시의 반경 (픽셀)
    public GameObject canvasPlane;       // 페인팅할 Plane 오브젝트

    void Start()
    {
        // RenderTexture 크기에 맞는 Texture2D 생성
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // RenderTexture를 초기화 (흰색 배경으로)
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        // Plane에 MeshCollider가 없다면 추가
        if (canvasPlane.GetComponent<MeshCollider>() == null)
        {
            canvasPlane.AddComponent<MeshCollider>();
        }
    }

    void Update()
    {
        // RenderTexture에서 데이터를 읽어서 Texture2D에 복사
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // 마우스가 클릭된 상태에서만 페인팅 수행
        if (Input.GetMouseButton(0))
        {
            PaintOnTexture();
        }

        // 수정된 Texture2D를 RenderTexture에 다시 적용
        Graphics.Blit(texture2D, renderTexture);

        // RenderTexture를 material에 적용
        materialToModify.mainTexture = renderTexture;

        RenderTexture.active = null;
    }

    void PaintOnTexture()
    {
        // 마우스 포인터의 위치를 가져옴
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Plane과의 충돌 여부 확인
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == canvasPlane)
            {
                // 충돌한 지점의 UV 좌표 얻기
                Vector2 textureCoord = hit.textureCoord;

                // UV 좌표를 텍스처 좌표로 변환
                int texX = (int)(textureCoord.x * texture2D.width);
                int texY = (int)(textureCoord.y * texture2D.height);

                // 브러시 반경만큼의 픽셀을 칠함
                for (int x = -brushRadius; x <= brushRadius; x++)
                {
                    for (int y = -brushRadius; y <= brushRadius; y++)
                    {
                        int px = texX + x;
                        int py = texY + y;

                        // 원형 브러시 구현 (거리 체크)
                        if (x * x + y * y <= brushRadius * brushRadius)
                        {
                            // 텍스처 범위를 벗어나지 않는지 확인
                            if (px >= 0 && px < texture2D.width && py >= 0 && py < texture2D.height)
                            {
                                texture2D.SetPixel(px, py, paintColor);
                            }
                        }
                    }
                }

                texture2D.Apply(); // 수정된 텍스처를 적용
            }
        }
    }
}
