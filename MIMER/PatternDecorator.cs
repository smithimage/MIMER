using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MIMER
{
    public abstract class PatternDecorator:ICompiledPattern
    {
        protected PatternDecorator(ICompiledPattern decoratedPattern)
        {
            DecoratedPattern = decoratedPattern;
        }

        protected internal ICompiledPattern DecoratedPattern { get; set; }

        public abstract string TextPattern
        { 
            get;
        }

        public abstract Regex RegularExpression
        { 
            get;
        }

        public abstract void Compile();
        
    }
}
