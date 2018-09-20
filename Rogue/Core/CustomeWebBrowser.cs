using CefSharp;
using CefSharp.WinForms;
using Rogue.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rogue.Core
{
    /// <summary>
    /// 自定义 WebBrowser
    /// </summary>
    public class CustomeWebBrowser : ChromiumWebBrowser
    {
        public CustomeWebBrowser(IRequestContext requestContext = null) : base("", requestContext)
        {
            var requestHandler = new CustomeRequestHandler(this);
            requestHandler.InterceptBeforeResourceLoad += RequestHandler_InterceptBeforeResourceLoad;
            requestHandler.InterceptResourceLoadComplete += RequestHandler_InterceptResourceLoadComplete;
            this.FrameLoadEnd += CustomeWebBrowser_FrameLoadEnd_SyncLoad;
            this.LoadError += CustomeWebBrowser_LoadError_SyncLoad;
            this.RequestHandler = requestHandler;
        }

        #region 私有成员

        /// <summary>
        /// 在资源加载前，过滤或写入缓存队列
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <returns></returns>
        private CefReturnValue RequestHandler_InterceptBeforeResourceLoad(IWebBrowser arg1, IBrowser arg2, IFrame arg3, IRequest arg4, IRequestCallback arg5)
        {
            if (OnBeforeResourceLoad == null)
            {
                return CefReturnValue.Continue;
            }
            var args = new BeforeResourceLoadEventArgs(arg4.Url, arg4.ResourceType);
            OnBeforeResourceLoad.Invoke(this, args);
            if (args.CacheToQueue)
            {
                ResourceCacheManager.CreateResource(arg4.Identifier);
            }
            return !args.Cancel ? CefReturnValue.Continue : CefReturnValue.Cancel;
        }

        /// <summary>
        /// 在资源加载完成后，触发完成事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="arg6"></param>
        /// <param name="arg7"></param>
        private void RequestHandler_InterceptResourceLoadComplete(IWebBrowser arg1, IBrowser arg2, IFrame arg3, IRequest arg4, IResponse arg5, UrlRequestStatus arg6, long arg7)
        {
            var stream = ResourceCacheManager.GetResource(arg4.Identifier);
            if (stream != null)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }
            try
            {
                this.OnResourceLoadComplete?.Invoke(this, new ResourceLoadCompleteEventArgs(arg4.Url, arg4.ResourceType, arg7, stream, arg6));
            }
            finally
            {
                ResourceCacheManager.DeleteResource(arg4.Identifier);
            }
        }

        #endregion

        #region 同步加载

        /// <summary>
        /// 用于同步加载页面的信号量
        /// </summary>
        private SemaphoreSlim _SyncSemaphore = new SemaphoreSlim(1);
        private bool _SyncLoading = false;
        private string _SyncUrl = string.Empty;
        /// <summary>
        /// 同步加载某个地址，直到超时时间已到
        /// </summary>
        /// <param name="url"></param>
        public void LoadSync(string url, TimeSpan timeout)
        {
            if (this._SyncLoading)
            {
                return;
            }
            lock (this)
            {
                if (this._SyncLoading)
                {
                    return;
                }
                this._SyncLoading = true;
            }
            this._SyncSemaphore = new SemaphoreSlim(0);
            this._SyncUrl = url;
            this.Load(url);
            this._SyncSemaphore.Wait((int)timeout.TotalMilliseconds);
            lock (this)
            {
                this._SyncLoading = false;
                this._SyncUrl = string.Empty;
                this._SyncSemaphore.Dispose();
                this._SyncSemaphore = null;
            }
        }

        private void CustomeWebBrowser_LoadError_SyncLoad(object sender, LoadErrorEventArgs e)
        {
            lock (this)
            {
                if (!this._SyncLoading
                    || string.IsNullOrEmpty(this._SyncUrl)
                    || this._SyncSemaphore == null
                    || !e.Frame.Url.Equals(this._SyncUrl))
                {
                    return;
                }
            }
            try
            {
                this._SyncSemaphore.Release();
            }
            catch { }

        }

        private void CustomeWebBrowser_FrameLoadEnd_SyncLoad(object sender, FrameLoadEndEventArgs e)
        {
            lock (this)
            {
                if (!this._SyncLoading
                    || string.IsNullOrEmpty(this._SyncUrl)
                    || this._SyncSemaphore == null
                    || !e.Url.Equals(this._SyncUrl))
                {
                    return;
                }
            }
            try
            {
                this._SyncSemaphore.Release();
            }
            catch { }
        }

        #endregion

        #region 扩展方法



        #endregion

        #region 扩展事件

        /// <summary>
        /// 在开始加载资源前触发
        /// </summary>
        public event EventHandler<BeforeResourceLoadEventArgs> OnBeforeResourceLoad;

        /// <summary>
        /// 在资源加载完成后触发
        /// </summary>
        public event EventHandler<ResourceLoadCompleteEventArgs> OnResourceLoadComplete;

        #endregion

        #region 扩展属性

        

        #endregion
    }
}
