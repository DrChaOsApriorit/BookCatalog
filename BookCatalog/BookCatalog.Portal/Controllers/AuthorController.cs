﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookCatalog.Portal.Controllers
{
    public class AuthorController : BaseController
    {
        public ActionResult Index()
        {
            return View("AuthorList");
        }
    }
}