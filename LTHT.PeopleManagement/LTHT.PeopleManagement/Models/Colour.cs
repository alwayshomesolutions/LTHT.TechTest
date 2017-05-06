using System.Runtime.Serialization;

namespace LTHT.PeopleManagement.Models
{
    /// <summary>
    /// Represent a colour that can be associated with a person
    /// </summary>
    [DataContract]
    public class Colour : EntityBase
    {
        /// <summary>
        /// Backing field for <see cref="Name"/>
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Gets or sets the Name property
        /// </summary>
        [DataMember(Name = "name")]
        public string Name
        {
            get { return this.name; }
            set { SetProperty(ref this.name, value); }
        }
    }
}
