using UnityEngine;
using DG.Tweening;
public class UIAnimationController : MonoBehaviour
{
    [System.Serializable]
    public class UIAnimation
    {
        [HideInInspector]
        public string name;
        public RectTransform uiElement;
        public AnimationType animationType;
        public Ease bounceType = Ease.OutBounce; // Bounce type seçeneði eklendi
        public float duration = 1f;
        public float delay = 0f;
        public AudioClip animationSound;
        [HideInInspector]
        public Vector3 targetPosition;
        [HideInInspector]
        public Vector3 initialScale;
        public bool isEnabledStart = true;
    }
    public UIAnimation[] animations;
    private AudioSource audioSource;
    public enum AnimationType
    {
        SlideFromBottom,
        SlideFromTop,
        SlideFromRight,
        SlideFromLeft,
        ScaleUp,
        ScaleDown,
        FadeIn,
        FadeOut
    }
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        foreach (var anim in animations)
        {
            anim.targetPosition = anim.uiElement.anchoredPosition;
            anim.initialScale = anim.uiElement.localScale; // Baþlangýç scale deðerini kaydet
        }
        foreach (var anim in animations)
        {
            if (!anim.isEnabledStart)
            {
                anim.uiElement.gameObject.SetActive(false);
            }
        }
    }
    void Start()
    {
        foreach (var anim in animations)
        {
            StartAnimation(anim);
        }
    }
    void StartAnimation(UIAnimation anim)
    {
        anim.uiElement.DOComplete();
        DOVirtual.DelayedCall(anim.delay, () => anim.uiElement.gameObject.SetActive(true));
        float offset = 50f;
        switch (anim.animationType)
        {
            case AnimationType.SlideFromBottom:
                anim.uiElement.anchoredPosition = new Vector3(anim.targetPosition.x, -Screen.height - anim.uiElement.rect.height - offset, anim.targetPosition.z);
                anim.uiElement.DOAnchorPos(anim.targetPosition, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.SlideFromTop:
                anim.uiElement.anchoredPosition = new Vector3(anim.targetPosition.x, Screen.height + anim.uiElement.rect.height + offset, anim.targetPosition.z);
                anim.uiElement.DOAnchorPos(anim.targetPosition, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.SlideFromRight:
                anim.uiElement.anchoredPosition = new Vector3(Screen.width / 2 + anim.uiElement.rect.width / 2 + offset, anim.targetPosition.y, anim.targetPosition.z);
                anim.uiElement.DOAnchorPos(anim.targetPosition, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.SlideFromLeft:
                anim.uiElement.anchoredPosition = new Vector3(-Screen.width / 2 - anim.uiElement.rect.width / 2 - offset, anim.targetPosition.y, anim.targetPosition.z);
                anim.uiElement.DOAnchorPos(anim.targetPosition, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.ScaleUp:
                anim.uiElement.localScale = Vector3.zero;
                anim.uiElement.DOScale(anim.initialScale, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.ScaleDown:
                anim.uiElement.localScale = anim.initialScale;
                anim.uiElement.DOScale(Vector3.zero, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.FadeIn:
                anim.uiElement.GetComponent<CanvasGroup>().alpha = 0;
                anim.uiElement.GetComponent<CanvasGroup>().DOFade(1, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
            case AnimationType.FadeOut:
                anim.uiElement.GetComponent<CanvasGroup>().alpha = 1;
                anim.uiElement.GetComponent<CanvasGroup>().DOFade(0, anim.duration).SetDelay(anim.delay).SetEase(anim.bounceType);
                break;
        }
        if (anim.animationSound != null)
        {
            DOVirtual.DelayedCall(anim.delay, () => audioSource.PlayOneShot(anim.animationSound));
        }
    }
}