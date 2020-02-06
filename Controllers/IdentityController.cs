using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace foo.api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SignUpAsync()
        {
            string input = "";
            using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.UTF8))
            {
                input = await reader.ReadToEndAsync();
            }

            // Check the input content value
            if (string.IsNullOrEmpty(input))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseContent("Request content is empty", HttpStatusCode.Conflict));
            }

            // Convert the input string into an InputClaimsModel object
            InputClaimsModel inputClaims = JsonConvert.DeserializeObject<InputClaimsModel>(input);

            if (inputClaims == null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseContent("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            // Run an input validation
            if (!IsValidIdNumber(inputClaims.idNumber))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseContent("Invalid ID Number", HttpStatusCode.Conflict));
            }

            // Create an output claims object and set the loyalty number with a random value
            OutputClaimsModel outputClaims = new OutputClaimsModel();
            outputClaims.loyaltyNumber = new Random().Next(100, 1000).ToString();

            // Return the output claim(s)
            return Ok(outputClaims);
        }

        private bool IsValidIdNumber(object idNumber)
        {
            try
            {
                string value = idNumber.ToString();
                if (value.Length == 0) return false;

                var tempDate = new DateTime(Convert.ToInt32(value.Substring(0, 2)), Convert.ToInt32(value.Substring(2, 2)), Convert.ToInt32(value.Substring(4, 2)));
                if (!((tempDate.Year.ToString("0000").Substring(2, 2) == value.Substring(0, 2)) &&
                      (tempDate.Month == Convert.ToInt32(value.Substring(2, 2))) &&
                      (tempDate.Day.ToString("00") == value.Substring(4, 2))))
                    return false;
                else
                {
                    //Apply Luhn Algorithm for check-digits
                    int tempTotal = 0;
                    int checkSum = 0;
                    int multiplier = 1;
                    for (var i = 0; i < 13; ++i)
                    {
                        tempTotal = int.Parse(value[i].ToString()) * multiplier;
                        if (tempTotal > 9)
                            tempTotal = int.Parse(tempTotal.ToString()[0].ToString()) + int.Parse(tempTotal.ToString()[1].ToString());
                        checkSum = checkSum + tempTotal;
                        multiplier = (multiplier % 2 == 0) ? 1 : 2;
                    }
                    if ((checkSum % 10) != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class B2CResponseContent
    {
        public string version { get; set; }
        public int status { get; set; }
        public string userMessage { get; set; }

        public B2CResponseContent(string message, HttpStatusCode status)
        {
            this.userMessage = message;
            this.status = (int)status;
            this.version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public class OutputClaimsModel
    {
        public string loyaltyNumber { get; set; }
    }

    public class InputClaimsModel
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string idNumber { get; set; }
    }
}
