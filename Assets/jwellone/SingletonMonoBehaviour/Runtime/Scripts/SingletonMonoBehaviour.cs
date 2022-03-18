using UnityEngine;

#nullable enable

namespace jwellone
{
	public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		private static T? _instance;

		public static T? instance
		{
			get
			{
				if (!isInstance)
				{
					new GameObject($"{typeof(T).Name}").AddComponent<T>();
				}

				return _instance;
			}
		}

		public static bool isInstance
		{
			get;
			private set;
		}

		public virtual bool isDontDestroyOnLoad => true;

		private void Awake()
		{
			if (isInstance)
			{
				Destroy(gameObject);
				return;
			}

			_instance = (T)this;
			isInstance = true;

			if (isDontDestroyOnLoad)
			{
				DontDestroyOnLoad(gameObject);
			}

			OnAwakened();
		}

		private void OnDestroy()
		{
			OnDestroyed();
			if (this == _instance)
			{
				isInstance = false;
				_instance = null;
			}
		}

		protected abstract void OnAwakened();
		protected abstract void OnDestroyed();
	}
}
