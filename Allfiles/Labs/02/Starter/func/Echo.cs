// using System;
// using System.IO;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;

// namespace func
// {
//     public static class Echo
//     {
//         [FunctionName("Echo")]
//         public static async Task<IActionResult> Run(
//             [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
//             ILogger log)
//         {
//             log.LogInformation("C# HTTP trigger function processed a request.");

//             string name = req.Query["name"];

//             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//             dynamic data = JsonConvert.DeserializeObject(requestBody);
//             name = name ?? data?.name;

//             string responseMessage = string.IsNullOrEmpty(name)
//                 ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
//                 : $"Hello, {name}. This HTTP triggered function executed successfully.";

//             return new OkObjectResult(responseMessage);
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
public static class Echo
{
 [FunctionName("Echo")]
 public static IActionResult Run(
     [HttpTrigger("POST")] HttpRequest request,
     ILogger logger)
 {
  logger.LogInformation("Received a request");
  return new OkObjectResult(request.Body);
 }
}