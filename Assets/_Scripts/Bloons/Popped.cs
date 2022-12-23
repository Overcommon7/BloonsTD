public class Popped : UnityEngine.MonoBehaviour
{
    const float Time = 0.07f;
    float begin;

    private void Start()
    {
        begin = UnityEngine.Time.time;
    }
    void FixedUpdate()
    {
        if (UnityEngine.Time.time - begin >= Time) 
            Destroy(gameObject);
    }
}
