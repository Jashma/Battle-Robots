using UnityEngine;
using System.Collections;

public class UI_ShowEffect : MonoBehaviour {

	void OnEnable () 
    {
        EffectMenu();
	}

    void EffectMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        transform.localPosition = Vector3.up * 300;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        StartCoroutine(Effect());
    }

    private IEnumerator Effect(float time = 0.5f)
    {
        yield return new WaitForSeconds(time * Time.deltaTime);

        if (transform.localScale.x < 1)
        {
            transform.localScale += Vector3.one * Time.deltaTime;

            StartCoroutine(Effect());

            if (transform.localPosition != Vector3.zero)
            {
                transform.localPosition -= Vector3.up * UI_Controller.speedMoveMenu;
            }
        }

        yield return null;
    }
}
