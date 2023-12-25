using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

namespace Alta.Plugin
{
    [Flags]
    public enum FileType
    {
        PNG = 1,
        JPG = 2
    }

    public static class TextureExtension
    {
        /// <summary>
        /// convert RenderTexture to Texture2D
        /// </summary>
        /// <param name="rt">RenderTexture input</param>
        /// <param name="format">TextureFormat output</param>
        /// <returns>Texture2D</returns>
        public static Texture2D CopyRenderTexture(this RenderTexture rt, TextureFormat format = TextureFormat.ARGB32)
        {
            RenderTexture prevRT = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D texture = new Texture2D(rt.width, rt.height, format, false);

            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply(false, false);
            RenderTexture.active = prevRT;
            return texture;
        }

        /// <summary>
        /// save Texture2D to file
        /// </summary>
        /// <param name="texture">input Texture2D</param>
        /// <param name="filePrefixName">prefix of file</param>
        /// <param name="type">file type</param>
        /// <returns>list file path</returns>
        public static List<string> SaveTextureToFile(this Texture2D texture, string filePrefixName,
            FileType type = FileType.JPG)
        {
            List<string> fileNames = new List<string>();
            List<FileType> types = new List<FileType>();
            if (EnumerationExtensions.has(type, FileType.PNG))
            {
                fileNames.Add(string.Format("{0}.png", filePrefixName));
                types.Add(FileType.PNG);
            }

            if (EnumerationExtensions.has(type, FileType.JPG))
            {
                fileNames.Add(string.Format("{0}.jpg", filePrefixName));
                types.Add(FileType.JPG);
            }

            for (int i = 0; i < fileNames.Count; i++)
            {
                using (Stream s = File.Open(fileNames[i], FileMode.Create))
                {
                    byte[] bytes;
                    if (types[i] == FileType.PNG)
                    {
                        bytes = texture.EncodeToPNG();
                    }
                    else
                    {
                        bytes = texture.EncodeToJPG();
                    }

                    BinaryWriter binary = new BinaryWriter(s);
                    binary.Write(bytes);
                    s.Close();
                }
            }

            return fileNames;
        }

        /// <summary>
        /// flip texture2d
        /// </summary>
        /// <param name="original">input Texture2D</param>
        /// <param name="flipX">hozizontal flip</param>
        /// <param name="flipY">vertical flip</param>
        /// <returns>output Texture2D</returns>
        public static Texture2D FlipTexture(this Texture2D original, bool flipX, bool flipY)
        {
            Texture2D flipped = new Texture2D(original.width, original.height);

            int xN = original.width;
            int yN = original.height;


            if (flipY)
            {
                for (int x = 0; x < xN; x++)
                {
                    for (int y = 0; y < yN; y++)
                    {
                        if (flipY && !flipX)
                            flipped.SetPixel(x, yN - (y + 1), original.GetPixel(x, y));
                        else if (flipX && !flipY)
                            flipped.SetPixel(xN - (x + 1), y, original.GetPixel(x, y));
                        else if (flipX && flipY)
                            flipped.SetPixel(xN - (x + 1), yN - (y + 1), original.GetPixel(x, y));
                    }
                }
            }

            flipped.Apply();

            return flipped;
        }

        public static Texture2D rotateTexture(this Texture2D originalTexture, bool clockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = originalTexture.width;
            int h = originalTexture.height;

            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }

        public static Texture2D Crop(this Texture2D webcam, int x, int y, int w = -1, int h = -1)
        {
            if (x < 0 || y < 0)
                return null;
            if (w < 0)
            {
                w = webcam.width - x;
            }

            if (h < 0)
            {
                h = webcam.height - y;
            }

            Texture2D tex = new Texture2D(w, h);
            tex.SetPixels(webcam.GetPixels(x, y, w, h));
            tex.Apply();
            return tex;
        }

        public static Vector2 ScaleByHeight(Vector2 inputTexSize, Vector2 targetSize)
        {
            // set thẳng chiều cao
            // Nhân chéo chia ngang để ra chiều ngang
            // vd inputTex size là 1920x1080, cần đưa vào targetsize 300x200.
            // thì kết quả cho ra y = 200, x = 1920 x 200 / 1080
            Vector2 newSize = new Vector2();
            newSize.y = targetSize.y;
            newSize.x = inputTexSize.x * newSize.y / inputTexSize.y;
            return newSize;
        }

        public static Vector2 ScaleByWidth(Vector2 inputTexSize, Vector2 targetSize)
        {
            // set thẳng chiều ngang
            // Nhân chéo chia ngang để ra chiều cao
            // vd inputTex size là 1920x1080, cần đưa vào targetsize 300x200.
            // thì kết quả cho ra x = 300, x = 1080 x 300 / 200
            Vector2 newSize = new Vector2();
            newSize.x = targetSize.x;
            newSize.y = inputTexSize.y * newSize.x / inputTexSize.x;
            return newSize;
        }

        public static Vector2 ScaleAutoFit(Texture2D inputTex, Vector2 targetSize)
        {
            if (inputTex.width > inputTex.height)
            {
                // đây là tấm hình chữ nhật ngang => scale theo chiều ngang
                return ScaleByWidth(new Vector2(inputTex.width,inputTex.height), targetSize);
            }

            // đây là tấm hình chữ nhật đứng => sacle theo chiều cao
            return ScaleByHeight(new Vector2(inputTex.width,inputTex.height), targetSize);
        }
        
        public static Vector2 ScaleAutoFit(Sprite inputSprite, Vector2 targetSize)
        {
            Texture2D inputTex = inputSprite.texture;
            if (inputTex.width > inputTex.height)
            {
                // đây là tấm hình chữ nhật ngang => scale theo chiều ngang
                return ScaleByWidth(new Vector2(inputTex.width,inputTex.height), targetSize);
            }

            // đây là tấm hình chữ nhật đứng => sacle theo chiều cao
            return ScaleByHeight(new Vector2(inputTex.width,inputTex.height), targetSize);
        }
    }
}