using LTHT.PeopleManagement.Helpers;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace LTHT.PeopleManagement.Models
{
    /// <summary>
    /// Represents a Person
    /// </summary>
    [DataContract]
    public class Person : EntityBase
    {
        /// <summary>
        /// Backing field for <see cref="FirstName"/>
        /// </summary>
        private string firstName = string.Empty;

        /// <summary>
        /// Gets or sets the FirstName property
        /// </summary>
        [DataMember(Name = "firstName")]
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                // If the firstname changes update the palindrome status
                if (SetProperty(ref this.firstName, value))
                {
                    this.CheckPalindrome();
                }
            }
        }

        /// <summary>
        /// Backing field for <see cref="LastName"/>
        /// </summary>
        private string lastName = string.Empty;

        /// <summary>
        /// Gets or sets the LastName property
        /// </summary>
        [DataMember(Name = "lastName")]
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                // If the lastname changes refresh the palindrome status
                if (SetProperty(ref this.lastName, value))
                {
                    this.CheckPalindrome();
                }
            }
        }

        /// <summary>
        /// Gets a formatted FullName
        /// </summary>
        public string FullName => $"{this.firstName} {this.lastName}";

        /// <summary>
        /// Backing field for <see cref="Authorised"/>
        /// </summary>
        private bool authorised;

        /// <summary>
        /// Gets or sets a flag indicating if the person is authorised
        /// </summary>
        [DataMember(Name = "isAuthorised")]
        public bool Authorised
        {
            get { return this.authorised; }
            set { SetProperty(ref this.authorised, value); }
        }

        /// <summary>
        /// Backing field for <see cref="Enabled"/>
        /// </summary>
        private bool enabled;

        /// <summary>
        /// Gets or sets a flag indicating if the person is enabled
        /// </summary>
        [DataMember(Name = "isEnabled")]
        public bool Enabled
        {
            get { return this.enabled; }
            set { SetProperty(ref this.enabled, value); }
        }

        /// <summary>
        /// Backing field for <see cref="Valid"/>
        /// </summary>
        private bool valid;

        /// <summary>
        /// Gets or sets a flag indicating if the person is valid
        /// </summary>
        [DataMember(Name = "isValid")]
        public bool Valid
        {
            get { return this.valid; }
            set { SetProperty(ref this.valid, value); }
        }

        /// <summary>
        /// Backing field for <see cref="Palindrome"/>
        /// </summary>
        private bool palindrome;

        /// <summary>
        /// Gets or sets a flag indicating if the person's name is a plaindrome
        /// </summary>
        public bool Palindrome
        {
            get { return this.palindrome; }
            set { SetProperty(ref this.palindrome, value); }
        }

        /// <summary>
        /// Backing field for <see cref="Colours"/>
        /// </summary>
        private ObservableRangeCollection<Colour> colours = new ObservableRangeCollection<Colour>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Colour"/>
        /// </summary>
        [DataMember(Name = "colours")]
        public ObservableRangeCollection<Colour> Colours
        {
            get { return this.colours; }
            set { SetProperty(ref this.colours, value); }
        }

        /// <summary>
        /// Check if the person's name is spelt the same forwards as backwards (excluding spaces)
        /// </summary>
        /// <remarks>Calculated and stored when the name changes rather than calculated per read</remarks>
        private void CheckPalindrome()
        {
            var nameWithoutSpaces = ((this.FirstName + this.LastName).ToLower().ToCharArray().Where(c => !Char.IsWhiteSpace(c)));
            this.Palindrome = nameWithoutSpaces.SequenceEqual(nameWithoutSpaces.Reverse());
        }
    }
}