using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.BaseImpl;

namespace ERP.Lavanderia.Module.PacoteGeral
{
    public static class BaseObjectExtension
    {
        public static void SetPropertyValue1to1<T>(BaseObject obj, string propertyName, ref T propertyValueHolder, T newValue, string otherPropertyName) where T : BaseObject
        {
            if (propertyValueHolder == newValue)
                return;

            var prev = propertyValueHolder;
            propertyValueHolder = newValue;

            if (obj.IsLoading) return;

            if (prev != null && prev.GetMemberValue(otherPropertyName).Equals(obj))
                prev.SetMemberValue(otherPropertyName, null);

            if (propertyValueHolder != null)
                propertyValueHolder.SetMemberValue(otherPropertyName, obj);
        }
    }
}
