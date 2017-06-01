using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TheLongestString
{
    public class AssemblyManager
    {
        public string Namespace { get; private set; }

        public Assembly Assembly { get; private set; }

        public AssemblyManager(string _namespace)
        {
            this.Namespace = _namespace;
            foreach (AssemblyName ably in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                if (ably.FullName.StartsWith(_namespace))
                {
                    this.Assembly = Assembly.Load(ably);
                    break;
                }
            }
            if (this.Assembly == null)
                throw new ArgumentException(string.Format("Cannot find any assembly corresponding to namespace", this.Namespace));
        }

        public object GetEnum(string typeName, string name)
        {
            Type type = GetType(typeName);
            FieldInfo fieldInfo = type.GetField(name);
            return fieldInfo.GetValue(null);
        }

        public Type GetType(string typeName)
        {
            Type type = null;
            string[] names = typeName.Split('.');

            if (names.Length > 0)
                type = this.Assembly.GetType(this.Namespace + "." + names[0]);

            for (int i = 1; i < names.Length; ++i)
            {
                type = type.GetNestedType(names[i], BindingFlags.NonPublic);
            }
            return type;
        }

        public object InvokeMethod(Type type, object obj, string func, object[] parameters)
        {
            MethodInfo methInfo = type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return methInfo.Invoke(obj, parameters);
        }

        public object CreateInstance(string name, object[] parameters)
        {
            Type type = GetType(name);

            ConstructorInfo[] ctorInfos = type.GetConstructors();
            foreach (ConstructorInfo ci in ctorInfos)
            {
                try
                {
                    return ci.Invoke(parameters);
                }
                catch { }
            }

            return null;
        }
    }
}