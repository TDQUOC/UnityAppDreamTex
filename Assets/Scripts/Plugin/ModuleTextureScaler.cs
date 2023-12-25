using UnityEngine;

public class ModuleTextureScaler : MonoBehaviour
{
    public static Texture2D ScaleTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
    {
        // Tạo một RenderTexture mới để chứa dữ liệu downscale
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        rt.filterMode = FilterMode.Bilinear;

        // Tạo một temporary texture và set active để chứa dữ liệu downscale
        RenderTexture.active = rt;

        // Tạo một temporary texture và set active để chứa dữ liệu downscale
        Graphics.Blit(sourceTexture, rt);

        // Tạo một Texture2D mới để chứa dữ liệu downscale
        Texture2D result = new Texture2D(targetWidth, targetHeight);

        // Đọc dữ liệu từ RenderTexture và set vào Texture2D
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        // Giải phóng các bộ nhớ RenderTexture và active RenderTexture trở lại null
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}