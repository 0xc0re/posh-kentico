﻿// <copyright file="SetCmsMediaLibraryFileBusiness.cs" company="Chris Crutchfield">
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

using System.ComponentModel.Composition;
using ImpromptuInterface;
using PoshKentico.Core.Services.ContentManagement.MediaLibraries;

namespace PoshKentico.Business.ContentManagement.MediaLibraries
{
    /// <summary>
    /// Business Layer of Set-CMSMediaLibraryFile Cmdlet.
    /// </summary>
    [Export(typeof(SetCmsMediaLibraryFileBusiness))]
    public class SetCmsMediaLibraryFileBusiness : CmdletBusinessBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets a reference to the Media Library Service.  Populated by MEF.
        /// </summary>
        [Import]
        public IMediaLibraryService MediaLibraryService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <see cref="IMediaFile"/> in the CMS System.
        /// </summary>
        /// <param name="file">The <see cref="IMediaFile"/> to set.</param>
        /// <returns>The updated File.</returns>
        public IMediaFile Set(IMediaFile file)
        {
            return this.MediaLibraryService.UpdateMediaFile(file);
        }

        #endregion
    }
}
