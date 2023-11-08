using System.Runtime.Serialization;
namespace testinglogin.Models
{
    public class Chart
    {
        [DataContract]
        public class DataPoint
        {
            public DataPoint(string label, dynamic y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public dynamic Y = null;
        }

    }
}
