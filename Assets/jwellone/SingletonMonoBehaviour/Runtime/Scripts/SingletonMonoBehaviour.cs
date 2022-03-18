using UnityEngine;

#nullable enable

namespace jwellone
{
	public abstract class IntermediateSingletonMonoBehaviour : MonoBehaviour
	{
		protected virtual bool isDontDestroyOnLoad => true;

		internal abstract void Awake();
		internal abstract void OnDestroy();
	}

	public abstract class SingletonMonoBehaviour<T> : IntermediateSingletonMonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		private static T? _instance;

		public static T? instance
		{
			get
			{
				if (!isExists)
				{
					instance = FindObjectOfType<T>();
					Debug.Assert(isExists, $"{typeof(T).Name} The instance does not exist.");
				}

				return _instance;
			}

			private set
			{
				_instance = value;
				isExists = (_instance != null);
			}
		}

		public static bool isExists
		{
			get;
			private set;
		}

		internal sealed override void Awake()
		{
			if (!isExists)
			{
				instance = (T)this;
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
				return;
			}

			if (isDontDestroyOnLoad)
			{
				DontDestroyOnLoad(gameObject);
			}

			OnAwakened();
		}

		internal sealed override void OnDestroy()
		{
			OnDestroyed();
			if (this == _instance)
			{
				instance = null;
			}
		}

		protected virtual void OnAwakened()
		{
		}

		protected virtual void OnDestroyed()
		{
		}
	}
}
