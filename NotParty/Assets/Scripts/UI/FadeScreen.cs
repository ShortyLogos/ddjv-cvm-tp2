using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

// FadeScreen script requiert que le GameObject possède une composante Image.
[RequireComponent(typeof(Image))]
public class FadeScreen: MonoBehaviour
{
    [SerializeField] [Tooltip("Do you want the scene to have a fade in effect?")]
    private bool fadeIn;
    
    [SerializeField] [Min(0)] [Tooltip("The amount of seconds between each incrementation of the alpha value of the fade screen.")]
    private float vitesseFadeIn;

    [SerializeField] [Range(0.001f, 1.0f)] [Tooltip("Value by which the alpha is incremented at each update.")]
    private float incrementation = 0.01f;


    [SerializeField] [Tooltip("Do you want the scene to have a fade out effect?")]
    private bool fadeOut;
    
    [SerializeField] [Min(0)] [Tooltip("The amount of seconds between each decrementation of the alpha value of the fade screen.")]
    private float vitesseFadeOut;
    
    [SerializeField] [Range(0.001f, 1.0f)] [Tooltip("Value by which the alpha is decremented at each update.")]
    private float decrementation = 0.01f;


    [SerializeField] [Tooltip("Do you want the scene to fade out automatically after a given amount of seconds?")]
    private bool autoFadeOut;
    
    [SerializeField] [Min(0)] [Tooltip("Amount of seconds to wait before starting the fade out.")]
    private float delai = 0.0f;

    private Image rendu;
    private Canvas canvas;

    private void OnValidate()
    {
        rendu = gameObject.GetComponent<Image>();
        rendu.color = new Color32(0, 0, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "FadeScreen";
        canvas = gameObject.GetComponentInParent<Canvas>();


        if (fadeIn)
        {
            rendu.color = new Color32(0, 0, 0, 255);
            FadeIn();
        }

        if (autoFadeOut)
        {
            FadeOut(false);
        }
        else
        {
            delai = 0.0f;
        }
    } 

    // Fonction publique pour forcer un fade In. Retourne le temps que l'animation prendra.
    public float FadeIn()
    {
        StartCoroutine(CFadeIn());
        return GetFadeInDuration();
    }

    //Fonction publique pour forcer un fade out. Si skipDelai est à true, le fadeOut est immédiat.  Retourne le temps que l'animation prendra.
    public float FadeOut(bool skipDelai = true)
    {
        StartCoroutine(CFadeOut(skipDelai));
        return GetFadeOutDuration();
    }

    public float GetTotalDuration()
    {
        float duration = 0.0f;
        if (fadeIn)
        {
            duration += 1 / decrementation * vitesseFadeIn;
        }
        if (fadeOut)
        {
            duration += 1 / incrementation * vitesseFadeOut;
        }
        if (autoFadeOut)
        {
            duration += delai;
        }
        return duration;
    }

    public float GetFadeInDuration()
    {
        return 1+(1 / decrementation * vitesseFadeIn);
    }

    public float GetFadeOutDuration()
    {
        return 1 / incrementation * vitesseFadeOut;
    }

    private IEnumerator CFadeIn()
    {
        yield return StartCoroutine(Utility.WaitForRealSeconds(1));

        Color couleur = rendu.color;
        float alpha = couleur.a;

        while (alpha > 0.0f)
        {
            alpha -= decrementation;
            couleur.a = Mathf.Max(alpha, 0.0f);
            rendu.color = couleur;

            yield return StartCoroutine(Utility.WaitForRealSeconds(vitesseFadeIn));
        }

        canvas.enabled = false;
    }

    private IEnumerator CFadeOut(bool skipDelai)
    {
        if (!skipDelai)
        {
            yield return StartCoroutine(Utility.WaitForRealSeconds(delai));
        }

        canvas.enabled = true;

        Color couleur = rendu.color;
        float alpha = couleur.a;

        while (alpha < 1.0f)
        {
            alpha += incrementation;
            couleur.a = Mathf.Min(alpha,1.0f);
            rendu.color = couleur;

            yield return StartCoroutine(Utility.WaitForRealSeconds(vitesseFadeOut));
        }
    }

//#if UNITY_EDITOR
//    // https://www.youtube.com/watch?v=RImM7XYdeAc
//    // https://docs.unity3d.com/Manual/editor-CustomEditors.html
//    [CustomEditor(typeof(FadeScreen))]
//    [CanEditMultipleObjects]
//    public class FadeScreenEditor : Editor
//    {
//        SerializedProperty fadeInProperty;
//        SerializedProperty fadeOutProperty;
//        SerializedProperty autoFadeOutProperty;
//        SerializedProperty vitesseFadeInProperty;
//        SerializedProperty vitesseFadeOutProperty;
//        SerializedProperty delaiProperty;
//        SerializedProperty incrementationProperty;
//        SerializedProperty decrementationProperty;

//        private void OnEnable()
//        {
//            fadeInProperty = serializedObject.FindProperty("fadeIn");
//            fadeOutProperty = serializedObject.FindProperty("fadeOut");
//            autoFadeOutProperty = serializedObject.FindProperty("autoFadeOut");
//            vitesseFadeInProperty = serializedObject.FindProperty("vitesseFadeIn");
//            vitesseFadeOutProperty = serializedObject.FindProperty("vitesseFadeOut");
//            delaiProperty = serializedObject.FindProperty("delai");
//            incrementationProperty = serializedObject.FindProperty("incrementation");
//            decrementationProperty = serializedObject.FindProperty("decrementation");
//        }

//        public override void OnInspectorGUI()
//        {
//            // https://answers.unity.com/questions/1223009/how-to-show-the-standard-script-line-with-a-custom.html
//            using (new EditorGUI.DisabledScope(true))
//                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

//            serializedObject.Update();

//            EditorGUILayout.PropertyField(fadeInProperty);
//            if (fadeInProperty.boolValue)
//            {
//                EditorGUI.indentLevel++;
//                EditorGUILayout.PropertyField(decrementationProperty);
//                EditorGUILayout.PropertyField(vitesseFadeInProperty);
//                EditorGUI.indentLevel--;
//            }

//            EditorGUILayout.PropertyField(fadeOutProperty);
//            if (fadeOutProperty.boolValue)
//            {
//                EditorGUI.indentLevel++;
//                EditorGUILayout.PropertyField(incrementationProperty);
//                EditorGUILayout.PropertyField(vitesseFadeOutProperty);
//                EditorGUI.indentLevel--;
//                EditorGUILayout.PropertyField(autoFadeOutProperty);
//                if (autoFadeOutProperty.boolValue)
//                {
//                    EditorGUI.indentLevel++;
//                    EditorGUILayout.PropertyField(delaiProperty);
//                    EditorGUI.indentLevel--;
//                }
//            }



//            serializedObject.ApplyModifiedProperties();
//        }
//    }

//#endif

}
