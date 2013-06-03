using System;
using System.Collections.Generic;
using System.Linq;
using MIMER.RFC2045;
using MIMER.RFC2183;
using MIMER.RFC2633.Pattern;
using MIMER.RFC822;
using MIMER.RFC2045.Pattern;

namespace MIMER.RFC2633
{
    class SMIMEFieldParser:FieldParserDecorator
    {
        #region IFieldParser Members        

        public SMIMEFieldParser(ContentDispositionFieldParser original)
            : base(original)
        {
            Original = original;

            var pattern = PatternFactory.GetInstance().Get(typeof (ApplicationSubTypePatern));
            var applicationSubtypePattern = pattern as ApplicationSubTypePatern;

            pattern = PatternFactory.GetInstance().Get(typeof (SMIMEApplicationSubTypePattern), new object[]{applicationSubtypePattern});
            var sMIMEPattern = pattern as SMIMEApplicationSubTypePattern;

            if (sMIMEPattern == null)
                throw new ApplicationException("Could not retrive a " + typeof(SMIMEApplicationSubTypePattern).Name);

           sMIMEPattern.Compile();
        }

        public ContentDispositionFieldParser Original { get; set; }

        public override void CompilePattern()
        {
            DecoratedFieldParser.CompilePattern();
        }

        public override void Parse(ref IList<Field> fields, ref string fieldString)
        {
            DecoratedFieldParser.Parse(ref fields, ref fieldString);

            var smimeFields = from field in fields
                              where field is ContentTypeField &&
                                    !string.IsNullOrEmpty(((ContentTypeField) field).Parameters["smime-type"])
                              select new SMIMETypeField(field as ContentTypeField);

            
            foreach (var smimeTypeField in smimeFields.ToList())
            {
                fields.Add(smimeTypeField);
            }
            

            
        }

        #endregion
    }
}
