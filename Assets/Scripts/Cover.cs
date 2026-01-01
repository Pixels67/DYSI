using UnityEngine;

public class Cover : MonoBehaviour
{
    public void FadeIn()
    {
        GetComponent<Animation>().Play("FadeIn");
    }

    public void FadeOut()
    {
        GetComponent<Animation>().Play("FadeOut");
    }
}
