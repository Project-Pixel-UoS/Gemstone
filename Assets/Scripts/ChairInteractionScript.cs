using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class ChairInteractionScript : MonoBehaviour
{
    public GameManager gameManager;

    public void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.OnChairClicked();
        }
    }
}



