using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{

    [SerializeField] private MinimapSettings settings;
    [SerializeField] private float cameraHeight;
    // Start is called before the first frame update
    private void Awake()
    {
        settings = GetComponentInParent<MinimapSettings>();
        cameraHeight = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPostion = settings.targetToFollow.transform.position;

        transform.position = new Vector3(targetPostion.x, cameraHeight, targetPostion.z);

        if (settings.rotateWithTheTarget) {
            Quaternion targetRotation = settings.targetToFollow.transform.rotation;

            transform.rotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, 0);

        }

    }
}
