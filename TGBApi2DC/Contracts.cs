using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TGBApi
{
    [DataContract]
    public class rootJson
    {
        [DataMember(Name = "version")]
        public string version { get; set; }
        [DataMember(Name = "types")]
        public List<Types> types { get; set; }
        [DataMember(Name = "methods")]
        public List<Methods> methods { get; set; }
    }
    [DataContract]
    public class Types
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "description")]
        public string description { get; set; }
        [DataMember(Name = "fields")]
        public List<TypeFields> fields { get; set; }
        [DataMember(Name = "extended_by")]
        public List<string> extended_by { get; set; }
    }
    [DataContract]
    public class TypeFields
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "description")]
        public string description { get; set; }
        [DataMember(Name = "types")]
        public string[] type { get; set; }
        [DataMember(Name = "optional")]
        public bool optional { get; set; }
        [DataMember(Name = "default")]
        public string _default { get; set; }
        public object defaultValue { get; set; }
        public object altProperty { get; set; }
        public bool hasHeader { get; set; }
        [OnDeserialized]
        void OnDeserialized(StreamingContext c)
        {
            hasHeader = true;
        }
    }
    [DataContract]
    public class Methods
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "description")]
        public string description { get; set; }
        [DataMember(Name = "fields")]
        public List<MethodsFields> fields { get; set; }
        [DataMember(Name = "return_types")]
        public List<string> return_types { get; set; }
    }
    [DataContract]
    public class MethodsFields
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "optional")]
        public bool required { get; set; }
        [DataMember(Name = "types")]
        public List<string> types { get; set; }
        [DataMember(Name = "description")]
        public string description { get; set; }
        [DataMember(Name = "default")]
        public string _default { get; set; }
    }
}
