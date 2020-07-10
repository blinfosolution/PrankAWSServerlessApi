using System;

namespace Prank.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DbParamNotMappedAttribute : Attribute
    {

    }
}
