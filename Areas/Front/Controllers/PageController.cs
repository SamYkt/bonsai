﻿using System.Threading.Tasks;
using Bonsai.Areas.Front.Logic;
using Bonsai.Areas.Front.Logic.Auth;
using Bonsai.Code.Infrastructure;
using Bonsai.Code.Services;
using Bonsai.Code.Utils;
using Bonsai.Code.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bonsai.Areas.Front.Controllers
{
    /// <summary>
    /// The root controller for pages.
    /// </summary>
    [Area("Front")]
    [Route("p")]
    [Authorize(Policy = AuthRequirement.Name)]
    public class PageController: AppControllerBase
    {
        public PageController(PagePresenterService pages, AuthService auth, CacheService cache)
        {
            _pages = pages;
            _auth = auth;
            _cache = cache;
        }

        private readonly PagePresenterService _pages;
        private readonly AuthService _auth;
        private readonly CacheService _cache;

        /// <summary>
        /// Displays the page description.
        /// </summary>
        [Route("{key}")]
        public async Task<ActionResult> Description(string key)
        {
            var encKey = PageHelper.EncodeTitle(key);
            if (encKey != key)
                return RedirectToActionPermanent("Description", new {key = encKey});

            try
            {
                ViewBag.User = await _auth.GetCurrentUserAsync(User);
                var vm = await _cache.GetOrAddAsync(key, async () => await _pages.GetPageDescriptionAsync(key));
                return View(vm);
            }
            catch (RedirectRequiredException ex)
            {
                return RedirectToActionPermanent("Description", new {key = ex.Key});
            }
        }


        /// <summary>
        /// Displays the related media files.
        /// </summary>
        [Route("{key}/media")]
        public async Task<ActionResult> Media(string key)
        {
            var encKey = PageHelper.EncodeTitle(key);
            if (encKey != key)
                return RedirectToActionPermanent("Media", new { key = encKey });

            try
            {
                var vm = await _cache.GetOrAddAsync(key, async () => await _pages.GetPageMediaAsync(key));
                return View(vm);
            }
            catch(RedirectRequiredException ex)
            {
                return RedirectToActionPermanent("Description", new { key = ex.Key });
            }
        }
    }
}
