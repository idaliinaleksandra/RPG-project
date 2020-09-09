using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneMnagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;


        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
                print("HALOO");
            }
            yield return null;
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                print("halooo");
                yield return null;
            }
            yield return null;
        }

    }
}
