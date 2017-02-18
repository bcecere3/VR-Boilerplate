using System;
using UnityEngine;

namespace FlatLighting {
	[AddComponentMenu("Flat Lighting/Point Light", 2)]
	[ExecuteInEditMode]
	public class PointLight : LightSource<PointLight> {

		private const string pointLightCountProperty = "_PointLight_Length";
		private const string pointLight0WorldToModelProperty = "_PointLightMatrixC0";
		private const string pointLightColorProperty = "_PointLightColor";
		private const string pointLightDistancesProperty = "_PointLightDistances";
		private const string pointLightIntensitiesProperty = "_PointLightIntensities";
		private const string pointLightSmoothnessProperty = "_PointLightSmoothness";

		public float Range = 1.0f;
		public Color LightColor = Color.white;

		[Tooltip("Every component is a circle of light, starting with X to Z. Example (0.5, 0.7, 1, 0).")]
		[VectorAsSliders("Lighting Distances", 3, 0.0f, 1.0f)] 
		public Vector4 LightDistances = new Vector4(0.5f, 0.7f, 1.0f, 0.0f);

		[Tooltip("Color intensitie of ecery fallof. Example (1, 0.5, 0.25, 0).")]
		[VectorAsSliders("Color Intensities", 3, -1.0f, 1.0f)] 
		public Vector4 LightIntensities = new Vector4(1.0f, 0.5f, 0.25f, 0.0f);

		public bool Smooth;
		public bool isRealTime;

		void OnEnable() {
			base.InitLightSource(pointLightCountProperty);
		}

		void OnDisable() {
			base.ReleaseLightSource(pointLightCountProperty);
		}

		private float GetSmoothness() {
			return Smooth ? 1.0f : 0.0f;
		}

		void Update() {
			#if UNITY_EDITOR
			if (!Application.isPlaying || isRealTime) {
			#else
			if (isRealTime) {
			#endif
				SetLighting();
			}
		}

		#if UNITY_5_4_OR_NEWER
			public static Matrix4x4[] worldToModel = new Matrix4x4[MAX_LIGHTS];
			public static Vector4[] distances = new Vector4[MAX_LIGHTS];
			public static Vector4[] intensities = new Vector4[MAX_LIGHTS];
			public static float[] smoothness = new float[MAX_LIGHTS];
			public static Vector4[] color = new Vector4[MAX_LIGHTS];

			void LateUpdate() {
				if (Id != 0)
					return;

				Shader.SetGlobalMatrixArray(pointLight0WorldToModelProperty, worldToModel);
				Shader.SetGlobalVectorArray(pointLightDistancesProperty, distances);
				Shader.SetGlobalVectorArray(pointLightColorProperty, color);
				Shader.SetGlobalVectorArray(pointLightIntensitiesProperty, intensities);
				Shader.SetGlobalFloatArray(pointLightSmoothnessProperty, smoothness);
			}
		#endif

		private void SetLighting() {
			#if UNITY_5_4_OR_NEWER
				worldToModel [Id] = transform.worldToLocalMatrix;
				distances [Id] = LightDistances * Range;
				color [Id] = LightColor;
				intensities [Id] = LightIntensities;
				smoothness [Id] = GetSmoothness ();
			#else
				Shader.SetGlobalMatrix(pointLight0WorldToModelProperty + Id.ToString(), transform.worldToLocalMatrix);
				Shader.SetGlobalVector(pointLightDistancesProperty + Id.ToString(), LightDistances * Range);
				Shader.SetGlobalColor(pointLightColorProperty + Id.ToString(), LightColor);
				Shader.SetGlobalVector(pointLightIntensitiesProperty + Id.ToString(), LightIntensities);
				Shader.SetGlobalFloat(pointLightSmoothnessProperty + Id.ToString(), GetSmoothness());
			#endif
		}

		void OnDrawGizmosSelected() {
			DrawSelectedGizmo();
			DrawPointLightSphere();
		}

		private void DrawPointLightSphere() {
			Color colorWidget = Color.yellow;
			colorWidget.a = 0.5f;
			Gizmos.color = colorWidget;

			Gizmos.DrawWireSphere(transform.position, Range);
		}
	}
}
