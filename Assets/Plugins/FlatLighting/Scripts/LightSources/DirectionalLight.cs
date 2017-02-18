using System;
using UnityEngine;

namespace FlatLighting {
	[AddComponentMenu("Flat Lighting/Directional Light", 1)]
	[ExecuteInEditMode]
	public class DirectionalLight : LightSource<DirectionalLight> {

		private const string directionalLightCountProperty = "_DirectionalLight_Length";
		private const string directionalLightColorProperty = "_DirectionalLightColor";
		private const string directionalLightForwardProperty = "_DirectionalLightForward";

		public bool isRealTime;

		public Color LightColor = Color.white;

		void OnEnable() {
			DirectionalLight.MAX_LIGHTS = 5;
			base.InitLightSource(directionalLightCountProperty);
		}

		void OnDisable() {
			base.ReleaseLightSource(directionalLightCountProperty);
		}

		void Update () {
			#if UNITY_EDITOR
			if (!Application.isPlaying || isRealTime) {
			#else
			if (isRealTime) {
			#endif
				SetLighting();
			}
		}

		#if UNITY_5_4_OR_NEWER
			public static Vector4[] forward = new Vector4[MAX_LIGHTS];
			public static Vector4[] color = new Vector4[MAX_LIGHTS];

			void LateUpdate() {
				if (Id != 0)
					return;

				Shader.SetGlobalVectorArray(directionalLightForwardProperty, forward);
				Shader.SetGlobalVectorArray(directionalLightColorProperty, color);
			}
		#endif

		private void SetLighting() {
			#if UNITY_5_4_OR_NEWER
				forward [Id] = transform.forward;
				color [Id] = LightColor;
			#else
				Shader.SetGlobalVector(directionalLightForwardProperty + Id.ToString(), transform.forward);
				Shader.SetGlobalVector(directionalLightColorProperty + Id.ToString(), LightColor);
			#endif
		}

		void OnDrawGizmosSelected() {
			DrawSelectedGizmo();

			Gizmos.color = Color.yellow;
			float lineLength = 5.0f;
			Vector3 lightDirection = -transform.forward * lineLength;
			Gizmos.DrawRay(transform.position, lightDirection);
		}
	}
}
