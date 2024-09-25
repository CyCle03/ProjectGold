using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera _camera;

    // Start is called before the first frame update
    private void LateUpdate()
    {
        transform.forward = _camera.transform.forward;
    }
}
