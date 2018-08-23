using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraViewScreenShot : MonoBehaviour {
    public int _width = 7000;
    public int _height = 5000;

    private bool _screenShot = true;

    public static string ScreenShotName(int width, int height) {
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                             Application.persistentDataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot() {
        _screenShot = true;
    }

    void LateUpdate() {
        Camera camera = Camera.main;
        _screenShot = Input.GetKeyDown("k");
        if (_screenShot) {
            _width = Screen.currentResolution.width * 5;
            _height = Screen.currentResolution.height * 5;
            _screenShot = false;
            RenderTexture rt = new RenderTexture(_width, _height, 24);
            camera.targetTexture = rt;

            Texture2D image = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
            camera.Render();
            RenderTexture.active = rt;
            image.ReadPixels(new Rect(0, 0, _width, _height), 0, 0);
            image.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);

            byte[] bytes = image.EncodeToPNG();
            string filename = ScreenShotName(_width, _height);
            if (File.Exists(filename)) {
                File.WriteAllBytes(filename, bytes);
            } else {
                Directory.CreateDirectory(Application.persistentDataPath + "/Screenshots/");
                File.WriteAllBytes(filename, bytes);
            }

            Debug.Log(string.Format("Took screenshot to: {0}", filename));

            Debug.Log("Capture!!");
        }
    }
}
