
using UnityEditor;

[CustomEditor(typeof(UIAnimationController))]

public class UIAnimationControllerEditor : Editor
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        var s = (UIAnimationController)target;

        // Animasyonlarýn isimlerini güncelle
        if (s.animations != null)
        {
            for (int i = 0; i < s.animations.Length; i++)
            {
                var anim = s.animations[i];
                if (anim != null && anim.uiElement != null)
                {
                    anim.name = anim.uiElement.gameObject.name;
                }
            }
        }
    }
}
