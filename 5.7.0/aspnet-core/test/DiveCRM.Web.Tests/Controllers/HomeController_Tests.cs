using System.Threading.Tasks;
using DiveCRM.Models.TokenAuth;
using DiveCRM.Web.Controllers;
using Shouldly;
using Xunit;

namespace DiveCRM.Web.Tests.Controllers
{
    public class HomeController_Tests: DiveCRMWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}