using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private Canvas[] canvasToModify;
        [SerializeField] private float planeDist;

        [ContextMenu("Update canvas")]
        public void UpdateCanvas()
        {
            for (int i = 0; i < canvasToModify.Length; i++)
            {
                canvasToModify[i].planeDistance = planeDist;
            }
        }

    }
}
