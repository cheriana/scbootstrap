namespace ScBootstrap.Tests.Helpers
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;
    using System.Security.Principal;

    /// <summary>Adapted from http://www.hanselman.com/blog/ASPNETMVCSessionAtMix08TDDAndMvcMockHelpers.aspx </summary>
    public static class MockHelpers
    {
        public class MockIdentity : IIdentity
        {
            public string AuthenticationType
            {
                get { return "MockAuthentication"; }
            }

            public bool IsAuthenticated
            {
                get { return true; }
            }

            public string Name
            {
                get { return "someUser"; }
            }
        }

        public class MockPrincipal : IPrincipal
        {
            IIdentity identity;

            public IIdentity Identity
            {
                get { return identity ?? (identity = new MockIdentity()); }
            }

            public bool IsInRole(string role)
            {
                return false;
            }
        }
        public static HttpContextBase FakeHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new MockPrincipal();

            context.SetupGet(ctx => ctx.Request).Returns(request.Object);
            context.SetupGet(ctx => ctx.Response).Returns(response.Object);
            context.SetupGet(ctx => ctx.Session).Returns(session.Object);
            context.SetupGet(ctx => ctx.Server).Returns(server.Object);
            context.SetupGet(ctx => ctx.User).Returns(user);

            return context.Object;
        }

        public static HttpContextBase FakeHttpContext(string url)
        {
            var context = FakeHttpContext();
            context.Request.SetupRequestUrl(url);
            return context;
        }

        public static void SetFakeControllerContext(this Controller controller)
        {
            var httpContext = FakeHttpContext();
            var context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
            controller.ControllerContext = context;
        }

        static string GetUrlFileName(string url)
        {
            return url.Contains("?") ? url.Substring(0, url.IndexOf("?", StringComparison.Ordinal)) : url;
        }

        static NameValueCollection GetQueryStringParameters(string url)
        {
            var parameters = new NameValueCollection();
            if (!url.Contains("?")) { return parameters; }
            var parts = url.Split("?".ToCharArray());
            var keys = parts[1].Split("&".ToCharArray());
            keys.Select(key => key.Split("=".ToCharArray())).ToList().ForEach(part => parameters.Add(part[0], part[1]));
            return parameters;
        }

        public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
        {
            Mock.Get(request)
                .Setup(req => req.HttpMethod)
                .Returns(httpMethod);
        }

        public static void SetupRequestUrl(this HttpRequestBase request, string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            if (!url.StartsWith("~/"))
                throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

            var mock = Mock.Get(request);

            mock.Setup(req => req.QueryString)
                .Returns(GetQueryStringParameters(url));
            mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
                .Returns(GetUrlFileName(url));
            mock.Setup(req => req.PathInfo)
                .Returns(string.Empty);
        }
    }
}
