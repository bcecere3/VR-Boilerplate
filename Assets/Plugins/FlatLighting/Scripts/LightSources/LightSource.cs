using UnityEngine;
using System.Collections;

namespace FlatLighting {
	public abstract class LightSource<T> : MonoBehaviour where T : LightSource<T> {

		protected static int MAX_LIGHTS = 25; //Don't modify this as it is a limit also included in the shader, and if it's change, the tool will be unstable.
		protected static int lightCount = 0;
		protected static object my_lock = new object();
		protected static T[] lights = new T[MAX_LIGHTS];
		protected int Id;

		protected void InitLightSource(string lightCountProperty) {
			lock(my_lock) {
				if (lightCount >= MAX_LIGHTS) {
					Debug.LogError("Could not initialize a new light source because a limit has been reached");
					return;
				}
				Id = lightCount;
				lights[Id] = (T)this;
				lightCount++;
				Shader.SetGlobalInt(lightCountProperty, lightCount);
			}
		}

		protected void ReleaseLightSource(string lightCountProperty) {
			lock(my_lock) {
				lights[lightCount-1].Id = Id;
				lightCount--;
				Shader.SetGlobalInt(lightCountProperty, lightCount);
			}
		}

		protected void DrawSelectedGizmo() {
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(transform.position, 0.25f);
		}
	}
}
