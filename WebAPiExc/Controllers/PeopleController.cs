using BL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPiExc.CODE;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;

namespace WebAPiExc.Controllers
{
    public class PeopleController : ApiController
    {
       
        [HttpGet]
        public HttpResponseMessage Index()
        {
            PersonManager pm = new PersonManager();
            var personFormServer =  pm.GetAll();
            if (personFormServer == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                // return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, pm.GetAll());
        }
        [HttpPost]
        public HttpResponseMessage Insert([FromBody]Person person)
        {
            if (ModelState.IsValid)
            {
                PersonManager pm = new PersonManager();
                pm.Add(person);
                var response = Request.CreateResponse<Person>(System.Net.HttpStatusCode.Created, person);
                //  response.Headers.Location = new Uri("/Products/" + person.ID);
                response.Headers.Location = new Uri(Request.RequestUri + "/" + person.ID);
                return response;
            }
            List<Error> errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new Error { FieldName = x.Key.Split('.').Last(), ErrorMsg = x.Value.Errors[0].ErrorMessage }).ToList();
            return Request.CreateResponse(HttpStatusCode.InternalServerError,errors);
        }

        [HttpPut]
        public HttpResponseMessage Update(int id,[FromBody] Person person)
        {
            PersonManager pm = new PersonManager();
            person.ID = id;
           
            var personFormServer = pm.GetByID(id);
            if (personFormServer ==null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            pm.Update(person);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            PersonManager pm = new PersonManager();
            var personFormServer = pm.GetByID(id);
            if (personFormServer == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            pm.Delete(id);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
           
        }
        [HttpPost]
        public bool Login([FromBody] UserDetails userDetails)
        {
            return userDetails.UserName == "rafa" && userDetails.Password == "12345";
        }
    }
}