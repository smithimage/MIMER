

using MIMER.RFC2045;

namespace MIMER.RFC2633
{
    class SMIMETypeField:ContentTypeField
    {
        public SMIMETypeField(ContentTypeField field)
        {
            Type = field.Type;
            SubType = field.SubType;
            Body = field.Body;
            Name = field.Name;
            Parameters = field.Parameters;
        }

        public string SMIMEType
        {
            get
            {
                return Parameters["smime-type"];
            }
            set
            {
                Parameters["smime-type"] = value;
            }
        }
    }
}
