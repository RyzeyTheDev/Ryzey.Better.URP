using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ryzey.BetterURP
{
    public class RyzeyURPProfileAI : MonoBehaviour
    {
        [Header("Describe the look (e.g., 'cinematic', 'happy', 'sunset', 'horror', 'bright warm')")]
        public string stylePrompt = "cinematic";

        [Header("Created Profile Output")]
        public VolumeProfile generatedProfile;

#if UNITY_EDITOR
        [ContextMenu("Create URP Profile From Prompt")]
        public void GenerateProfile()
        {
            if (string.IsNullOrWhiteSpace(stylePrompt))
            {
                Debug.LogWarning("Please provide a style description before generating.");
                return;
            }

            // Create new profile asset
            generatedProfile = ScriptableObject.CreateInstance<VolumeProfile>();
            string sanitizedPrompt = stylePrompt.Replace(" ", "_").ToLowerInvariant();
            string path = $"Assets/Generated_{sanitizedPrompt}_Profile.asset";
            AssetDatabase.CreateAsset(generatedProfile, path);

            ApplyStyleFromPrompt(stylePrompt.ToLowerInvariant());

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Generated URP Profile for style '{stylePrompt}' at: {path}");
        }

        private void ApplyStyleFromPrompt(string prompt)
        {
            // Example: parse keywords and apply corresponding settings
            if (prompt.Contains("cinematic"))
            {
                AddOrUpdate<Bloom>(b =>
                {
                    b.active = true;
                    b.intensity.value = 1.2f;
                    b.threshold.value = 1f;
                });
                AddOrUpdate<ColorAdjustments>(c =>
                {
                    c.active = true;
                    c.contrast.value = 40f;
                    c.postExposure.value = 0.4f;
                    c.saturation.value = -10f;
                });
                AddOrUpdate<Vignette>(v =>
                {
                    v.active = true;
                    v.intensity.value = 0.3f;
                });
            }

            if (prompt.Contains("happy") || prompt.Contains("bright"))
            {
                AddOrUpdate<ColorAdjustments>(c =>
                {
                    c.active = true;
                    c.saturation.value = 30f;
                    c.postExposure.value = 0.5f;
                    c.contrast.value = 15f;
                });
                AddOrUpdate<Bloom>(b =>
                {
                    b.active = true;
                    b.intensity.value = 0.8f;
                    b.scatter.value = 1f;
                });
            }

            if (prompt.Contains("horror") || prompt.Contains("dark"))
            {
                AddOrUpdate<ColorAdjustments>(c =>
                {
                    c.active = true;
                    c.saturation.value = -50f;
                    c.postExposure.value = -0.3f;
                    c.contrast.value = 50f;
                });
                AddOrUpdate<Vignette>(v =>
                {
                    v.active = true;
                    v.intensity.value = 0.4f;
                    v.smoothness.value = 1f;
                });
                AddOrUpdate<FilmGrain>(f =>
                {
                    f.active = true;
                    f.intensity.value = 0.6f;
                });
            }

            if (prompt.Contains("warm"))
            {
                AddOrUpdate<ColorAdjustments>(c =>
                {
                    c.active = true;
                    c.colorFilter.value = new Color(1f, 0.85f, 0.7f); // warm orange tint
                });
            }

            if (prompt.Contains("cold") || prompt.Contains("cool"))
            {
                AddOrUpdate<ColorAdjustments>(c =>
                {
                    c.active = true;
                    c.colorFilter.value = new Color(0.7f, 0.85f, 1f); // cool blue tint
                });
            }
        }

        private void AddOrUpdate<T>(System.Action<T> configure) where T : VolumeComponent
        {
            if (!generatedProfile.TryGet(out T component))
            {
                component = generatedProfile.Add<T>(true);
            }

            configure?.Invoke(component);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RyzeyURPProfileAI))]
    public class RyzeyURPProfileAIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Create URP Profile From Prompt"))
            {
                ((RyzeyURPProfileAI)target).GenerateProfile();
            }
        }
    }
#endif
}
