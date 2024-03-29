using Microsoft.AspNetCore.Mvc.Filters;

namespace Byte.Core.Common.Attributes
{
    public class IgnoreAttribute : Attribute, IFilterMetadata
    {
    }
}
