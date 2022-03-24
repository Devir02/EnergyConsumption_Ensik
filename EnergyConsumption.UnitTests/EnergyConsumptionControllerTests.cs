

using EnergyConsumption.Repository.Interfaces;
using System.Net.Http;
using Xunit;

namespace EnergyConsumption.UnitTests
{
 
    public class EnergyConsumptionControllerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        public EnergyConsumptionControllerTests(IUnitOfWork work)
        {
            _unitOfWork = work;
        }
        [Theory]
        [InlineData("POST", "")]
        public void LibrarianUploadFile_Error(string method,string? data = null)
        {

            //arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/meter-reading-uploads");

            //act
            var response =  _client.SendAsync(request);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
