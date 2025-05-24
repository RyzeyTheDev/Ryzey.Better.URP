using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ryzey.BetterURP
{
    // This script requires you have the Universal Render Pipeline package installed
    // and your project set to use URP in Graphics Settings.

    public class RyzeyURPProfileEditorFixed : MonoBehaviour
    {
        [Header("Assign a URP Volume Profile to enhance")]
        public VolumeProfile targetProfile;

#if UNITY_EDITOR
        [ContextMenu("Apply Beautiful Settings to Volume Profile")]
        public void ApplyBeautifulSettings()
        {
            if (targetProfile == null)
            {
                Debug.LogError("RyzeyBetterURP: No Volume Profile assigned.");
                return;
            }

            AddOrUpdate<Bloom>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 1.2f;
                settings.scatter.value = 0.8f;
                settings.threshold.value = 0.9f;
            });

            AddOrUpdate<ColorAdjustments>(settings =>
            {
                settings.active = true;
                settings.postExposure.value = 0.4f;
                settings.contrast.value = 25f;
                settings.saturation.value = 20f;
                settings.colorFilter.value = Color.white;
            });

            AddOrUpdate<Vignette>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 0.15f;
                settings.smoothness.value = 0.9f;
            });

            AddOrUpdate<DepthOfField>(settings =>
            {
                settings.active = true;
                settings.mode.value = DepthOfFieldMode.Gaussian;
                settings.gaussianStart.value = 4f;
                settings.gaussianEnd.value = 25f;
                settings.highQualitySampling.value = true;
            });

            AddOrUpdate<ChromaticAberration>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 0.02f;
            });

            AddOrUpdate<MotionBlur>(settings =>
            {
                settings.active = true;
                settings.intensity.value = 0.25f;
            });

            Debug.Log("RyzeyBetterURP: Beautiful settings applied to Volume Profile.");

            // Show popup dialog in Editor
            EditorUtility.DisplayDialog(
                "Thanks for Buying Ryzey.Better.URP Beta",
                "Some assets have bugs but will be fixed soon.\n\nAlso, this is my first ever URP package — sorry for any issues!",
                "OK"
            );
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
[CustomEditor(typeof(Ryzey.BetterURP.RyzeyURPProfileEditorFixed))]
public class RyzeyURPProfileEditorFixedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Apply Beautiful Settings"))
        {
            ((Ryzey.BetterURP.RyzeyURPProfileEditorFixed)target).ApplyBeautifulSettings();
        }
    }
}
#endif
