using UnityEngine;
using System.Collections;

namespace FlatLighting {
	[AddComponentMenu("Flat Lighting/Spot Light", 3)]
	[ExecuteInEditMode]
	public class SpotLight : LightSource<SpotLight> {

		private const string spotLightCountProperty = "_SpotLight_Length";
		private const string spotLightWorldToModelProperty = "_SpotLightMatrixC0";
		private const string spotLightForwardProperty = "_SpotLightObjectSpaceForward";
		private const string spotLightColorProperty = "_SpotLightColor";
		private const string spotLightBaseRadiusProperty = "_SpotLightBaseRadius";
		private const string spotLightHeightProperty = "_SpotLightHeight";
		private const string spotLightDistancesProperty = "_SpotLightDistances";
		private const string spotLightIntensitiesProperty = "_SpotLightIntensities";
		private const string spotLightSmoothnessProperty = "_SpotLightSmoothness";

		public float BaseRadius = 2.0f;
		public float Height = 4.0f;
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
			base.InitLightSource(spotLightCountProperty);
		}

		void OnDisable() {
			base.ReleaseLightSource(spotLightCountProperty);
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
			public static Vector4[] forward = new Vector4[MAX_LIGHTS];
			public static float[] baseRadius = new float[MAX_LIGHTS];
			public static float[] height = new float[MAX_LIGHTS];
			public static Vector4[] distances = new Vector4[MAX_LIGHTS];
			public static Vector4[] intensities = new Vector4[MAX_LIGHTS];
			public static float[] smoothness = new float[MAX_LIGHTS];
			public static Vector4[] color = new Vector4[MAX_LIGHTS];
			
			void LateUpdate() {
				if (Id != 0)
					return;

				Shader.SetGlobalMatrixArray(spotLightWorldToModelProperty, worldToModel);
				Shader.SetGlobalVectorArray(spotLightForwardProperty, forward);
				Shader.SetGlobalFloatArray(spotLightBaseRadiusProperty, baseRadius);
				Shader.SetGlobalFloatArray(spotLightHeightProperty, height);
				Shader.SetGlobalVectorArray(spotLightDistancesProperty, distances);
				Shader.SetGlobalVectorArray(spotLightColorProperty, color);
				Shader.SetGlobalVectorArray(spotLightIntensitiesProperty, intensities);
				Shader.SetGlobalFloatArray(spotLightSmoothnessProperty, smoothness);
			}
		#endif

		private void SetLighting() {
			#if UNITY_5_4_OR_NEWER
				worldToModel [Id] = transform.worldToLocalMatrix;
				forward [Id] = Vector3.forward;
				baseRadius [Id] = BaseRadius;
				height [Id] = Height;
				distances [Id] = LightDistances * BaseRadius;
				color [Id] = LightColor;
				intensities [Id] = LightIntensities;
				smoothness [Id] = GetSmoothness ();
			#else
				Shader.SetGlobalMatrix(spotLightWorldToModelProperty + Id.ToString(), transform.worldToLocalMatrix);
				Shader.SetGlobalVector(spotLightForwardProperty + Id.ToString(), Vector3.forward);
				Shader.SetGlobalFloat(spotLightBaseRadiusProperty + Id.ToString(), BaseRadius);
				Shader.SetGlobalFloat(spotLightHeightProperty + Id.ToString(), Height);
				Shader.SetGlobalVector(spotLightDistancesProperty + Id.ToString(), LightDistances * BaseRadius);
				Shader.SetGlobalColor(spotLightColorProperty + Id.ToString(), LightColor);
				Shader.SetGlobalVector(spotLightIntensitiesProperty + Id.ToString(), LightIntensities);
				Shader.SetGlobalFloat(spotLightSmoothnessProperty + Id.ToString(), GetSmoothness());
			#endif
		}

		private void DrawSpotLightConePairLines(Vector3 side) {
//			Vector3 lightDirection = transform.TransformDirection( (Vector3.forward * Height) + side );
//			Gizmos.DrawRay(transform.position, lightDirection);
			Vector3 lightDirection = (transform.forward * Height) + transform.rotation * side;
			Gizmos.DrawRay(transform.position, lightDirection);

			lightDirection = (Quaternion.AngleAxis(45.0f, transform.forward)) * lightDirection;
			Gizmos.DrawRay(transform.position, lightDirection);
		}

		private void DrawSpotLightCone() {
			Color colorWidget = Color.yellow;
			colorWidget.a = 0.5f;
			Gizmos.color = colorWidget;

			Vector3 spotDirectionX = new Vector3(BaseRadius, 0, 0);
			Vector3 spotDirectionY = new Vector3(0, BaseRadius, 0);

			DrawSpotLightConePairLines(spotDirectionX);
			DrawSpotLightConePairLines(-spotDirectionX);
			DrawSpotLightConePairLines(spotDirectionY);
			DrawSpotLightConePairLines(-spotDirectionY);
		}

		void OnDrawGizmosSelected() {
			DrawSelectedGizmo();
			DrawSpotLightCone();
		}
	}
}
