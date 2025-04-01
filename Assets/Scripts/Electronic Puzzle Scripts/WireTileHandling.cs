using UnityEngine;

public class WireTileHandling : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        if (isDragging && !isCorrect)
        {
            transform.position = CheckPuzzleCompletion() + offset;
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
