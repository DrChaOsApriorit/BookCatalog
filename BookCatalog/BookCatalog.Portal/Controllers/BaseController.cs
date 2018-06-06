﻿using BookCatalog.Common.Bootstrap;
using BookCatalog.Common.Request;
using BookCatalog.Portal.Context;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookCatalog.Portal.Controllers
{
    public class BaseController : Controller
    {
        private readonly object mutex = new object();

        private DefaultContext context = default(DefaultContext);

        private IServiceProviderFactory _modelFactory = default(IServiceProviderFactory);

        public IRequestContext RequestContext
        {
            get
            {
                if (this.context == null)
                {
                    this.context = new DefaultContext();
                }

                return this.context;
            }
        }

        protected IServiceProviderFactory Factory
        {
            get
            {
                if (this._modelFactory == null)
                {
                    this._modelFactory = this.RequestContext.Factory;
                }

                return this._modelFactory;
            }
        }

        protected JsonResult Success(object model = null)
        {
            return new JsonResult()
            {
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = model,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }
    }
}