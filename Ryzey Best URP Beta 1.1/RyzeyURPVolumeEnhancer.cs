using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace Ryzey.BetterURP
{
    public class RyzeyURPVolumeEnhancer : MonoBehaviour
    {
        [Header("Assign a URP Volume Profile to enhance")]
        public VolumeProfile targetProfile;

#if UNITY_EDITOR
        public void EnhanceProfile()
        {
            if (targetProfile == null)
            {
                Debug.LogError("RyzeyBetterURP: No Volume Profile assigned.");
                return;
            }

            AddOrUpdate<DepthOfField>(settings =>
            {
                settings.active = true;
                settings.mode.value = DepthOfFieldMode.Gaussian;
                settings.gaussianStart.value = 5f;
                settings.gaussianEnd.value = 20f;
                settings.highQualitySampling.value = true;
            });

            AddOrUpdate<Bloom>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 1f;
                settings.scatter.value = 0.7f;
                settings.threshold.value = 1f;
            });

            AddOrUpdate<ColorAdjustments>(settings =>
            {
                settings.active = true;
                settings.postExposure.value = 0.25f;
                settings.contrast.value = 20f;
                settings.saturation.value = 10f;
            });

            AddOrUpdate<Vignette>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 0.2f;
                settings.smoothness.value = 0.9f;
            });

            AddOrUpdate<ChromaticAberration>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 0.05f;
            });

            AddOrUpdate<MotionBlur>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 0.3f;
            });

            AddOrUpdate<LensDistortion>(settings =>
            {
                settings.active = true;
                settings.intensity.value = -0.1f;
                settings.scale.value = 1f;
            });

            Debug.Log("RyzeyBetterURP: URP profile enhanced beautifully.");
        }

        private void AddOrUpdate<T>(System.Action<T> configure) where T : VolumeComponent
        {
            if (!targetProfile.TryGet(out T component))
            {
                component = targetProfile.Add<T>(true);
            }

            configure?.Invoke(component);
        }
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Ryzey.BetterURP.RyzeyURPVolumeEnhancer))]
public class RyzeyURPVolumeEnhancerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Upgrade URP Profile"))
        {
            var enhancer = (Ryzey.BetterURP.RyzeyURPVolumeEnhancer)target;

            if (EditorUtility.DisplayDialog(
                "Ryzey Better URP",
                "This will apply beautiful realism settings to your URP profile.\n\nDo you want to continue and reload the scene?",
                "Yes, enhance and reload",
                "Cancel"))
            {
                enhancer.EnhanceProfile();
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(SceneManager.GetActiveScene().path);
            }
        }
    }
}
#endif
