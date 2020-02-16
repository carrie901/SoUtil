using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Framework.Services;

namespace Framework.Context
{
    public class Context
    {
        private bool innerContainer = false;
        private Context contextBase;
        private IServiceContainer container;
        private Dictionary<string, object> attributes;

        public Context() : this(null, null)
        {
        }

        public Context(IServiceContainer container, Context contextBase)
        {
            this.attributes = new Dictionary<string, object>();
            this.contextBase = contextBase;
            this.container = container;
            if (this.container == null)
            {
                this.innerContainer = true;
                this.container = new ServiceContainer();
            }
        }
        
        public virtual bool Contains(string name, bool cascade = true)
        {
            if (this.attributes.ContainsKey(name))
                return true;

            if (cascade && this.contextBase != null)
                return this.contextBase.Contains(name, cascade);

            return false;
        }

        public virtual object Get(string name, bool cascade = true)
        {
            return this.Get<object>(name, cascade);
        }

        public virtual T Get<T>(string name, bool cascade = true)
        {
            object v;
            if (this.attributes.TryGetValue(name, out v))
                return (T)v;

            if (cascade && this.contextBase != null)
                return this.contextBase.Get<T>(name, cascade);

            return default(T);
        }

        public virtual void Set(string name, object value)
        {
            this.Set<object>(name, value);
        }

        public virtual void Set<T>(string name, T value)
        {
            this.attributes[name] = value;
        }

        public virtual object Remove(string name)
        {
            return this.Remove<object>(name);
        }

        public virtual T Remove<T>(string name)
        {
            if (!this.attributes.ContainsKey(name))
                return default(T);

            object v = this.attributes[name];
            this.attributes.Remove(name);
            return (T)v;
        }

        public virtual IServiceContainer GetContainer()
        {
            return container;
        }

        public virtual object GetService(Type type)
        {
            object result = container.Resolve(type);
            if (result != null)
                return result;

            if (this.contextBase != null)
                return contextBase.GetService(type);

            return null;
        }
        
        public virtual object GetService(string name)
        {
            object result = container.Resolve(name);
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService(name);

            return null;
        }

        public virtual T GetService<T>()
        {
            T result = container.Resolve<T>();
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService<T>();

            return default(T);
        }

        public virtual T GetService<T>(string name)
        {
            T result = container.Resolve<T>(name);
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService<T>(name);

            return default(T);
        }

        public virtual void Register<T>(T target)
        {
            Register(typeof(T).Name, target);
        }

        public virtual void Register<T>(Func<T> factory)
        {
            Register(typeof(T).Name, factory);
        }

        public virtual void Register<T>(string name, T target)
        {
            container.Register(name, target);
        }

        public virtual void Register<T>(string name, Func<T> factory)
        {
            container.Register(name, factory);
        }

        public virtual void UnRegister<T>()
        {
            container.Unregister<T>();
        }

        public virtual void UnRegister(Type type)
        {
            container.Unregister(type);
        }

        public virtual void UnRegister(string name)
        {
            container.Unregister(name);
        }

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this.innerContainer && this.container != null)
                    {
                        IDisposable dis = this.container as IDisposable;
                        if (dis != null)
                            dis.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~Context()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}