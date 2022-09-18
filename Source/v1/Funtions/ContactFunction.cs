using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using TingTango.Source.v1.Models;
using TingTango.Source.v1.Repositories;

namespace TingTango.Source.v1.Functions;

public static class ContactFunction
{
    [FunctionName(nameof(GetContactList))]
    public static IActionResult GetContactList(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = AppConstant.version + "/contact")]
        HttpRequest req,
        ILogger log)
    {
        try
        {
            var allContacts = ContactRepository
                .Instance()
                .GetAll();
            return new JsonResult(allContacts);
        }
        catch (Exception ex)
        {
            return new JsonResult(ex.Message);
        }
    }

    [FunctionName(nameof(SearchContact))]
    public static IActionResult SearchContact(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = AppConstant.version + "/contact/{contactName}")]
        HttpRequest req,
        ILogger log,
        string contactName)
    {
        try
        {
            var contacts = ContactRepository
                .Instance()
                .GetAlike(contactName);
            if (contacts.Count == 0)
                return new JsonResult("No match found.");

            return new JsonResult(contacts);
        }
        catch (Exception ex)
        {
            return new JsonResult(ex.Message);
        }
    }

    [FunctionName(nameof(CreateContact))]
    public static IActionResult CreateContact(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = AppConstant.version + "/contact")]
        Contact input,
        ILogger log)
    {
        try
        {
            ContactRepository
                .Instance()
                .Create(input);

            return new OkObjectResult(ApplicationMessages.CreateSuccess);
        }
        catch (Exception ex)
        {
            return new JsonResult(ex.Message);
        }
    }

    [FunctionName(nameof(UpdateContact))]
    public static IActionResult UpdateContact(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = AppConstant.version + "/contact/{contactName}")]
        Contact input,
        ILogger log,
        string contactName)
    {
        try
        {
            ContactRepository
                .Instance()
                .Update(contactName, input);
            return new OkObjectResult(ApplicationMessages.UpdateSuccess);
        }
        catch (Exception ex)
        {
            return new JsonResult(ex.Message);
        }
    }

    [FunctionName(nameof(DeleteContact))]
    public static IActionResult DeleteContact(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = AppConstant.version + "/contact/{contactName}")]
        Contact input,
        ILogger log,
        string contactName)
    {
        try
        {
            ContactRepository
                .Instance()
                .Delete(contactName);
            return new OkObjectResult(ApplicationMessages.DeleteSuccess);
        }
        catch (Exception ex)
        {
            return new JsonResult(ex.Message);
        }
    }
}