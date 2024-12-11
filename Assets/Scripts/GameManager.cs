using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cafeCanvas;
    public GameObject chair;
    public GameObject table;
    public GameObject store;


    void Start()
    {
        cafeCanvas.SetActive(true);
        table.SetActive(false);
        store.SetActive(false);
    }

    public void OnChairClicked()
    {
        cafeCanvas.SetActive(false);
        table.SetActive(true);
    }
    public void OnStoreClicked()
    {
        cafeCanvas.SetActive(false);
        store.SetActive(true);
    }
}
