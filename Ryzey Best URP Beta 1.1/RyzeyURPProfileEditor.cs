using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ryzey.BetterURP
{
    public class RyzeyURPProfileEditor : MonoBehaviour
    {
        [Header("Assign the URP Volume Profile to edit")]
        public VolumeProfile profile;

#if UNITY_EDITOR
        [ContextMenu("Apply Beautiful Settings")]
        public void ApplyBeautifulSettings()
        {
            if (profile == null)
            {
                Debug.LogError("RyzeyURPProfileEditor: No URP Volume Profile assigned.");
                return;
            }

            AddOrUpdate<Bloom>(bloom =>
            {
                bloom.active = true;
                bloom.intensity.overrideState = true;
                bloom.intensity.value = 1.2f;
                bloom.threshold.overrideState = true;
                bloom.threshold.value = 1f;
                bloom.scatter.overrideState = true;
                bloom.scatter.value = 0.8f;
            });

            AddOrUpdate<ColorAdjustments>(color =>
            {
                color.active = true;
                color.postExposure.overrideState = true;
                color.postExposure.value = 0.4f;
                color.saturation.overrideState = true;
                color.saturation.value = 20f;
                color.contrast.overrideState = true;
                color.contrast.value = 30f;
            });

            AddOrUpdate<Vignette>(vignette =>
            {
                vignette.active = true;
                vignette.intensity.overrideState = true;
                vignette.intensity.value = 0.15f;
                vignette.smoothness.overrideState = true;
                vignette.smoothness.value = 0.8f;
            });

            AddOrUpdate<DepthOfField>(dof =>
            {
                dof.active = true;
                dof.mode.overrideState = true;
                dof.mode.value = DepthOfFieldMode.Gaussian;
                dof.gaussianStart.value = 5f;
                dof.gaussianEnd.value = 25f;
                dof.highQualitySampling.value = true;
            });

            AddOrUpdate<ChromaticAberration>(ca =>
            {
                ca.active = true;
                ca.intensity.overrideState = true;
                ca.intensity.value = 0.04f;
            });

            Debug.Log("RyzeyURPProfileEditor: Beautiful settings applied to URP Volume Profile.");
        }

        private void AddOrUpdate<T>(System.Action<T> configure) where T : VolumeComponent
        {
            if (!profile.TryGet(out T component))
            {
                component = profile.Add<T>();
            }
            configure?.Invoke(component);
        }
#endif
    }
}
