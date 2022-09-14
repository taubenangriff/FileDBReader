﻿using FileDBSerializing.EncodingAwareStrings;
using FileDBSerializing.ObjectSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDBReader_Tests.TestSerializationData
{
    public class NewObject
    {
        public int? PrimitiveObject { get; set; }
        public AnyChild? RefObject { get; set; }
        public ArrayElement[] RefArray { get; set; }
        public int[] PrimitiveArray { get; set; }

        [FlatArray]
        public ArrayElement[] FlatArray { get; set; }
        public String[] StringArray { get; set; }
        
        public (String, int)[] TupleArray { get; set; }

        public CountsElement counts { get; set; }
    }

    public class AnyChild
    {
        public String DefaultStr { get; set; }
        public UTF32String EncAwareStr {get; set;}
    }

    public class CountsElement
    {
        public int size { get; set; }
        [FlatArray]
        public CountsElementEntry[] None { get; set; }
    }

    public class CountsElementEntry
    { 
        public (byte, byte) None { get; set; }  
    }

    public class ArrayElement
    {
        public uint ElementContent { get; set; } = 5;
    }
}
