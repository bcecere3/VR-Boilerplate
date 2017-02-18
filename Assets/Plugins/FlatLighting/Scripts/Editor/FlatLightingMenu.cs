using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FlatLighting {
	public class FlatLightingMenu {

		[MenuItem("GameObject/Flat Lighting/Directional Light", false, 1)]
		public static void AddDirectionalLightToCurrentObject() {
			GameObject directionalLight = new GameObject("Directional Light");
			directionalLight.AddComponent<DirectionalLight>();
		}

		[MenuItem("GameObject/Flat Lighting/Spot Light", false, 2)]
		public static void AddSpotLightToCurrentObject() {
			GameObject spotLight = new GameObject("Spot Light");
			spotLight.AddComponent<SpotLight>();
		}

		[MenuItem("GameObject/Flat Lighting/Point Light", false, 3)]
		public static void AddPointLightToCurrentObject() {
			GameObject pointLight = new GameObject("Point Light");
			pointLight.AddComponent<PointLight>();
		}

		[MenuItem("GameObject/Flat Lighting/Shadow Projector", false, 4)]
		public static void AddShadowProjectorToCurrentObject() {
			GameObject shadowProjector = new GameObject("Shadow Projector");
			shadowProjector.AddComponent<ShadowProjector>();
		}
	}
}
