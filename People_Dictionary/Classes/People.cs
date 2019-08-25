using System;
namespace People_Dictionary.Classes
{
    /// <summary>
    /// This is the class for the people and will be used for entity framework to
    /// use.
    ///
    /// The names of the properties match the same as the database so entity framework knows how to map it.
    /// </summary>
    public class People
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string StreetAddressAdditional { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Interests { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
    }
}
