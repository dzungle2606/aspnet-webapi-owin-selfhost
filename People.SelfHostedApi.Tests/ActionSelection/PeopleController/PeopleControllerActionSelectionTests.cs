﻿namespace People.SelfHostedApi.Tests.ActionSelection.PeopleController
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;
    using Common;
    using NUnit.Framework;
    using SelfHostedApi.Controllers;
    using static NUnit.Framework.Assert;

    [TestFixture]
    public class PeopleControllerActionSelectionTests
    {
        [Test]
        [TestCase("http://localhost:3001/api/people/", "GET", typeof(PeopleController), "Get")]
        [TestCase("http://localhost:3001/api/people/1", "GET", typeof(PeopleController), "Get")]
        [TestCase("http://localhost:3001/api/people/1", "PUT", typeof(PeopleController), "Put")]
        [TestCase("http://localhost:3001/api/people/1", "DELETE", typeof(PeopleController), "Delete")]
        [TestCase("http://localhost:3001/api/people/", "POST", typeof(PeopleController), "Post")]
        public void CorrectControllerAndActionAreSelected_Test(string url, string method, Type controller, string action)
        {
            // Arrange
            IHttpRouteData routeData;
            var config = new HttpConfiguration();
            WebApiRouteConfig.Register(config);
            config.EnsureInitialized();
            var actionSelector = config.Services.GetActionSelector();
            var controllerSelector = config.Services.GetHttpControllerSelector();
            var request = CommonMethods.SetupRequest(url, method, config, out routeData);

            // Act
            var controllerDescriptor = controllerSelector.SelectController(request);
            var context = new HttpControllerContext(config, routeData, request)
            {
                ControllerDescriptor = controllerDescriptor
            };
            var actionDescriptor = actionSelector.SelectAction(context);

            // Assert
            AreEqual(controller, controllerDescriptor.ControllerType);
            AreEqual(action, actionDescriptor.ActionName);
        }
    }
}