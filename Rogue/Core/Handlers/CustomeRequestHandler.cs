using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Core.Handlers
{
    public class CustomeRequestHandler : IRequestHandler
    {
        public CustomeWebBrowser WebBrowser { get; private set; }
        public CustomeRequestHandler(CustomeWebBrowser webBrowser)
        {
            this.WebBrowser = webBrowser;
        }

        public bool CanGetCookies(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return true;
        }

        public bool CanSetCookie(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
        {
            return true;
        }

        public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return true;
        }

        /// <summary>
        /// 在CEF IO线程上调用以选择性地过滤资源响应内容
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var stream = ResourceCacheManager.GetResource(request.Identifier);
            if (stream != null)
            {
                return new CustomResponseFilter(stream);
            }
            return null;
        }

        /// <summary>
        /// 在导航前触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="userGesture"></param>
        /// <param name="isRedirect"></param>
        /// <returns>返回(false:允许导航,true:结束导航)</returns>
        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        /// <summary>
        /// 在某个资源加载前触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            if (this.InterceptBeforeResourceLoad != null)
            {
                return this.InterceptBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
            }
            return CefReturnValue.Continue;
        }

        /// <summary>
        /// 在某个资源加载后触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="status"></param>
        /// <param name="receivedContentLength"></param>
        public void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            if (this.InterceptResourceLoadComplete != null)
            {
                this.InterceptResourceLoadComplete(chromiumWebBrowser, browser, frame, request, response, status, receivedContentLength);
            }
        }

        /// <summary>
        /// 在收到资源响应时触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns>返回(false:允许加载)</returns>
        public bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var uri = request.Url;

            return false;
        }

        /// <summary>
        /// 证书无效时触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="errorCode"></param>
        /// <param name="requestUrl"></param>
        /// <param name="sslInfo"></param>
        /// <param name="callback"></param>
        /// <returns>返回(false:取消执行,true:继续执行)</returns>
        public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return true;
        }

        /// <summary>
        /// 从tab页或其他浏览器打开链接
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetDisposition"></param>
        /// <param name="userGesture"></param>
        /// <returns>返回(false:继续导航,true:取消导航)</returns>
        public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        /// <summary>
        /// 在某个插件崩溃时触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="pluginPath"></param>
        public void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {

        }

        /// <summary>
        /// 在某个协议错误时触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="url"></param>
        /// <returns>返回(true:尝试执行,false:取消执行)</returns>
        public bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, string url)
        {
            return true;
        }

        /// <summary>
        /// 在js请求分配空间时触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="originUrl"></param>
        /// <param name="newSize"></param>
        /// <param name="callback"></param>
        /// <returns>返回(false:取消分配,true:继续分配)</returns>
        public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            return true;
        }

        public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {

        }

        public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }



        public void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {

        }

        /// <summary>
        /// 当需要选择用户证书时触发
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="isProxy"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="certificates"></param>
        /// <param name="callback"></param>
        /// <returns>返回(true:继续选择)</returns>
        public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            return true;
        }


        /// <summary>
        /// 在开始加载资源前触发
        /// </summary>
        internal event Func<IWebBrowser, IBrowser, IFrame, IRequest, IRequestCallback, CefReturnValue> InterceptBeforeResourceLoad;

        /// <summary>
        /// 在资源加载完成后触发
        /// </summary>
        internal event Action<IWebBrowser, IBrowser, IFrame, IRequest, IResponse, UrlRequestStatus, long> InterceptResourceLoadComplete;
    }
}
