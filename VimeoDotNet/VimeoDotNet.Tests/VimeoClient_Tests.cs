using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using VimeoDotNet.Authorization;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace VimeoDotNet.Tests
{
    [TestClass]
    public class VimeoClient_Tests
    {
        private const string AccessToken = "access-token";

        Dictionary<string, string> urlSegments;
        Mock<IAuthorizationClientFactory> authorizationClientFactory;
        Mock<IApiRequest> apiRequest;
        Mock<IApiRequestFactory> apiRequestFactory;
        VimeoClientFactory factory;
        IVimeoClient client;

        [TestMethod]
        public void VimeoClient_Constructor()
        {
        }

        [TestInitialize]
        public void VimeoClient_Initialize()
        {
            urlSegments = new Dictionary<string, string>();
            authorizationClientFactory = new Mock<IAuthorizationClientFactory>();
            apiRequest = new Mock<IApiRequest>();
            apiRequest.Setup(x => x.UrlSegments).Returns(urlSegments);
            apiRequestFactory = new Mock<IApiRequestFactory>();
            apiRequestFactory.Setup(x => x.GetApiRequest(AccessToken)).Returns(apiRequest.Object);
            factory = new VimeoClientFactory(authorizationClientFactory.Object, apiRequestFactory.Object);
            client = factory.GetVimeoClient(AccessToken);
        }

        [TestMethod]
        public void VimeoClient_GetAccountAlbumVideos_CreatesCorrectRequestPath()
        {
            var restResponse = new Mock<IRestResponse<Paginated<Video>>>();
            restResponse.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
            apiRequest.Setup(x => x.ExecuteRequestAsync<Paginated<Video>>()).ReturnsAsync(restResponse.Object);

            const long albumId = 1;
            client.GetAccountAlbumVideos(albumId);

            apiRequest.VerifySet(x => x.Path = "/me/albums/{albumId}/videos");
            Assert.IsTrue(urlSegments.ContainsKey("albumId"), "Missing albumId key");
            Assert.IsTrue(urlSegments.ContainsValue("1"), "Missing albumId value");
        }

        [TestMethod]
        public void VimeoClient_GetAccountAlbumVideo_CreatesCorrectRequestPath()
        {
            var restResponse = new Mock<IRestResponse<Video>>();
            restResponse.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
            apiRequest.Setup(x => x.ExecuteRequestAsync<Video>()).ReturnsAsync(restResponse.Object);

            const long albumId = 1;
            const long clipId = 2;
            client.GetAccountAlbumVideo(albumId, clipId);

            apiRequest.VerifySet(x => x.Path = "/me/albums/{albumId}/videos/{clipId}");
            Assert.IsTrue(urlSegments.ContainsKey("albumId"), "Missing albumId key");
            Assert.IsTrue(urlSegments.ContainsValue("1"), "Missing albumId value");
            Assert.IsTrue(urlSegments.ContainsKey("clipId"), "Missing clipId key");
            Assert.IsTrue(urlSegments.ContainsValue("1"), "Missing clipId value");
        }

        [TestMethod]
        public void VimeoClient_GetUserAlbumVideos_CreatesCorrectRequestPath()
        {
            var restResponse = new Mock<IRestResponse<Paginated<Video>>>();
            restResponse.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
            apiRequest.Setup(x => x.ExecuteRequestAsync<Paginated<Video>>()).ReturnsAsync(restResponse.Object);

            const long userId = 1;
            const long albumId = 2;
            client.GetUserAlbumVideos(userId, albumId);

            apiRequest.VerifySet(x => x.Path = "/users/{userId}/albums/{albumId}/videos");
            Assert.IsTrue(urlSegments.ContainsKey("userId"), "Missing userId key");
            Assert.IsTrue(urlSegments.ContainsValue("1"), "Missing userId value");
            Assert.IsTrue(urlSegments.ContainsKey("albumId"), "Missing albumId key");
            Assert.IsTrue(urlSegments.ContainsValue("2"), "Missing albumId value");
        }

        [TestMethod]
        public void VimeoClient_GetUserAlbumVideo_CreatesCorrectRequestPath()
        {
            var restResponse = new Mock<IRestResponse<Video>>();
            restResponse.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
            apiRequest.Setup(x => x.ExecuteRequestAsync<Video>()).ReturnsAsync(restResponse.Object);

            const long userId = 1;
            const long albumId = 2;
            const long clipId = 3;
            client.GetUserAlbumVideo(userId, albumId, clipId);

            apiRequest.VerifySet(x => x.Path = "/users/{userId}/albums/{albumId}/videos/{clipId}");
            Assert.IsTrue(urlSegments.ContainsKey("userId"), "Missing userId key");
            Assert.IsTrue(urlSegments.ContainsValue("1"), "Missing userId value");
            Assert.IsTrue(urlSegments.ContainsKey("albumId"), "Missing albumId key");
            Assert.IsTrue(urlSegments.ContainsValue("2"), "Missing albumId value");
            Assert.IsTrue(urlSegments.ContainsKey("clipId"), "Missing clipId key");
            Assert.IsTrue(urlSegments.ContainsValue("3"), "Missing clipId value");
        }
    }
}