using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace FunctionAPPAzureSQL
{
    public static class GetProduct
    {
        public static int ProductId { get; private set; }
        public static string ProductName { get; private set; }
        public static int Quantity { get; private set; }

        [FunctionName("GetProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);

              List<Product> list = new List<Product>();
            //List<string> list = new List<string>();

            SqlConnection conn = Getconnection();
            conn.Open();

            string query = "Select * from Products";
            SqlCommand cmd = new SqlCommand(query, conn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    //list.Add(reader.GetString(0));
                    Product product = new Product();
                    {
                        ProductId = reader.GetInt32(0);
                        ProductName = reader.GetString(1);
                        Quantity = reader.GetInt32(2);

                    };
                    list.Add(product);
                }
            }

            conn.Close();

            return new OkObjectResult(list);
        }

        private static SqlConnection Getconnection()
        {
            string connectionString = "Server=tcp:appdbsagar.database.windows.net,1433;Initial Catalog=appdb;Persist Security Info=False;User ID=sqladmin;Password=KeyaBirla@15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return new SqlConnection(connectionString);

        }
    }
}
