using System;

namespace OneCog.Core
{
    public class RefCounted<T>
    {
        private readonly Func<T> _build;
        private readonly Action<T> _dispose;
        private readonly object _syncRoot;

        private long _refCount;

        public RefCounted(Func<T> build, Action<T> dispose)
        {
            _build = build;
            _dispose = dispose;

            _syncRoot = new object();
        }

        public bool HasInstance { get; private set; }

        public T Instance { get; private set; }

        public void AddReference()
        {
            lock (_syncRoot)
            {
                if (++_refCount == 1)
                {
                    Instance = _build();
                    HasInstance = true;
                }
            }
        }

        public void DropReference()
        {
            lock (_syncRoot)
            {
                if (--_refCount == 0)
                {
                    _dispose(Instance);
                    Instance = default(T);
                    HasInstance = false;
                }
            }
        }
    }
}
