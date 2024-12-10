using UnityEngine;
public class ShowandHideObject : MonoBehaviour
{
    public GameObject self;

    public void showGameObject()
    {
        self.SetActive(true);
    }
    public void hideGameObject()
    {
        self.SetActive(false);
    }
}
