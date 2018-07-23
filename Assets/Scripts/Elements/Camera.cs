using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour {

    public WebCamTexture mCamera;
    public Image iCamera;

    private Texture2D tex;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Script has been started");

        mCamera = new WebCamTexture();
        mCamera.Play();
        Debug.Log("test");
        tex = new Texture2D(mCamera.width, mCamera.height);
        IntPtr pointer = mCamera.GetNativeTexturePtr();
        tex.UpdateExternalTexture(pointer);

        iCamera.sprite = Sprite.Create(tex, new Rect(0, 0, mCamera.width, mCamera.height), new Vector2(0, 0));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
