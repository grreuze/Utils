using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkingText : MonoBehaviour {

    [SerializeField]
    float blinkTime = 1, waitTime = 0.5f;

	void Start() {
        StartCoroutine(Idle());
	}
	
    IEnumerator Idle() {
        Text text = GetComponent<Text>();
        bool isOn = false;
        float elapsed = 0;
        float alpha;
        while(!Input.GetKeyUp(KeyCode.Space)){
            elapsed += Time.deltaTime;
            if (elapsed >= blinkTime) {
                yield return new WaitForSeconds(waitTime);
                elapsed = 0;
                isOn ^= true;
            }
            
            if (isOn) 
                alpha = Mathf.Lerp(1, 0, elapsed / blinkTime);
            else 
                alpha = Mathf.Lerp(0, 1, elapsed / blinkTime);

            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
        DestroyImmediate(gameObject);
    }

}
