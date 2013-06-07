using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIMER.RFC2183;

namespace MIMER.RFC2045
{
    public static class MessageExtentions
    {

        public static string GetAttachmentName(this IEntity entity)
        {
            var dispositionField = entity.FindField<ContentDispositionField>();
            var contentTypeField = entity.FindField<ContentTypeField>();

            string name;
            if (dispositionField == null)
            {
                name = contentTypeField.Parameters["name"];
            }
            else
            {
                if (string.IsNullOrEmpty(dispositionField.Parameters["filename"]))
                {
                    name = contentTypeField.Parameters["name"];
                }
                else
                {
                    name = dispositionField.Parameters["filename"];
                }
            }
            return name;
        }

        public  static IAttachment AsAttachment(this IEntity entity)
        {
            ContentDispositionField dispositionField = entity.FindField<ContentDispositionField>();
            ContentTypeField contentTypeField = entity.FindField<ContentTypeField>();

            if(dispositionField == null && contentTypeField == null)
                return new NullAttachment();

            if (contentTypeField != null)
            {
                IAttachment attachment = new Attachment();

                if (dispositionField != null)
                {
                    attachment.Disposition = dispositionField.Disposition;
                }

                attachment.Name = entity.GetAttachmentName();
                attachment.Data = entity.Body;
                attachment.Type = contentTypeField.Type;
                attachment.SubType = contentTypeField.SubType;
                return attachment;
            }

            return new NullAttachment();
        }


    }
}
