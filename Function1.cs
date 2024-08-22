using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var dataSet = new MockData
            {
                CaseNumber = "NPE-2024-00240",
                Name = "Ola Nordmann",
                TreatmentCenter = "Oslo Universitetssykehus",
                Questions = new List<Question>
                {
                    new Question
                    {
                        QuestionText = "Hva er ditt navn?",
                        Answer = "Ola Nordmann"
                    },
                    new Question
                    {
                        QuestionText = "Hva er din alder?",
                        Answer = "45"
                    },
                    new Question
                    {
                        QuestionText = "Hva er din adresse?",
                        Answer = "Osloveien 1, 0010 Oslo"
                    }
                }
            };

            response.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dataSet)));
            return response;
        }
    }
    public class FormData
    {
        [JsonPropertyName("formUserName")]
        public string ApplicantName { get; set; }
    }

    public class MockData
    {
        public string CaseNumber { get; set; }
        public string Name { get; set; }
        public string TreatmentCenter { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
    }
}
