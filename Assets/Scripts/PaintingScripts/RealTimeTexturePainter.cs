using UnityEngine;
using UnityEngine.UI;

public class RealTimeTexturePainter : MonoBehaviour
{
    public Material materialToModify;    // 수정할 material
    private Texture2D texture2D;         // 텍스처를 저장할 Texture2D
    public Color paintColor = Color.red; // 페인트 색상 설정
    public int brushRadius = 2;          // 브러시의 반경 (픽셀)
    public GameObject canvasPlane;       // 페인팅할 Plane 오브젝트

    //사이즈 변경용 슬라이더
    public Slider size;

    private Vector2? previousUVPosition = null; // 이전 프레임의 UV 좌표

    void Start()
    {
        // Plane의 월드 좌표계에서의 크기를 계산
        Vector3 planeSize = GetPlaneSizeInWorldUnits(canvasPlane);

        // Plane 크기에 맞춰 텍스처 크기를 설정
        int textureWidth = Mathf.CeilToInt(planeSize.x * 150);
        int textureHeight = Mathf.CeilToInt(planeSize.z * 150);

        // Texture2D 생성
        texture2D = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        // 흰색으로 텍스처 초기화
        Color[] fillColorArray = texture2D.GetPixels();

        for (int i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = Color.white;
        }
        texture2D.SetPixels(fillColorArray);
        texture2D.Apply();

        // Material에 초기 텍스처 적용
        materialToModify.mainTexture = texture2D;

        // Plane에 MeshCollider가 없다면 추가
        if (canvasPlane.GetComponent<MeshCollider>() == null)
        {
            canvasPlane.AddComponent<MeshCollider>();
        }
    }

    void Update()
    {
        // 마우스가 클릭된 상태에서만 페인팅 수행
        if (Input.GetMouseButton(0))
        {
            PaintOnTexture();
        }
        else
        {
            // 마우스를 클릭하지 않았을 경우, 이전 위치를 초기화
            previousUVPosition = null;
        }
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
                Vector2 currentUVPosition = hit.textureCoord;

                // 이전 UV 좌표가 있다면 두 좌표 사이를 채워줌
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;

                    // 두 좌표 사이의 거리를 구함
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = 15; //Mathf.CeilToInt(distance * texture2D.width); // 보간할 스텝 수

                    for (int i = 0; i <= steps; i++)
                    {
                        // 두 좌표 사이를 보간
                        Vector2 lerpUV = Vector2.Lerp(previousUV, currentUVPosition, (float)i / steps);

                        // 보간된 위치에서 페인트 칠하기
                        int texX = (int)(lerpUV.x * texture2D.width);
                        int texY = (int)(lerpUV.y * texture2D.height);

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
                    }
                }

                texture2D.Apply(); // 수정된 텍스처를 적용

                // 현재 위치를 이전 위치로 업데이트
                previousUVPosition = currentUVPosition;
            }
        }
        else
        {
            //캔버스를 벗어나면 이전위치 초기화
            previousUVPosition = null;

        }
    }

    public void SizeChange()
    {
        brushRadius =(int) size.value;
    }
    Vector3 GetPlaneSizeInWorldUnits(GameObject plane)
    {
        // Plane의 기본 크기는 10x10 유닛입니다. 이를 기반으로 Plane의 실제 크기를 계산합니다.
        Vector3 scale = plane.transform.localScale;
        float planeWidth = scale.x * 10.0f;
        float planeHeight = scale.z * 10.0f;

        return new Vector3(planeWidth, 1.0f, planeHeight);
    }
}
