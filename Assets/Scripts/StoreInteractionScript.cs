using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StoreInteractionScript : MonoBehaviour
{
    public GameManager gameManager;

    public void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.OnStoreClicked();
        }
    }
}

