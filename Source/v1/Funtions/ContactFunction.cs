using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TingTango.Source.v1.Models;
using TingTango.Source.v1.Repositories;

namespace TingTango.Source.v1.Functions
{
    public static class ContactFunction
    {
        [FunctionName(nameof(ContactList))]
        public static IActionResult ContactList(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = AppConstant.version + "/contact")]
            HttpRequest req,
            ILogger log)
        {
            var allContacts = ContactRepository
                .Instance()
                .GetAll()
                .Select(x => new Contact
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return new JsonResult(allContacts);
        }

        [FunctionName(nameof(SingleContact))]
        public static IActionResult SingleContact(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = AppConstant.version + "/contact/{contactId}")]
            HttpRequest req,
            ILogger log,
            string contactId)
        {
            var contact = ContactRepository
                .Instance()
                .Get(contactId);

            if (contact == null)
                return new NotFoundResult();

            return new OkObjectResult(contact);
        }

        [FunctionName(nameof(CreateContact))]
        public static IActionResult CreateContact(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = AppConstant.version + "/contact")]
            Contact input,
            ILogger log)
        {
            var contact = new Contact
            {
                Id = input.Id,
                Name = input.Name
            };

            try
            {
                ContactRepository
                    .Instance()
                    .Create(contact);

                return new AcceptedResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName(nameof(UpdateContact))]
        public static IActionResult UpdateContact(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = AppConstant.version + "/contact/{contactId}")]
            Contact input,
            ILogger log,
            string contactId)
        {
            var contact = new Contact
            {
                Id = contactId,
                Name = input.Name
            };

            try
            {
                ContactRepository
                    .Instance()
                    .Update(contactId, contact);
                return new AcceptedResult();
            }
            catch (Exception)
            {
                return new NotFoundResult();
            }
        }
    }
}
