using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArrowBlink : MonoBehaviour
{
    [SerializeField]
    private float fadeTime; // 페이드 되는 시간
    private Image fadeImage; //페이드 효과에 사용되는 이미지

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        //Fade 효과를 In -> Out 무한 반복
        StartCoroutine("FadeInOut");
    }

    private void OnDisable()
    {
        StopCoroutine("FadeInOut");
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0)); //Fade In
            yield return StartCoroutine(Fade(0, 1)); //Fade Out
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            //UnityEngine.Color
            Color color = fadeImage.color;

            //Color.r,g,b,a 중 Color.a (알파 값)
            //Mathf.Lerp(start,end,보간값) -> 부드럽게 start에서 end까지 보간값만큼 변화함
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;

        }
    }

}