using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using People_Dictionary.Classes;

namespace People_Dictionary.Controllers
{
    /// <summary>
    /// People Controller
    ///
    /// The People Controller class is made so that you can inteface with the people database.
    /// 
    /// We are using Json for our input and output of the methods.
    /// </summary>
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        /// <summary>
        /// GET: api/people
        ///
        /// A get request for getting all the people from the database. You will
        /// probably want to use pagination.
        ///
        /// WARNING:
        /// A person that has an active value of false will not be returned.
        /// 
        /// </summary>
        /// <param name="limit">The max amount of items you want to return as integer.</param>
        /// <param name="offset">The amount of items you want to skip as integer.</param>
        /// <returns>A Json serialized string of all the people.</returns>
        ///
        [HttpGet("{limit}/{offset}")]
        public string Get(int limit = 10, int offset = 0)
        {
            List<People> allPeople = new List<People>();

            using (var context = new PeopleContext())
            {
                // Get all the people from the database.
                allPeople = context.People.Where(x => x.Active == true).Skip(offset).Take(limit).ToList();
            }

            // Convert the list of people to a Json representation.
            string jsonOutput = JsonConvert.SerializeObject(allPeople);             
            return jsonOutput;
        }

        /// <summary>
        /// GET api/people/5
        ///
        /// You can make a request to get one person from the database. You will supply the
        /// id of the person in the url.
        ///
        /// WARNING:
        /// The people that have their active status turned off will not be
        /// returned.
        /// 
        /// </summary>
        /// <param name="id">The id of the person you would like returned to you.</param>
        /// <returns>Returns a Json string including the person's details</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            People person = null;

            using (var context = new PeopleContext())
            {
                // Get the person by if and make sure it is actibe only.
                person = context.People.Where(x => x.Id == id && x.Active == true).SingleOrDefault();                
            }

            // If we have found the person we need to make a Jsopn representation of it.
            string jsonOutput = JsonConvert.SerializeObject(person);
            return jsonOutput;
        }

        /// <summary>
        /// POST api/people
        ///
        /// You can create a person in the database with this http request.
        ///
        /// INFO:
        /// The Created Date and Updated Date fields are managed by the database and will not be saved.
        ///  
        /// </summary>
        /// <param name="value">The json string representing a people object.</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
            People person = (People)JsonConvert.DeserializeObject(value);

            using (var context = new PeopleContext())
            {
                context.People.Add(person);
                context.SaveChanges();
            }            
        }

        /// <summary>
        /// PUT api/people/5
        /// You can update the person with this request.
        ///                
        /// INFO:
        /// The Created Date and the Updated Date are managed by the database and will not be updated.
        /// In this case you can update the active status and this will be updated.
        /// 
        /// </summary>
        /// <param name="id">The id of the person in the database you want to edit.</param>
        /// <param name="value">The Json object string representation for updating the object.</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            // Get the person that has the new information.
            People updatedPerson = (People)JsonConvert.DeserializeObject(value);

            using (var context = new PeopleContext())
            {
                // Get the person that we want to update.
                People person = context.People.Where(x => x.Id == id).SingleOrDefault();

                if(person != null)
                {
                    // We have a person so let's update the values.
                    person.FirstName = updatedPerson.FirstName;
                    person.LastName = updatedPerson.LastName;
                    person.StreetAddress = updatedPerson.StreetAddress;
                    person.StreetAddressAdditional = updatedPerson.StreetAddressAdditional;
                    person.City = updatedPerson.City;
                    person.State = updatedPerson.State;
                    person.ZipCode = updatedPerson.ZipCode;
                    person.DateOfBirth = updatedPerson.DateOfBirth;
                    person.Interests = updatedPerson.Interests;
                    person.AvatarUrl = updatedPerson.AvatarUrl;
                    person.Active = updatedPerson.Active;

                    // Update the user.
                    context.People.Update(person);
                    context.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// DELETE api/people/5
        ///
        /// With this request you can delete a person from the database.
        ///
        /// INFO:
        /// Regradeless of the active status a person can still be removed.
        /// 
        /// </summary>
        /// <param name="id">The id of the person in the database you wish to remove.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var context = new PeopleContext())
            {
                // Find the person we want to remove in the database.
                People person = context.People.Where(x => x.Id == id).SingleOrDefault();

                if (person != null)
                {
                    context.People.Remove(person);
                    context.SaveChanges();
                }
            }
        }        
    }
}
