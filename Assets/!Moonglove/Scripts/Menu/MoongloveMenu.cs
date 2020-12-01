using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeppukuStudio
{

    public class MoongloveMenu : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.anyKey)
                SceneTransition.Instance.LoadLevel(1);

        }
    }
}