using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MIMER
{
    public class PatternFactory
    {
        protected static PatternFactory s_Instance = null;

        private IDictionary<Type, IPattern> m_PatternPool;

        protected PatternFactory()
        {
            m_PatternPool = new Dictionary<Type, IPattern>();
        }

        public static PatternFactory GetInstance()
        {
            if(s_Instance == null)
                s_Instance = new PatternFactory();

            return s_Instance;
        }

        public IPattern Get(Type type)
        {
            if(type.GetInterface(typeof(IPattern).Name) == null)
                throw new ArgumentException("Type does not implement " + typeof(IPattern).Name, "type");

            if(!m_PatternPool.ContainsKey(type))
            {
                ConstructorInfo cInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[]{}, null);
                if (cInfo != null)
                {
                    IPattern pattern = Activator.CreateInstance(type) as IPattern;
                    if (pattern != null)
                        m_PatternPool.Add(pattern.GetType(), pattern);
                }   
            }
            return m_PatternPool[type];
            
        }

        public IPattern Get(Type type, object[] args)
        {
            if (type.GetInterface(typeof(IPattern).Name) == null)
                throw new ArgumentException("Type does not implement " + typeof(IPattern).Name, "type");

            if (!m_PatternPool.ContainsKey(type))
            {
                IPattern pattern = Activator.CreateInstance(type, args) as IPattern;
                if (pattern != null)
                    m_PatternPool.Add(pattern.GetType(), pattern);
            }
            return m_PatternPool[type];
        }

        
    }
}
