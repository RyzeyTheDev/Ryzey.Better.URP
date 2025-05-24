// =========================================
// Ryzey Better URP - Light Probe & URP Booster
// =========================================

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Ryzey.BetterURP
{
    [ExecuteInEditMode]
    public class RyzeyURPBooster : MonoBehaviour
    {
        public Vector3 probeBoundsCenter = Vector3.zero;
        public Vector3 probeBoundsSize = new Vector3(10, 5, 10);
        public float probeSpacing = 1.5f;
        public LayerMask geometryMask = ~0;

        public Vector3 reflectionAreaSize = new Vector3(20, 10, 20);
        public Vector3 reflectionSpacing = new Vector3(10, 10, 10);

        public void CreateBeautifulURPData()
        {
            GenerateLightProbes();
            GenerateReflectionProbes();
            BakeLighting();
        }

        public void GenerateLightProbes()
        {
            GameObject probeGroupObj = new GameObject("Ryzey_LightProbeGroup");
            LightProbeGroup probeGroup = probeGroupObj.AddComponent<LightProbeGroup>();
            List<Vector3> positions = new List<Vector3>();

            Vector3 min = probeBoundsCenter - probeBoundsSize / 2f;
            Vector3 max = probeBoundsCenter + probeBoundsSize / 2f;

            for (float x = min.x; x <= max.x; x += probeSpacing)
            {
                for (float y = min.y; y <= max.y; y += probeSpacing)
                {
                    for (float z = min.z; z <= max.z; z += probeSpacing)
                    {
                        Vector3 pos = new Vector3(x, y, z);
                        if (!Physics.CheckSphere(pos, probeSpacing * 0.4f, geometryMask))
                        {
                            positions.Add(pos);
                        }
                    }
                }
            }

            probeGroup.probePositions = positions.ToArray();
            Debug.Log($"RyzeyBetterURP: Placed {positions.Count} light probes.");
        }

        public void GenerateReflectionProbes()
        {
            ClearExistingReflectionProbes();

            Vector3 min = transform.position - reflectionAreaSize / 2f;
            Vector3 max = transform.position + reflectionAreaSize / 2f;

            for (float x = min.x; x <= max.x; x += reflectionSpacing.x)
            {
                for (float y = min.y; y <= max.y; y += reflectionSpacing.y)
                {
                    for (float z = min.z; z <= max.z; z += reflectionSpacing.z)
                    {
                        GameObject probeObj = new GameObject("Ryzey_ReflectionProbe");
                        probeObj.transform.position = new Vector3(x, y, z);
                        var probe = probeObj.AddComponent<ReflectionProbe>();
                        probe.size = reflectionSpacing;
                        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
                    }
                }
            }

            Debug.Log("RyzeyBetterURP: Reflection probes generated.");
        }

        public void ClearExistingReflectionProbes()
        {
            var probes = GameObject.FindObjectsOfType<ReflectionProbe>();
            foreach (var probe in probes)
            {
                if (probe.name.StartsWith("Ryzey_ReflectionProbe"))
                {
                    DestroyImmediate(probe.gameObject);
                }
            }
        }

        public void BakeLighting()
        {
#if UNITY_EDITOR
            Lightmapping.BakeAsync();
            Debug.Log("RyzeyBetterURP: Lighting bake started.");
#endif
        }
    }
}

#if UNITY_EDITOR
namespace Ryzey.BetterURP.Editor
{
    [CustomEditor(typeof(RyzeyURPBooster))]
    public class RyzeyURPBoosterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Create Beautiful URP Data"))
            {
                RyzeyURPBooster booster = (RyzeyURPBooster)target;
                booster.CreateBeautifulURPData();
            }
        }
    }
}
#endif
