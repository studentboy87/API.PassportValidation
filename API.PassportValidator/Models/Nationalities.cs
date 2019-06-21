using System.Runtime.Serialization;

namespace API.PassportValidator.Models
{
    [DataContract]
    public class Nationalities
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        [DataMember(Name = "Alpha3Code")]
        public string Alpha3Code { get; set; }
    }
}
