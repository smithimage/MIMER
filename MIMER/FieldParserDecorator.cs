using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIMER.RFC822;

namespace MIMER
{
    public abstract class FieldParserDecorator:IFieldParser
    {
        protected FieldParserDecorator(IFieldParser original)
        {
            DecoratedFieldParser = original;
        }

        protected internal IFieldParser DecoratedFieldParser { get; set; }


        public abstract void Parse(ref IList<Field> fields, ref string fieldString);
        public abstract void CompilePattern();
    }
}
