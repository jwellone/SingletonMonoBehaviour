using UnityEngine;

#nullable enable

namespace jwellone
{
	public abstract class IntermediateSingletonMonoBehaviour : MonoBehaviour
	{
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
					new GameObject($"{typeof(T).Name}").AddComponent<T>();
				}

				return _instance;
			}
		}

		public static bool isExists
		{
			get;
			private set;
		}

		public virtual bool isDontDestroyOnLoad => true;

		internal sealed override void Awake()
		{
			if (isExists)
			{
				Destroy(gameObject);
				return;
			}

			_instance = (T)this;
			isExists = true;

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
				isExists = false;
				_instance = null;
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
