using UnityEngine;
using Util;


public class Bathroom : MonoBehaviour
{
    private GameObject panel, bathroom, withSmoker, withoutSmoker;

    private void OnEnable()
    {
        panel = GameObject.FindWithTag("GameStage");
        bathroom = panel.transform.Find("Room: Bathroom")?.gameObject;
        withSmoker = bathroom.transform.Find("WithSmoker")?.gameObject;
        withoutSmoker = bathroom.transform.Find("WithoutSmoker")?.gameObject;
        withSmoker.SetActive(true);
        withoutSmoker.SetActive(false);
    }
    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
            if (clickedItem != null && clickedItem.gameObject.name == "Smoker")
            {
                withSmoker.SetActive(false);
                withoutSmoker.SetActive(true);
            }

        }
    }
}
