﻿// <copyright file="GetCMSWebPartCategoryBusiness.cs" company="Chris Crutchfield">
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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using PoshKentico.Core.Services.Development.WebParts;

namespace PoshKentico.Business.Development.WebParts
{
    /// <summary>
    /// Business layer for the Get-CMSWebPartCategory cmdlet.
    /// </summary>
    [Export(typeof(GetCMSWebPartCategoryBusiness))]
    public class GetCMSWebPartCategoryBusiness : WebPartBusinessBase
    {
        #region Methods

        /// <summary>
        /// Gets a list of all of the <see cref="IWebPartCategory"/>.
        /// </summary>
        /// <returns>A list of all of the <see cref="IWebPartCategory"/>.</returns>
        public IEnumerable<IWebPartCategory> GetWebPartCategories() => this.WebPartService.WebPartCategories;

        /// <summary>
        /// Gets a list of all of the <see cref="IWebPartCategory"/> which match the specified criteria.
        /// </summary>
        /// <param name="matchString">The string which to match the webpart categories to.</param>
        /// <param name="isRegex">Indicates whether <paramref name="matchString"/> is a regular expression.</param>
        /// <param name="recurse">Indicates whether webpart categories should be returned recursively.</param>
        /// <returns>A list of all of the <see cref="IWebPartCategory"/> which match the specified criteria.</returns>
        public virtual IEnumerable<IWebPartCategory> GetWebPartCategories(string matchString, bool isRegex, bool recurse)
        {
            Regex regex = null;

            if (isRegex)
            {
                regex = new Regex(matchString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            else
            {
                regex = new Regex($"^{matchString.Replace("*", ".*")}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            var matched = from c in this.WebPartService.WebPartCategories
                          where regex.IsMatch(c.CategoryName) ||
                              regex.IsMatch(c.CategoryDisplayName)
                          select c;

            if (recurse)
            {
                return this.GetRecurseWebPartCategories(matched);
            }
            else
            {
                return matched.ToArray();
            }
        }

        /// <summary>
        /// Gets a list of the <see cref="IWebPartCategory"/> which match the supplied IDs.
        /// </summary>
        /// <param name="ids">The IDs of the <see cref="IWebPartCategory"/> to return.</param>
        /// <param name="recurse">Indicates whether webpart categories should be returned recursively.</param>
        /// <returns>A list of the <see cref="IWebPartCategory"/> which match the supplied IDs.</returns>
        public IEnumerable<IWebPartCategory> GetWebPartCategories(int[] ids, bool recurse)
        {
            var webPartCategories = from id in ids
                                    select this.WebPartService.GetWebPartCategory(id);

            var nonNullCategories = from wpc in webPartCategories
                                    where wpc != null
                                    select wpc;

            if (recurse)
            {
                return this.GetRecurseWebPartCategories(nonNullCategories);
            }
            else
            {
                return nonNullCategories.ToArray();
            }
        }

        /// <summary>
        /// Gets a list of the <see cref="IWebPartCategory"/> which are children of the <paramref name="parentWebPartCategory"/>.
        /// </summary>
        /// <param name="parentWebPartCategory">The <see cref="IWebPartCategory"/> which is parent to the categories to find.</param>
        /// <param name="recurse">Indicates whether webpart categories should be returned recursively.</param>
        /// <returns>A list of the <see cref="IWebPartCategory"/> which are children to the supplied <paramref name="parentWebPartCategory"/>.</returns>
        public IEnumerable<IWebPartCategory> GetWebPartCategories(IWebPartCategory parentWebPartCategory, bool recurse)
        {
            var categories = this.WebPartService.GetWebPartCategories(parentWebPartCategory);

            if (recurse)
            {
                return this.GetRecurseWebPartCategories(categories);
            }
            else
            {
                return categories;
            }
        }

        /// <summary>
        /// Gets a list of web part categories by path.
        /// </summary>
        /// <param name="path">The path to get the list of web part categories.</param>
        /// <param name="recurse">Indicates if the web part category children should be returned as well.</param>
        /// <returns>A list of all of the web part categories found at the specified path.</returns>
        public IEnumerable<IWebPartCategory> GetWebPartCategories(string path, bool recurse)
        {
            var categories = from c in this.WebPartService.WebPartCategories
                             where c.CategoryPath.Equals(path, StringComparison.InvariantCultureIgnoreCase)
                             select c;

            if (recurse)
            {
                return this.GetRecurseWebPartCategories(categories);
            }
            else
            {
                return categories.ToArray();
            }
        }

        /// <summary>
        /// Gets the web part category for the current web part.
        /// </summary>
        /// <param name="webpart">The webpart to get the category for.</param>
        /// <returns>The web part category.</returns>
        public IWebPartCategory GetWebPartCategory(IWebPart webpart) =>
            (from c in this.WebPartService.WebPartCategories
             where c.CategoryID == webpart.WebPartCategoryID
             select c).SingleOrDefault();

        private IEnumerable<IWebPartCategory> GetRecurseWebPartCategories(IEnumerable<IWebPartCategory> webPartCategories)
        {
            return webPartCategories
                .Select(wp => this.GetWebPartCategories(wp, true))
                .SelectMany(c => c)
                .Concat(webPartCategories)
                .ToArray();
        }

        #endregion

    }
}
