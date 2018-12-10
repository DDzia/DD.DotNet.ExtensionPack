using System;

namespace DD.DotNet.ExtensionPack
{
    /// <summary>Object which presents disposable object.</summary>
    public abstract class DisposableObject : IDisposable
    {
        /// <summary>Flag is idicates if object was been disposed.</summary>
        protected bool Disposed { get; private set; }

        /// <summary>Thrown if current object was been disposed.</summary>
        /// <exception cref="ObjectDisposedException">Thrown if current object was been disposed.</exception>
        protected void ThrowIfDisposed()
        {
            if (Disposed) throw new ObjectDisposedException($"Object({GetType().Name}) is disposed.");
        }

        /// <summary>Release managed resources.</summary>
        protected virtual void ReleaseManaged() { }
        /// <summary>Release unmanaged resources.</summary>
        protected virtual void ReleaseUnmanaged() { }

        private void Dispose(bool disposing)
        {
            if (disposing && !Disposed)
            {
                ReleaseManaged();
                Disposed = true;
            }

            ReleaseUnmanaged();
        }

        /// <summary>
        ///   <para>Dispose object</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
