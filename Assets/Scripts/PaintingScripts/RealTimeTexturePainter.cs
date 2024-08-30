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

    public Material materialToModify;    // 수정할 material
    private Texture2D texture2D;         // 텍스처를 저장할 Texture2D
    public Color paintColor = Color.red; // 페인트 색상 설정
    public int brushRadius = 2;          // 브러시의 반경 (픽셀)
    public GameObject canvasPlane;       // 페인팅할 Plane 오브젝트

    //알파 변경 슬라이더
    public Slider alpha;
    //사이즈 변경용 슬라이더
    public Slider size;
    //색상 변경용
    public InputField colorHex;

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
                    // 마우스를 클릭하지 않았을 경우, 이전 위치를 초기화
                    previousUVPosition = null;
                }

                break;
            case BrushStyle.HardType:
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
                break;
            default:
                break;
        }
       
        //print(colorHex.text);
    }

    void PaintOnTexture()//하드타입

    {
        // 마우스 포인터의 위치를 가져옴
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Plane과의 충돌 여부 확인
        if (Physics.Raycast(ray, out hit))
        {
                Vector2 currentUVPosition = hit.textureCoord;

            if (hit.collider.gameObject == canvasPlane)
            {
                // 충돌한 지점의 UV 좌표 얻기

                // 이전 UV 좌표가 있다면 두 좌표 사이를 채워줌
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;

                    // 두 좌표 사이의 거리를 구함
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance/brushRadius)*10; // 보간할 스텝 수

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
                                        // 기존 색상과 새로운 색상을 혼합
                                        Color originalColor = texture2D.GetPixel(px, py);
                                        Color blendedColor = Color.Lerp(originalColor, paintColor, paintColor.a); // 혼합 비율은 새로운 색상의 알파값에 따라 결정

                                        // 혼합된 색상을 텍스처에 적용
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

    void PaintOnTexture_Airbrush()//애어브러쉬

    {
        // 마우스 포인터의 위치를 가져옴
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Plane과의 충돌 여부 확인
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 currentUVPosition = hit.textureCoord;

            if (hit.collider.gameObject == canvasPlane)
            {
                // 충돌한 지점의 UV 좌표 얻기

                // 이전 UV 좌표가 있다면 두 좌표 사이를 채워줌
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;

                    // 두 좌표 사이의 거리를 구함
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance / brushRadius) * 10; // 보간할 스텝 수

                    for (int i = 0; i <= steps; i++)
                    {
                        // 두 좌표 사이를 보간
                        Vector2 lerpUV = Vector2.Lerp(previousUV, currentUVPosition, (float)i / steps);

                        // 보간된 위치에서 페인트 칠하기
                        int texX = (int)(lerpUV.x * texture2D.width);
                        int texY = (int)(lerpUV.y * texture2D.height);

                        // 브러시 반경만큼의 픽셀을 칠함
                        // 브러시 반경만큼의 픽셀을 칠함
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
                                        // 거리 기반으로 알파값 계산 (가운데는 진하고, 가장자리로 갈수록 연해짐)
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

    void PaintAtUV(Vector2 uvPosition)//하드타입
    {
        // UV 좌표를 텍스처 좌표로 변환
        int texX = (int)(uvPosition.x * texture2D.width);
        int texY = (int)(uvPosition.y * texture2D.height);

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
                        // 기존 색상과 새로운 색상을 혼합
                        Color originalColor = texture2D.GetPixel(px, py);
                        Color blendedColor = Color.Lerp(originalColor, paintColor, paintColor.a); // 혼합 비율은 새로운 색상의 알파값에 따라 결정

                        // 혼합된 색상을 텍스처에 적용
                        texture2D.SetPixel(px, py, blendedColor);
                        // texture2D.SetPixel(px, py, paintColor);
                    }
                }
            }
        }
    }




    void PaintAtUV_Airbrush(Vector2 uvPosition)//에어브러쉬용
    {
        // UV 좌표를 텍스처 좌표로 변환
        int texX = (int)(uvPosition.x * texture2D.width);
        int texY = (int)(uvPosition.y * texture2D.height);

        // 브러시 반경만큼의 픽셀을 칠함
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
                        // 거리 기반으로 알파값 계산 (가운데는 진하고, 가장자리로 갈수록 연해짐)
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
       // print("실행됨");
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
        // Texture2D의 내용을 PNG 포맷으로 변환
        byte[] bytes = texture2D.EncodeToPNG();

        // 저장할 파일 경로 설정
        string filePath = Path.Combine(Application.dataPath, "SavedTexture.png");

        // 파일로 저장
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Texture saved to {filePath}");
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
    public void AlphaChange()
    {
        paintColor.a = alpha.value;
    }

}
