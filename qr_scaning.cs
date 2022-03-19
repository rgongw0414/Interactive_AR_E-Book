using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ZXing;
using UnityEngine.UI;

public class qr_scaning : MonoBehaviour
{
    public RawImage cameraTexture; // 即時預覽相機畫面
    public Text txtQRcode; // 掃完 QR-CODE 後，顯示裡面的字串
    private WebCamTexture webCameraTexture;
    //private BarcodeReader barcodeReader;

    /*IEnumerator Start()
    {
        barcodeReader = new BarcodeReader();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            string devicename = devices[0].name;
            webCameraTexture = new WebCamTexture(devicename, 800, 600);
            cameraTexture.texture = webCameraTexture;
            webCameraTexture.Play();

            txtQRcode.text = "相機解析度 : " + webCameraTexture.width + "x" + webCameraTexture.height;

            InvokeRepeating("DecodeQR", 0, 0.5f); // 0.5 秒掃描一次
        }
    }*/

   /* private void DecodeQR()
    {
        var br = barcodeReader.Decode(webCameraTexture.GetPixels32(), webCameraTexture.width, webCameraTexture.height);
        if (br != null)
        {
            txtQRcode.text = br.Text;
            webCameraTexture.Stop();
        }
    }*/
}