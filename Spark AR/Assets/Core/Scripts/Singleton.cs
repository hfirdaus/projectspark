using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> { }
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static bool m_ShuttingDown = false;
	static object m_Lock = new object();
	static T m_Instance;

	public static T Instance
	{
		get
		{
			if (m_ShuttingDown)
				return null;

			lock (m_Lock)
			{
				if (m_Instance == null)
				{
					m_Instance = (T)FindObjectOfType(typeof(T));

					if (m_Instance == null)
					{
						GameObject singletonObject = new GameObject();
						m_Instance = singletonObject.AddComponent<T>();
						singletonObject.name = typeof(T).ToString() + " (Singleton)";

						DontDestroyOnLoad(singletonObject);
					}
				}

				return m_Instance;
			}
		}
	}

	void OnApplicationQuit()
	{
		m_ShuttingDown = true;
	}

	void OnDestroy()
	{
		m_ShuttingDown = true;
	}
}