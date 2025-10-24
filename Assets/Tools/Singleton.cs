using UnityEngine;

/// <summary>
/// A generic Singleton class for Unity MonoBehaviours.
/// Inherit from this class to create a singleton.
/// Example: public class GameManager : Singleton<GameManager> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	// Lock object for thread safety.
	private static readonly object _lock = new object();

	// Flag to indicate if the application is quitting.
	private static bool _applicationIsQuitting = false;

	/// <summary>
	/// Accessor for the singleton instance.
	/// </summary>
	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					// Search for existing instance.
					_instance = (T)FindObjectOfType(typeof(T));

					// Create new instance if one doesn't already exist.
					if (_instance == null)
					{
						// Create a new GameObject to attach the singleton to.
						GameObject singletonObject = new GameObject();
						_instance = singletonObject.AddComponent<T>();
						singletonObject.name = typeof(T).ToString() + " (Singleton)";

						// Make instance persistent across scenes.
						DontDestroyOnLoad(singletonObject);

						Debug.Log("[Singleton] An instance of " + typeof(T) +
							" is needed in the scene, so '" + singletonObject +
							"' was created with DontDestroyOnLoad.");
					}
					else
					{
						Debug.Log("[Singleton] Using instance already created: " +
							_instance.gameObject.name);
					}
				}

				return _instance;
			}
		}
	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In practice, this makes sure the singleton instance is not recreated on quitting.
	/// </summary>
	protected virtual void OnDestroy()
	{
		_applicationIsQuitting = true;
	}
}
