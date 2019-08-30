using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Cors;
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
        /// probably want to use pagination. And you can use query paramenters in the url
        /// to specify the limit anf offset.
        ///
        /// You search for matches or partially in first name and last name by using then name query parameter.
        ///
        /// To know how many people we have in the database so you can generate the
        /// amount of pages possible you can look at the header of the request at X-Total-Count.
        ///
        /// WARNING:
        /// A person that has an active value of false will not be returned.
        /// 
        /// </summary>
        /// <returns>A Json serialized string of all the people.</returns>
        ///
        [HttpGet]
        public string Get()
        {
            // Get query parameters.
            int.TryParse(HttpContext.Request.Query["limit"] , out int limit);
            int.TryParse(HttpContext.Request.Query["offset"], out int offset);
            int.TryParse(HttpContext.Request.Query["speed"], out int speed);
            string name = HttpContext.Request.Query["name"].ToString();

            List<People> people = new List<People>();

            using (var context = new PeopleContext())
            {
                // We want to store the query and so we can build it along the way.
                IEnumerable<People> query = null;

                // Check to see if a name was set.
                if (string.IsNullOrWhiteSpace(name))
                {
                    // We have found no name so we can start the query by returning everything but in-active memebers.
                    query = context.People.Where(x => x.Active == true);
                }
                else
                {
                    // We have a name so we want to filter out only matching cases for the search criteria.
                    query = context.People.Where(x => (x.FirstName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower())) && x.Active == true);
                }

                // Add the total amount of people in the header.
                Response.Headers.Add("People-Total-Count", query.Count().ToString());
                Response.Headers.Add("access-control-expose-headers", "content-disposition");

                // If we do not specify a limit it will be zero so we want to return everything.
                if (limit < 1)
                {
                    // We do not have a limit setup (not pagination) query all items and store them in variable.
                    people = query.ToList();
                }
                else
                {
                    // Pagination numbers recceveid so lets do that.
                    people = query.Skip(offset).Take(limit).ToList();
                }                
            }

            // Simulate slowness for search
            if (speed > -1)
            {
                int milliseconds = speed;
                Thread.Sleep(milliseconds);
            }            

            // Convert the list of people to a Json representation.
            string jsonOutput = JsonConvert.SerializeObject(people);             
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
                // Get the person by if and make sure it is active only.
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
        /// <param name="person">The json string representing a people object.</param>
        /// <returns> A tring will be returned and success if it succeeded</returns>
        [HttpPost]
        public string Post([FromBody]People person)
        {
            try
            {
                using (var context = new PeopleContext())
                {
                    context.People.Add(person);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "success";
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
        /// <param name="updatedPerson">The Json object string representation for updating the object.</param>
        /// <returns> A tring will be returned and success if it succeeded</returns>
        [HttpPut("{id}")]
        public string Put(int id, [FromBody]People updatedPerson)
        {
            try
            {
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
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "success";
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
