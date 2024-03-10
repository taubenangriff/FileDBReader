﻿using AnnoMods.ObjectSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnnoMods.BBDom
{
    public static class CreateNodesExtension
    {
        public static Tag CreateTag(this IBBDocument doc, string Name)
        {
            var newTagId = doc.TagSection.GetOrCreateTagId(Name);
            return new Tag() { ID = newTagId, ParentDoc = doc as BBDocument };
        }

        public static Attrib CreateAttrib(this IBBDocument doc, string Name)
        {
            var newAttribId = doc.TagSection.GetOrCreateAttribId(Name);
            return new Attrib() { ID = newAttribId, ParentDoc = doc as BBDocument };
        }

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, byte[] content)
        {
            var attrib = doc.CreateAttrib(Name);
            attrib.Content = content;
            return attrib;
        }

        #region Numerics 
        public static Attrib CreateAttrib(this IBBDocument doc, string Name, bool b) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(b));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, byte b) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(b));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, sbyte b) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(b));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, short s) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(s));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, ushort u) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(u));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, int i) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(i));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, uint i) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(i));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, long i) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(i));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, ulong i) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(i));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, float i) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(i));

        public static Attrib CreateAttrib(this IBBDocument doc, string Name, double i) =>
            doc.CreateAttrib(Name, PrimitiveTypeConverter.GetBytes(i));

        #endregion
        public static Attrib CreateAttrib(this IBBDocument doc, string Name, string content, Encoding encoding)
            => doc.CreateAttrib(Name, encoding.GetBytes(content));
    }
}