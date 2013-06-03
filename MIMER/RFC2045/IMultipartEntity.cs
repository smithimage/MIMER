using System;
namespace MIMER.RFC2045
{
    public interface IMultipartEntity: IEntity
    {
        System.Collections.Generic.IList<IEntity> BodyParts { get; set; }
    }
}
