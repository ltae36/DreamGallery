using Photon.Pun; // Photon 관련 라이브러리 추가
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RealTimeTexturePainter : MonoBehaviourPun // MonoBehaviourPun으로 변경
{
    public enum BrushStyle
    {
        Airbrush,
        HardType,
    }

    public BrushStyle brush;
    public bool canPaint = true;

    public int paintCount = 0;
    public int questionStartcount = 500;

    public GameObject paintingPlane;
    public Text userInput;
    public GameObject ChatUI;
    public Text question;

    public Material materialToModify;
    private Texture2D texture2D;
    public Color paintColor = Color.red; // Color는 개별 요소로 나눠서 전송할 필요가 있음
    public int brushRadius = 2;
    public GameObject canvasPlane;

    public int resolution = 100;

    public Slider alpha;
    public Slider size;
    public InputField colorHex;

    private Vector2? previousUVPosition = null;

    private PhotonView pv; // PhotonView 추가

    void Start()
    {
        pv = GetComponent<PhotonView>(); // PhotonView 초기화
    }

    [PunRPC]
    void RPC_CanvasChange(int planeViewID)
    {
        // 받은 ViewID를 통해 평면 오브젝트 가져오기
        GameObject newCanvasPlane = PhotonView.Find(planeViewID).gameObject;

        if (newCanvasPlane != null)
        {
            canvasPlane = newCanvasPlane;
            materialToModify = canvasPlane.GetComponent<MeshRenderer>().material;
            CanvasChange();  // 동기화된 캔버스에 변경사항 적용
        }
    }
    void CanvasChange()
    {
        if (materialToModify == null)
        {
            materialToModify = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            Renderer renderer = canvasPlane.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialToModify;
            }
        }

        if (materialToModify.mainTexture != null)
        {
            texture2D = materialToModify.mainTexture as Texture2D;
        }
        else
        {
            Vector3 planeSize = GetPlaneSizeInWorldUnits(canvasPlane);
            int textureWidth = Mathf.CeilToInt(planeSize.x * resolution);
            int textureHeight = Mathf.CeilToInt(planeSize.z * resolution);
            texture2D = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

            Color[] fillColorArray = texture2D.GetPixels();
            for (int i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.white;
            }
            texture2D.SetPixels(fillColorArray);
            texture2D.Apply();

            materialToModify.mainTexture = texture2D;

            if (canvasPlane.GetComponent<MeshCollider>() == null)
            {
                canvasPlane.AddComponent<MeshCollider>();
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))  // 오른쪽 마우스 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Plane"))  // Plane 태그가 있는 오브젝트 클릭
                {
                    paintingPlane = hit.transform.gameObject;

                    if (hit.transform.gameObject != canvasPlane)  // 새로운 Plane을 클릭했을 때
                    {
                        canvasPlane = hit.transform.gameObject;
                        materialToModify = hit.transform.gameObject.GetComponent<MeshRenderer>().material;

                        // 모든 클라이언트에게 canvasPlane 변경을 동기화
                        int planeViewID = canvasPlane.GetPhotonView().ViewID;
                        pv.RPC("RPC_CanvasChange", RpcTarget.AllBuffered, planeViewID);
                    }
                }
            }
        }
    }
    void FixedUpdate()
    {
        //if (paintCount >= questionStartcount)
        //{
        //    print("AI에 지금까지 그린 그림 전송");
        //}

        if (ChatUI.activeInHierarchy)
        {
            canPaint = false;
        }
        else
        {
            canPaint = true;
        }

        if (!canPaint)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) brush = BrushStyle.HardType;
        if (Input.GetKeyDown(KeyCode.Alpha2)) brush = BrushStyle.Airbrush;

        if (true) 
        {
            switch (brush)
            {
                case BrushStyle.Airbrush:
                    if (Input.GetMouseButton(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject == canvasPlane)
                            {
                                Vector2 uvPosition = hit.textureCoord;
                                // Color 값을 개별 요소로 나눠서 전송
                                pv.RPC("RPC_PaintAirbrush", RpcTarget.All, uvPosition.x, uvPosition.y, brushRadius, paintColor.r, paintColor.g, paintColor.b, paintColor.a);
                            }
                        }
                    }
                    else
                    {
                        previousUVPosition = null;
                    }
                    break;

                case BrushStyle.HardType:
                    if (Input.GetMouseButton(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject == canvasPlane)
                            {
                                Vector2 uvPosition = hit.textureCoord;
                                // Color 값을 개별 요소로 나눠서 전송
                                pv.RPC("RPC_PaintHardType", RpcTarget.All, uvPosition.x, uvPosition.y, brushRadius, paintColor.r, paintColor.g, paintColor.b, paintColor.a);
                            }
                        }
                    }
                    else
                    {
                        previousUVPosition = null;
                    }
                    break;
            }
        }
    }

    // 수신된 float 값을 Color로 변환하여 사용
    [PunRPC]
    void RPC_PaintAirbrush(float uvX, float uvY, int brushRadius, float r, float g, float b, float a)
    {
        Vector2 uvPosition = new Vector2(uvX, uvY);
        Color color = new Color(r, g, b, a); // 받은 float 값을 Color로 변환
        PaintOnTexture_Airbrush(uvPosition, brushRadius, color);
    }

    // 수신된 float 값을 Color로 변환하여 사용
    [PunRPC]
    void RPC_PaintHardType(float uvX, float uvY, int brushRadius, float r, float g, float b, float a)
    {
        Vector2 uvPosition = new Vector2(uvX, uvY);
        Color color = new Color(r, g, b, a); // 받은 float 값을 Color로 변환
        PaintOnTexture(uvPosition, brushRadius, color);
    }

    void PaintOnTexture(Vector2 uvPosition, int brushRadius, Color color)
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
                        Color blendedColor = Color.Lerp(originalColor, color, color.a);
                        texture2D.SetPixel(px, py, blendedColor);
                    }
                }
            }
        }
        texture2D.Apply(); // 텍스처 업데이트
        paintingPlane.GetComponent<PlaneScript>().paintedCount++;
        
    }

    void PaintOnTexture_Airbrush(Vector2 uvPosition, int brushRadius, Color color)
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
                        Color blendedColor = Color.Lerp(originalColor, color, alpha * color.a);
                        texture2D.SetPixel(px, py, blendedColor);
                    }
                }
            }
        }
        texture2D.Apply(); // 텍스처 업데이트
        paintingPlane.GetComponent<PlaneScript>().paintedCount++;


    }

    public void ColorChange()
    {
        if (ColorUtility.TryParseHtmlString(colorHex.text.ToString(), out Color newColor))
        {
            paintColor = newColor;
            paintColor.a = alpha.value;
        }
    }

    public string picName;
    public void SaveTexture()
    {
        int numbering = 1;
        string filePath = Path.Combine(Application.streamingAssetsPath, "pic" + numbering.ToString() + ".png");

        while (File.Exists(filePath))
        {
            numbering++;
            filePath = Path.Combine(Application.streamingAssetsPath, "pic" + numbering.ToString() + ".png");
            picName = "pic" + numbering.ToString();
        }

        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Texture saved to {filePath}");
        StartCoroutine(UploadTextureToServer(bytes));
    }

    IEnumerator UploadTextureToServer(byte[] pngData)
    {
        string uploadURL = "https://7126-59-13-225-125.ngrok-free.app/save_image";

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", pngData, picName + ".png", "image/png");

        UnityWebRequest www = UnityWebRequest.Post(uploadURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Upload failed: {www.error}");
        }
        else
        {
            Debug.Log("Upload successful!");
        }
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