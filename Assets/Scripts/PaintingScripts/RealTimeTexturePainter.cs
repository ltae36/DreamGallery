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

    //void Start()
    void CanvasChange()
    {
        // Material이 할당되지 않은 경우 새 Material 생성
        if (materialToModify == null)
        {
            // Universal Render Pipeline의 Unlit 쉐이더를 사용하는 새 Material 생성
            materialToModify = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            // 새로 생성한 Material을 Plane에 적용
            Renderer renderer = canvasPlane.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialToModify;
            }
        }

        // Material에 이미 텍스처가 있는지 확인
        if (materialToModify.mainTexture != null)
        {
            // 기존 텍스처가 있는 경우 이를 Texture2D로 캐스팅
            texture2D = materialToModify.mainTexture as Texture2D;
        }
        else
        {
            // 텍스처가 없는 경우 새로 생성
            Vector3 planeSize = GetPlaneSizeInWorldUnits(canvasPlane);
            int textureWidth = Mathf.CeilToInt(planeSize.x * 150);
            int textureHeight = Mathf.CeilToInt(planeSize.z * 150);
            texture2D = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            print((planeSize.x*100).ToString());
            print((planeSize.z*100).ToString());

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
                    //print("캔버스임");
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
                    // 마우스를 클릭하지 않았을 경우, 이전 위치를 초기화
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
                    // 마우스를 클릭하지 않았을 경우, 이전 위치를 초기화
                    previousUVPosition = null;
                }
                break;
        }
    }

    void PaintOnTexture() // 하드타입
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
                // 이전 UV 좌표가 있다면 두 좌표 사이를 채워줌
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance / brushRadius) * 10; // 보간할 스텝 수

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

                texture2D.Apply(); // 수정된 텍스처를 적용
                previousUVPosition = currentUVPosition; // 현재 위치를 이전 위치로 업데이트
            }
        }
        else
        {
            previousUVPosition = null; // 캔버스를 벗어나면 이전 위치 초기화
        }
    }

    void PaintOnTexture_Airbrush() // 에어브러쉬
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
                // 이전 UV 좌표가 있다면 두 좌표 사이를 채워줌
                if (previousUVPosition.HasValue)
                {
                    Vector2 previousUV = previousUVPosition.Value;
                    float distance = Vector2.Distance(previousUV, currentUVPosition);
                    int steps = Mathf.CeilToInt(distance / brushRadius) * 10; // 보간할 스텝 수

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

                texture2D.Apply(); // 수정된 텍스처를 적용
                previousUVPosition = currentUVPosition; // 현재 위치를 이전 위치로 업데이트
            }
        }
        else
        {
            previousUVPosition = null; // 캔버스를 벗어나면 이전 위치 초기화
        }
    }

    void PaintAtUV(Vector2 uvPosition) // 하드타입
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

    void PaintAtUV_Airbrush(Vector2 uvPosition) // 에어브러쉬용
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
        int numbering = 1;//나중에 1로 바꿔줘야됨

        // 파일 경로를 생성
        string filePath = Path.Combine(Application.streamingAssetsPath, "pic" + numbering.ToString() + ".png");

        // 동일한 파일이 존재하는지 확인하고, 존재하면 numbering을 증가시켜 새 경로를 생성
        while (File.Exists(filePath))
        {
            numbering++;
            filePath = Path.Combine(Application.streamingAssetsPath, "pic" + numbering.ToString() + ".png");
        }

        // Texture2D를 PNG 포맷으로 변환하여 파일로 저장
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Texture saved to {filePath}");
        //int numbering = 1; // 저장 파일 번호

        //// 파일 경로를 생성
        //string filePath = Path.Combine(Application.dataPath, "Resources", "pic" + numbering.ToString() + ".png");

        //// 동일한 파일이 존재하는지 확인하고, 존재하면 numbering을 증가시켜 새 경로를 생성
        //while (File.Exists(filePath))
        //{
        //    numbering++;
        //    filePath = Path.Combine(Application.dataPath, "Resources", "pic" + numbering.ToString() + ".png");
        //}

        //// Plane의 크기에 맞게 텍스처 크기 설정
        //Vector3 planeSize = GetPlaneSizeInWorldUnits(canvasPlane);
        //int textureWidth = Mathf.CeilToInt(planeSize.x * 150);
        //int textureHeight = Mathf.CeilToInt(planeSize.z * 150);

        //// 이미 만들어진 texture2D의 크기와 맞는지 확인 (생성 및 업데이트가 필요하다면 별도의 로직이 필요)
        //if (texture2D.width != textureWidth || texture2D.height != textureHeight)
        //{
        //    Debug.LogWarning("Current texture resolution does not match calculated resolution. Consider regenerating the texture.");
        //}

        //// Texture2D를 PNG 포맷으로 변환하여 파일로 저장
        //byte[] bytes = texture2D.EncodeToPNG();
        //File.WriteAllBytes(filePath, bytes);

        ////Application.streamingAssetsPath

        //Debug.Log($"Texture saved to {filePath}");

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
