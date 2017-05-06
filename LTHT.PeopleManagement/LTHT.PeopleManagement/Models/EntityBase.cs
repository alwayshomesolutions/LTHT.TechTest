using LTHT.PeopleManagement.Helpers;
using System.Runtime.Serialization;

namespace LTHT.PeopleManagement.Models
{
    /// <summary>
    /// Base class for all model objects providing common properties
    /// </summary>
    [DataContract]
    public abstract class EntityBase : ObservableObject
    {
        /// <summary>
        /// Base class for all entities
        /// </summary>
        public EntityBase()
        {
        }

        /// <summary>
        /// Id for <see cref="EntityBase"/>
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
