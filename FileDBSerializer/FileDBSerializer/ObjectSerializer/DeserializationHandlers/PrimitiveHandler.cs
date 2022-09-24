﻿using FileDBSerializing;
using FileDBSerializing.ObjectSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDBSerializer.ObjectSerializer.DeserializationHandlers
{
    public class PrimitiveHandler : IDeserializationHandler
    {
        private static PrimitiveTypeConverter? PrimitiveConverter;

        public PrimitiveHandler()
        {
            PrimitiveConverter ??= new PrimitiveTypeConverter();
        }

        public object? Handle(IEnumerable<FileDBNode> nodes, Type targetType, FileDBSerializerOptions options)
        {
            if (nodes.Count() != 1)
                throw new InvalidOperationException("PrimitiveHandler can handle exactly one node");
            var node = nodes.First();
            if (node is not Attrib attrib)
                throw new InvalidOperationException("Only attribs can be handled by PrimitiveHandler");

            return PrimitiveConverter!.GetObject(targetType, attrib.Content);
        }
    }
}