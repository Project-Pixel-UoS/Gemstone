using UnityEngine;
using UnityEngine.UI;

public class RoomPuzzleManagaer : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 correctPosition;
    private bool isCorrect = false;

    private float snapThreshold = 1f;
    private static int snappedPieces = 0; 
    private static int totalPieces = 15;

    void Start()
    {
        correctPosition = transform.position; 
        ScatterPieces(); 
    }

    void ScatterPieces()
    {
        if (gameObject.CompareTag("PuzzlePiece"))
        {
            float randomX = Random.Range(-4f, 4f);
            float randomY = Random.Range(-2f, 2f);
            transform.position = new Vector3(randomX, randomY, 0);
        }
    }

    private void OnMouseDown()
    {
        if (!isCorrect) 
        {
            offset = transform.position - CheckPuzzleCompletion();
            isDragging = true;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging && !isCorrect)
        {
            transform.position = CheckPuzzleCompletion() + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (Vector3.Distance(transform.position, correctPosition) < snapThreshold)
        {
            transform.position = correctPosition;
            isCorrect = true;
            snappedPieces++;

            /*if (snappedPieces == totalPieces)
            {
                ShowSuccessMessage();
            }*/
        }
    }

    private Vector3 CheckPuzzleCompletion()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    
}
