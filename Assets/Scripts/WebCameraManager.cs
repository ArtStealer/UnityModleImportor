using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCameraManager : MonoBehaviour
{

    public string DeviceName;
    public Vector2 CameraSize;
    public float CameraFPS;

    //接收返回的图片数据  
    WebCamTexture _webCamera;
    public GameObject Plane;//作为显示摄像头的面板


    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "Initialize Camera"))
        {
            StartCoroutine("InitCameraCor");
        }

        //添加一个按钮来控制摄像机的开和关
        if (GUI.Button(new Rect(100, 250, 100, 100), "ON/OFF"))
        {
            if (_webCamera != null && Plane != null)
            {

                if (_webCamera.isPlaying)
                    StopCamera();
                else
                    PlayCamera();
            }
        }
        if (GUI.Button(new Rect(100, 450, 100, 100), "Quit"))
        {

            Application.Quit();
        }

    }

    public void PlayCamera()
    {
        Plane.GetComponent<MeshRenderer>().enabled = true;
        _webCamera.Play();
    }


    public void StopCamera()
    {
        Plane.GetComponent<MeshRenderer>().enabled = false;
        _webCamera.Stop();
    }

    /// <summary>  
    /// 初始化摄像头
    /// </summary>  
    public IEnumerator InitCameraCor()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            DeviceName = devices[0].name;
            _webCamera = new WebCamTexture(DeviceName, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);

            Plane.GetComponent<Renderer>().material.mainTexture = _webCamera;
            Plane.transform.localScale = new Vector3(1, 1, 1);

            _webCamera.Play();
        }
    }
}
