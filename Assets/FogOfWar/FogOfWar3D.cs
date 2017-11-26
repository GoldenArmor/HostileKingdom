using UnityEngine;

public class FogOfWar3D : MonoBehaviour
{
    public Transform[] m_viewer = null;
	public Vector3[] m_viewerCenter = new Vector3[4];
	public Transform m_topLeft = null;
	public Transform m_topRight = null;
	public Transform m_bottomLeft = null;
	public Transform m_bottomRight = null;
	public LayerMask m_fogCoverLayer = 0;
    public int m_fogOfWarTexSize = 256;
    public Vector3 m_projecterPosition = Vector3.zero;
    public Vector3 m_projecterEulerAngle = new Vector3(90,0,0);
	[Range(0.0f,2.0f)]
	public float m_brushSize = 0.4f;
	[Range(0.0f, 1.0f)]
	public float m_edgeSmoothValue = 0.1f;
	[Range(0.0f, 1.0f)]
	public float m_fogDensity = 0.9f;
	
	Matrix4x4 m_viewMatrix = Matrix4x4.identity;
	Matrix4x4 m_projMatrix = Matrix4x4.identity;

    RenderTexture m_warFogRTT = null;
    Material m_fogOfWarMaterial = null;
    Material m_fogOfWarCastMaterial = null;
    Projector m_projectorFogCast = null;
    Texture2D m_perlinNoise = null;

    float m_areaAspect = 1.0f;
    void Start()
    {
        CreateMaterial();
        CreateWarFogRTT(m_fogOfWarTexSize, m_fogOfWarTexSize, 1);
        CreateCastProjecter();
		CreateFogOfWarProjecterMatrix ();
        Matrix4x4 matVP = GL.GetGPUProjectionMatrix(m_projMatrix, true) * m_viewMatrix;
        m_projectorFogCast.material.SetTexture("_FogOfWarTex", m_warFogRTT);
        m_projectorFogCast.material.SetMatrix("_MatCastViewProj", matVP);
		
        if (m_perlinNoise == null)
            CreatePerlinNoise(ref m_perlinNoise, m_fogOfWarTexSize, m_fogOfWarTexSize, 10, Vector2.zero);
        m_fogOfWarMaterial.SetTexture("_NoiseTex", m_perlinNoise);
        m_fogOfWarMaterial.SetFloat("_BufferAspect", m_areaAspect);
        m_fogOfWarMaterial.SetFloat("_FogDensity", m_fogDensity);
    }

	void OnPreRender()
	{
		if (m_fogOfWarMaterial !=null)
		{
            Vector3[] viewerCenter = new Vector3[m_viewerCenter.Length];

            for (int i = 0; i < viewerCenter.Length; i++)
            {
                viewerCenter[i] = m_viewerCenter[i];
            }
            for (int i = 0; i < m_viewer.Length; i++)
            {
                if (m_viewer[i] != null)
                {
                    viewerCenter[i] = m_viewer[i].position;
                }
            }

            Vector4[] viewCenter = new Vector4[viewerCenter.Length];
            for (int i = 0; i < viewCenter.Length; i++)
            {
                viewCenter[i] = TransformWorldCoordToUVCoord(viewerCenter[i]);
                viewCenter[i].z = m_brushSize;
                viewCenter[i].w = m_edgeSmoothValue;
                m_fogOfWarMaterial.SetVector("_ViewCenter", viewCenter[i]);
            }
            //debug
            m_fogOfWarMaterial.SetTexture("_NoiseTex", m_perlinNoise);
            m_fogOfWarMaterial.SetFloat("_BufferAspect", m_areaAspect);
            m_fogOfWarMaterial.SetFloat("_FogDensity", m_fogDensity);

            RenderTexture ac = RenderTexture.active;
            RenderTexture.active = m_warFogRTT;
            GL.Clear(false, false, Color.black);
            Graphics.Blit(null, m_warFogRTT, m_fogOfWarMaterial);
            RenderTexture.active = ac;
        }
	}

    void OnDestroy()
    {
        if (m_warFogRTT != null)
        {
            Destroy(m_warFogRTT);
            m_warFogRTT = null;
        }
        if (m_perlinNoise != null)
        {
            Destroy(m_perlinNoise);
            m_perlinNoise = null;
        }
        if (m_projectorFogCast != null)
        {
            Destroy(m_projectorFogCast.gameObject);
            m_projectorFogCast = null;
        }
    }

    Vector4 TransformWorldCoordToUVCoord(Vector3 v3PosInWorld)
    {
        Vector4 ret = Vector4.zero;
		Matrix4x4 matVP = GL.GetGPUProjectionMatrix(m_projMatrix, true) * m_viewMatrix;
		Vector3 v3PosInProjSpace = matVP.MultiplyPoint(v3PosInWorld);
		ret = new Vector4(0.5f * v3PosInProjSpace.x + 0.5f, 0.5f * v3PosInProjSpace.y + 0.5f, 0.0f, 0.0f);
        return ret;
    }

    void CreateMaterial()
    {
        m_fogOfWarMaterial = new Material(Shader.Find("Assets/FogOfWar/FogOfWar"));
        m_fogOfWarCastMaterial = new Material(Shader.Find("Assets/FogOfWar/FogOfWarCast"));
    }

    void CreateWarFogRTT(int w, int h, int antiAliasing)
    {
        if (m_warFogRTT == null)
        {
            m_warFogRTT = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32);
            m_warFogRTT.wrapMode = TextureWrapMode.Clamp;
            m_warFogRTT.name = "Fog Of War RTT";
            m_warFogRTT.antiAliasing = antiAliasing;

            RenderTexture mainRTT = RenderTexture.active;
            RenderTexture.active = m_warFogRTT;
            GL.Clear(true, true, new Color(0, 0, 0, 1), 1.0f);
            RenderTexture.active = mainRTT;
        }
    }

    void CreateCastProjecter()
    {
        if (m_projectorFogCast == null)
        {
            GameObject gameObjCast = new GameObject("FogOfWarCastProjector");
            gameObjCast.transform.localPosition = Vector3.zero;
            gameObjCast.transform.localRotation = new Quaternion(0, 0, 0, 1);
            gameObjCast.transform.localScale = Vector3.one;
            gameObjCast.transform.Rotate(m_projecterEulerAngle, Space.Self);
            gameObjCast.transform.position = m_projecterPosition;

            m_projectorFogCast = gameObjCast.AddComponent<Projector>();
            m_projectorFogCast.orthographic = true;
            m_projectorFogCast.orthographicSize = 100.0f;
            m_projectorFogCast.nearClipPlane = -100f;
            m_projectorFogCast.farClipPlane = 100.0f;
            m_projectorFogCast.material = m_fogOfWarCastMaterial;
            m_projectorFogCast.ignoreLayers = ~m_fogCoverLayer;
            m_projectorFogCast.enabled = true;
        }
    }

	Vector3[] vertices = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
	void CreateFogOfWarProjecterMatrix()
	{
		Vector3 look = m_projectorFogCast.transform.rotation * new Vector3(0,0,-1);
		Vector3 up = m_projectorFogCast.transform.rotation * new Vector3(0,1,0);
		Vector3 right = Vector3.Cross (look, up);
		CreateViewMatrix (ref  m_viewMatrix, look, up, right, m_projectorFogCast.transform.position);
	
		//计算视图空间Area的AABB
		vertices [0] = m_topLeft.position;
		vertices [1] = m_bottomLeft.position;
		vertices [2] = m_bottomRight.position;
		vertices [3] = m_topRight.position;

		Vector3 v3MaxPosition = -Vector3.one * 500000.0f;
		Vector3 v3MinPosition = Vector3.one * 500000.0f;	
		for (int vertId = 0; vertId < 4; ++vertId)
		{
			// Light view space
			Vector3 v3Position = m_viewMatrix.MultiplyPoint3x4(vertices[vertId]);
			if (v3Position.x > v3MaxPosition.x)
			{
				v3MaxPosition.x = v3Position.x;
			}
			if (v3Position.y > v3MaxPosition.y)
			{
				v3MaxPosition.y = v3Position.y;
			}
			if (v3Position.z > v3MaxPosition.z)
			{
				v3MaxPosition.z = v3Position.z;
			}
			if (v3Position.x < v3MinPosition.x)
			{
				v3MinPosition.x = v3Position.x;
			}
			if (v3Position.y < v3MinPosition.y)
			{
				v3MinPosition.y = v3Position.y;
			}
			if (v3Position.z < v3MinPosition.z)
			{
				v3MinPosition.z = v3Position.z;
			}
		}
		CreateOrthogonalProjectMatrix (ref m_projMatrix, v3MaxPosition, v3MinPosition);

        m_areaAspect = (v3MaxPosition.x - v3MinPosition.x) / (v3MaxPosition.y - v3MinPosition.y);
	}

	
	//创建正交投影矩阵
	void CreateOrthogonalProjectMatrix(ref Matrix4x4 projectMatrix,Vector3 v3MaxInViewSpace, Vector3 v3MinInViewSpace)
	{
		float scaleX, scaleY, scaleZ;
		float offsetX, offsetY, offsetZ;
		scaleX = 2.0f / (v3MaxInViewSpace.x - v3MinInViewSpace.x);
		scaleY = 2.0f / (v3MaxInViewSpace.y - v3MinInViewSpace.y);
		offsetX = -0.5f * (v3MaxInViewSpace.x + v3MinInViewSpace.x) * scaleX;
		offsetY = -0.5f * (v3MaxInViewSpace.y + v3MinInViewSpace.y) * scaleY;
		scaleZ = 1.0f / (v3MaxInViewSpace.z - v3MinInViewSpace.z);
		offsetZ = -v3MinInViewSpace.z * scaleZ;
		
		//列矩阵
		projectMatrix.m00 = scaleX; projectMatrix.m01 = 0.0f; projectMatrix.m02 = 0.0f; projectMatrix.m03 = offsetX;
		projectMatrix.m10 = 0.0f; projectMatrix.m11 = scaleY; projectMatrix.m12 = 0.0f; projectMatrix.m13 = offsetY;
		projectMatrix.m20 = 0.0f; projectMatrix.m21 = 0.0f; projectMatrix.m22 = scaleZ; projectMatrix.m23 = offsetZ;
		projectMatrix.m30 = 0.0f; projectMatrix.m31 = 0.0f; projectMatrix.m32 = 0.0f; projectMatrix.m33 = 1.0f;
	}

	//创建视图矩阵
	void CreateViewMatrix(ref Matrix4x4 viewMatrix,Vector3 look,Vector3 up,Vector3 right,Vector3 pos)
	{
		look.Normalize ();
		up.Normalize ();
		right.Normalize ();

		float x = -Vector3.Dot (right,pos);
		float y = -Vector3.Dot (up,pos);
		float z = -Vector3.Dot (look,pos);
		
		viewMatrix.m00 = right.x; viewMatrix.m10 = up.x; viewMatrix.m20 = look.x; viewMatrix.m30 = 0.0f;
		viewMatrix.m01 = right.y; viewMatrix.m11 = up.y; viewMatrix.m21 = look.y; viewMatrix.m31 = 0.0f;
		viewMatrix.m02 = right.z; viewMatrix.m12 = up.z; viewMatrix.m22 = look.z; viewMatrix.m32 = 0.0f;
		viewMatrix.m03 = x;       viewMatrix.m13 = y;    viewMatrix.m23 = z;      viewMatrix.m33 = 1.0f;
	}
    //---------------------------------------------Perlin 噪音-------------------------------------------------------------------------------
     void CreatePerlinNoise(ref Texture2D noise, int w, int h, float frequency, Vector2 seed)
    {
        float xOrg = seed.x;
        float yOrg = seed.y;
        Color[] randomColor = new Color[w * h];
        int y = 0;
        while (y < w)
        {
            int x = 0;
            while (x < h)
            {
                float xCoord = xOrg + (float)x / (float)w * frequency;
                float yCoord = yOrg + (float)y / (float)h * frequency;
                float sample = PerlinNoise2D(4, 1.0f, xCoord, yCoord) * 0.5f + 0.5f;
                randomColor[y + x * w] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noise = new Texture2D(w, h, TextureFormat.ARGB32, false, true);
        noise.filterMode = FilterMode.Bilinear;
        noise.wrapMode = TextureWrapMode.Repeat;
        noise.SetPixels(randomColor);
        noise.Apply();
    }

     float PerlinNoise2D(int octaves, float amp, float x, float y)
     {
         float noise = 0.0f;
         float persistence = 0.5f;
         float lacunarity = 2.0f;
         for (int i = 0; i < octaves; i++)
         {
             float frequency = Mathf.Pow(lacunarity, i);
             float amplitude = Mathf.Pow(persistence, i);
             noise += (InterpolateNoise2D(x * frequency, y * frequency) * amplitude);
         }
         return noise;
     }

     float InterpolateNoise2D(float x, float y)
     {
         int intergerX = (int)x;
         float fracX = x - intergerX;

         int intergerY = (int)y;
         float fracY = y - intergerY;

         float v1 = SmoothRandomNoise2D(intergerX, intergerY);
         float v2 = SmoothRandomNoise2D(intergerX + 1, intergerY);
         float v3 = SmoothRandomNoise2D(intergerX, intergerY + 1);
         float v4 = SmoothRandomNoise2D(intergerX + 1, intergerY + 1);

         float i1 = Interpolate(v1, v2, fracX);
         float i2 = Interpolate(v3, v4, fracX);

         return Interpolate(i1, i2, fracY);
     }
     float SmoothRandomNoise2D(int x, int y)
     {
         float corners = (RandomNoise2D(x - 1, y - 1) + RandomNoise2D(x + 1, y - 1) + RandomNoise2D(x - 1, y + 1) + RandomNoise2D(x + 1, y + 1)) / 16.0f;
         float slides = (RandomNoise2D(x, y - 1) + RandomNoise2D(x, y + 1) + RandomNoise2D(x - 1, y) + RandomNoise2D(x + 1, y)) / 8.0f;
         float center = RandomNoise2D(x, y) / 4.0f;
         return corners + slides + center;
     }

     float RandomNoise2D(int x, int y)
     {
         int n = x + y * 57;
         n = (n << 13) ^ n;
         return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
     }

     float Interpolate(float a, float b, float t)
     {
         float f = t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
         return a * (1 - f) + b * f;
     }
}
