using UnityEngine;
using System.Collections;

public class Self_Destroy : MonoBehaviour 
{
    public float lifeTime = 10;
	
	void Start ()
    {
        StartCoroutine(Destroy(lifeTime));
    }
	

    private IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
