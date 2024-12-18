using UnityEngine;

namespace Util
{ 
    public class Utils : MonoBehaviour
    {
        public static bool IsMouseClicked()
        {
            return Input.GetMouseButtonDown(0);
        }

        /// <summary>
        /// calculates what the mouse is clicking on
        /// </summary>
        /// <param name="layerMask">the layer that the objects will collide w the raycast</param>
        /// <returns></returns>
        public static RaycastHit2D CalculateMouseDownRaycast(int layerMask)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return Physics2D.Raycast(mousePos, Vector2.zero, float.PositiveInfinity, layerMask);
        }
        public static bool CheckMousePosInsideStage(string tag)
        {
            var stage = GameObject.FindGameObjectWithTag(tag);
            Vector3[] corners = GetItemCorners(stage);
            Vector2 bottomLeft = corners[0];
            Vector2 topRight = corners[2];
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mouseWorldPos.x > bottomLeft.x && mouseWorldPos.y > bottomLeft.y &&
                   mouseWorldPos.x < topRight.x && mouseWorldPos.y < topRight.y;
        }
        public static Vector3[] GetItemCorners(GameObject rect)
        {
            RectTransform rectTransform = rect.GetComponent<RectTransform>();
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            return worldCorners;
        }
    }
}

