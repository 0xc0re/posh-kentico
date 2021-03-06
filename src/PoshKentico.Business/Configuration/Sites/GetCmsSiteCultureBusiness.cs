﻿// <copyright file="GetCmsSiteCultureBusiness.cs" company="Chris Crutchfield">
// Copyright (C) 2017  Chris Crutchfield
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see &lt;http://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.Composition;
using PoshKentico.Core.Services.Configuration.Localization;
using PoshKentico.Core.Services.Configuration.Sites;

namespace PoshKentico.Business.Configuration.Sites
{
    /// <summary>
    /// Business layer for the Get-CMSSiteCulture cmdlet.
    /// </summary>
    [Export(typeof(GetCmsSiteCultureBusiness))]
    public class GetCmsSiteCultureBusiness : CmdletBusinessBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets a reference to the Site Service.  Populated by MEF.
        /// </summary>
        [Import]
        public ISiteService SiteService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the cultures of the specified site.
        /// </summary>
        /// <param name="site">the site to get culture from.</param>
        /// <returns>Returns the list containing the cultures of the specified site.</returns>
        public IEnumerable<ICulture> GetCultures(ISite site) => this.GetSiteCultures(site);

        /// <summary>
        /// Gets the cultures of the specified site.
        /// </summary>
        /// <param name="siteName">the site name of the site to get culture from.</param>
        /// <returns>Returns the list containing the cultures of the specified site.</returns>
        public IEnumerable<ICulture> GetCultures(string siteName)
        {
            var site = this.SiteService.GetSite(siteName);

            return this.GetSiteCultures(site);
        }

        /// <summary>
        /// Gets the cultures of the specified site.
        /// </summary>
        /// <param name="site">the site to get culture from.</param>
        /// <returns>Returns the list containing the cultures of the specified site.</returns>
        private IEnumerable<ICulture> GetSiteCultures(ISite site)
        {
            return this.SiteService.GetSiteCultures(site);
        }
        #endregion

    }
}
