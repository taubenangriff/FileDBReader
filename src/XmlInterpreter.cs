﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace FileDBReader
{
    //expected structure of interpreter
    /*<Converts>
          <Default Type ="int" />
          <Converts>
            <Convert Path ="//Text" Type ="String" />
          </Converts>
        </Converts>
     */

    /// <summary>
    /// converts hex strings in an xml file into their types using conversion rules set up in an external xml file.
    /// </summary>
    class XmlInterpreter
    {
        
        public XmlInterpreter() {
            
        }

        public XmlDocument Interpret(String docPath, String InterpreterPath) 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(docPath);

            XmlDocument interpreter = new XmlDocument();
            interpreter.Load(InterpreterPath);

            return Interpret(doc, interpreter);
        }

        public XmlDocument Interpret(XmlDocument doc, XmlDocument interpreter) 
        {
            //default type
            XmlNode defaultAttrib = null;
            defaultAttrib = interpreter.SelectSingleNode("/Converts/Default");
            var internalFileDBs = interpreter.SelectNodes("/Converts/InternalCompression/Element");
            var converts = interpreter.SelectNodes("/Converts/Converts/Convert");

            //Convert internal FileDBs before conversion
            foreach (XmlNode n in internalFileDBs) {
                var nodes = doc.SelectNodes(n.Attributes["Path"].Value);
                foreach (XmlNode node in nodes) {
                    var span = HexHelper.toSpan<byte>(node.InnerText);

                    


                    var filereader = new FileReader();
                    var decompressed = filereader.ReadSpan(span);
                    node.InnerText = "";
                    node.AppendChild(doc.ImportNode(decompressed.DocumentElement, true));

                    //write the current decompressed internal filedb to a file
                    //String path = Path.Combine("tests", "lists", DateTime.Now.ToString("HH-mm-ss-ff") + "_" + node.Name + ".bin");
                    //using FileStream fs = File.Create(path);
                    //fs.Write(span);
                }
            }

            //converts
            foreach (XmlNode x in converts) {
                try
                {
                    String Path = x.Attributes["Path"].Value;
                    var Nodes = doc.SelectNodes(Path);
                    ConvertNodeSet(Nodes, x);
                }
                catch (Exception e) {
                    Console.WriteLine("Path not correctly set lol");
                }
            }

            //DefaultType
            if (defaultAttrib != null) {
                //get a combined xpath of all
                List<String> StringList = new List<string>(); 
                foreach (XmlNode convert in converts)
                    StringList.Add(convert.Attributes["Path"].Value);
                foreach (XmlNode internalFileDB in internalFileDBs)
                    StringList.Add(internalFileDB.Attributes["Path"].Value);
                String xPath = String.Join(" | ", StringList);

                //select all text that is not in the combined path
                var Base = doc.SelectNodes("//*[text()]");
                var toFilter = doc.SelectNodes(xPath);
                var defaults = HexHelper.ExceptNodelists(Base, toFilter);
                ConvertNodeSet(defaults, defaultAttrib);
            }

            return doc;
        }

        //f* performance I won't write everything twice :) the cast should not take to long
        private void ConvertNodeSet(XmlNodeList matches, XmlNode ConverterInfo) 
        {
            IEnumerable<XmlNode> cast = matches.Cast<XmlNode>();
            ConvertNodeSet(cast, ConverterInfo);
        }

        private void ConvertNodeSet(IEnumerable<XmlNode> matches, XmlNode ConverterInfo)
        {
            //get type the nodeset should be converted to
            var type = Type.GetType("System." + ConverterInfo.Attributes["Type"].Value);
            //get encoding
            Encoding encoding = new UnicodeEncoding();
            if (ConverterInfo.Attributes["Encoding"] != null)
                encoding = Encoding.GetEncoding(ConverterInfo.Attributes["Encoding"].Value);
            //get structure
            String Structure = "Default";
            if (ConverterInfo.Attributes["Structure"] != null)
                Structure = ConverterInfo.Attributes["Structure"].Value;

            foreach (XmlNode match in matches)
            {
                switch (Structure)
                {
                    case "List":
                        InterpretAsList(match, type);
                        break;
                    case "Default":
                        InterpretSingleNode(match, type, encoding);
                        break;
                }
            }
        }

        private void InterpretAsList(XmlNode n, Type type)
        {
            try {
                String BinaryData = n.InnerText;
                String s = ConverterFunctions.ListFunctionsInterpret[type](BinaryData);
                n.InnerText = s;
            }
            catch (Exception ex) {
                Console.WriteLine("an Error occured: " + n.InnerText);
                Console.WriteLine(ex.Message);
            }
            
        }

        private void InterpretSingleNode(XmlNode n, Type type, Encoding e)
        {
            try
            {
                String BinaryData = n.InnerText;

                //make a bytesize check
                int ExpectedBytesize = 0;
                int StringSize = BinaryData.Length;

                if (type != typeof(String))
                {
                    ExpectedBytesize = Marshal.SizeOf(type);
                    if (type == typeof(Boolean)) {
                        if (StringSize != 2) { 
                            Console.WriteLine("Wrong Bytesize at {0}", n.Name);
                        }
                    }
                    else if (ExpectedBytesize != (StringSize / 2))
                    {
                        Console.WriteLine("Wrong Bytesize at {0}", n.Name);
                    }
                }
                
                String s = ConverterFunctions.ConversionRulesImport[type](BinaryData, e);
                n.InnerText = s;
            }
            catch (Exception ex) {
                Console.WriteLine("an Error occured: " + n.InnerText);
                Console.WriteLine(ex.Message);
                Console.WriteLine("at: {0}", n.Name);
            }
}
    }
}