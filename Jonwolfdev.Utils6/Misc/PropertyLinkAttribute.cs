using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Misc
{
    /// <summary>
    /// This is a helper attribute to link the same property in different models.
    /// For example, Name can be inside entity, read model (or dto), etc.
    /// If you add this attribute (PropertyLink("ImageName")) to both properties in different classes,
    /// it will be easier to identify.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class PropertyLinkAttribute : Attribute
    {
        public PropertyLinkAttribute(int propId)
        {
            if (propId < 0)
                throw new ArgumentException("Must be greater than -1", nameof(propId));
        }
    }
}
