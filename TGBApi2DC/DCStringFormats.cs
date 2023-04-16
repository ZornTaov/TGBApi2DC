using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBApi2DC
{
    public class DCStringFormats
    {
        /// <summary>
        /// Inserts License region
        /// <para>Params: Year</para>
        /// </summary>
        public static string LICENSE =
            "#region License \r\n" +
            "//MIT License\r\n" +
            "//Copyright(c) [{0}]\r\n" +
            "//[Xylex Sirrush Rayne]\r\n" +
            "//\r\n" +
            "//Permission is hereby granted, free of charge, to any person obtaining a copy\r\n" +
            "//of this software and associated documentation files (the \"Software\"), to deal\r\n" +
            "//in the Software without restriction, including without limitation the rights\r\n" +
            "//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell\r\n" +
            "//copies of the Software, and to permit persons to whom the Software is\r\n" +
            "//furnished to do so, subject to the following conditions:\r\n" +
            "//\r\n" +
            "//The above copyright notice and this permission notice shall be included in all\r\n" +
            "//copies or substantial portions of the Software.\r\n" +
            "//\r\n" +
            "//THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\n" +
            "//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\r\n" +
            "//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\r\n" +
            "//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\r\n" +
            "//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\r\n" +
            "//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\r\n" +
            "//SOFTWARE.\r\n" +
            "#endregion";

        public static string LIST_HEADER =
            "using System.Collections.Generic;";
        public static string HTTP_HEADER =
            "using System.IO;\r\nusing System.Net.Http;";

        /// <summary>
        /// File Header
        /// </summary>
        public static string FILE_HEADER =
            "using System.Runtime.Serialization;\r\n" +
            "namespace DreadBot";

        /// <summary>
        /// DataContract Header
        /// <para>Params: ClassName</para>
        /// </summary>
        public static string DC_HEADER =
            "\t[DataContract]\r\n" +
            "\tpublic class {0}";

        /// <summary>
        /// KnownType header, used if a class has childs
        /// <para>Params: ClassName</para>
        /// </summary>
        public static string KNOWN_TYPE =
            "\t[KnownType(typeof({0}))]";

        /// <summary>
        /// DataContract Header
        /// <para>Params: ClassName</para>
        /// </summary>
        public static string DC_HEADER_INHERITS =
            "\t[DataContract]\r\n" +
            "\tpublic class {0} : {1}";

        /// <summary>
        /// partial Methods Header
        /// </summary>
        public static string METHOD_HEADER =
            "\tpublic partial class Methods";

        /// <summary>
        /// Constructor method
        /// <para>Params: name, param1, param1 name</para>
        /// </summary>
        public static string CONSTRUCTOR =
            "\t\tpublic {0}({1} {2}) : base({2}) {{ }}";
        /// <summary>
        /// DataMember Field
        /// <para>Params: name, required</para>
        /// </summary>
        public static string DM_FIELD_HEADER =
            "\t\t[DataMember(Name = \"{0}\"{1})]";
        /// <summary>
        /// DataMember Field
        /// <para>Params: name, type, default value</para>
        /// </summary>
        public static string DM_FIELD =
            "\t\tpublic {1} {0} {{ get; set; }}{2}";
        /// <summary>
        /// DataMember Field
        /// <para>Params: name, type, alt property</para>
        /// </summary>
        public static string DM_FIELD_PROPERTY =
            "\t\tpublic {1} {0} {{ get {{ return {2}; }} set {{ {2} = value; }} }}";
        /// <summary>
        /// Use if required = true
        /// </summary>
        public static string DM_REQUIRED = ", IsRequired = true";
        /// <summary>
        /// use if required = false
        /// </summary>
        public static string DM_EMIT_DEFAULT = ", EmitDefaultValue = false";

        /// <summary>
        /// Inserts summary tag
        /// <para>Params: description, tabs</para>
        /// </summary>
        public static string SUMMARY =
            "{1}/// <summary>\r\n" +
            "{1}/// {0}\r\n" +
            "{1}/// </summary>";
        /// <summary>
        /// Inserts param tag
        /// <para>Params: parameterName, description</para>
        /// </summary>
        public static string PARAM = "/// <param name=\"{0}\">{1}</param>";
        /// <summary>
        /// Inserts returns tag
        /// <para>Params: description</para>
        /// </summary>
        public static string RETURNS = "/// <returns>{0}</returns>";
    }
}
