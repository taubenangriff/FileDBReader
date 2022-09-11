﻿using FileDBSerializer.ObjectSerializer.SerializationHandlers;
using FileDBSerializing.ObjectSerializer;
using System;
using System.Reflection;

namespace FileDBSerializer.ObjectSerializer.HandlerSelector
{
    public class ArrayHandlerSelector : IHandlerSelector
    {
        public HandlerType GetHandlerFor(PropertyInfo propertyInfo)
        {            
            Type arrayContentType = propertyInfo.GetNullablePropertyType().GetElementType()!;

            if (arrayContentType.IsPrimitiveType())
            {
                return HandlerType.PrimitiveArray;
            }
            else if (propertyInfo.HasAttribute<FlatArrayAttribute>())
            {
                return HandlerType.FlatArray;
            }
            else if (arrayContentType.IsReference() || arrayContentType.IsStringType())
            {
                return HandlerType.ReferenceArray;
            }

            //fuck the implementation for now
            throw new NotImplementedException();
        }
    }
}