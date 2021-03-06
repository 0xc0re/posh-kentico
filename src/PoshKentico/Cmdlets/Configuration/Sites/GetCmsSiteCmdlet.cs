﻿// <copyright file="GetCmsSiteCmdlet.cs" company="Chris Crutchfield">
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
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using CMS.SiteProvider;
using ImpromptuInterface;
using PoshKentico.Business.Configuration.Sites;
using PoshKentico.Core.Services.Configuration.Sites;
using AliasAttribute = System.Management.Automation.AliasAttribute;

namespace PoshKentico.Cmdlets.Configuration.Sites
{
    /// <summary>
    /// <para type="synopsis">Gets the sites selected by the provided input.</para>
    /// <para type="description">Gets the sites selected by the provided input.  This command automatically initializes the connection to Kentico if not already initialized.</para>
    /// <para type="description"></para>
    /// <para type="description">Without parameters, this command returns all sites.</para>
    /// <para type="description">With parameters, this command returns the sites that match the criteria.</para>
    /// <example>
    ///     <para>Get all the sites.</para>
    ///     <code>Get-CMSSite</code>
    /// </example>
    /// <example>
    ///     <para>Get all sites with a display name "*bas*", site name "*bas*", or a domain name "bas*".</para>
    ///     <code>Get-CMSSite bas</code>
    /// </example>
    /// <example>
    ///     <para>Get all sites with a display name "basic", site name "basic", or domain name "basic"</para>
    ///     <code>Get-CMSSite basic -Exact</code>
    /// </example>
    /// <example>
    ///     <para>Get all the sites with the specified IDs.</para>
    ///     <code>Get-CMSSite -ID 5,304,5</code>
    /// </example>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Cmdlet(VerbsCommon.Get, "CMSSite", DefaultParameterSetName = NONE)]
    [OutputType(typeof(SiteInfo[]))]
    [Alias("gsite")]
    public class GetCmsSiteCmdlet : MefCmdlet
    {
        #region Constants

        private const string NONE = "None";
        private const string DISPLAYNAME = "Dislpay Name";
        private const string IDSETNAME = "ID";

        #endregion
        #region Properties

        /// <summary>
        /// <para type="description">The display name of the site to retrive.</para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ParameterSetName = DISPLAYNAME)]
        [Alias("SiteName", "DomainName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// <para type="description">If set, the match is exact, else the match performs a contains for display name and category name and starts with for path.</para>
        /// </summary>
        [Parameter(ParameterSetName = DISPLAYNAME)]
        public SwitchParameter Exact { get; set; }

        /// <summary>
        /// <para type="description">The IDs of the site to retrieve.</para>
        /// </summary>
        [Parameter(Mandatory = false, Position = 0, ParameterSetName = IDSETNAME)]
        public int[] ID { get; set; }

        /// <summary>
        /// Gets or sets the Business layer for this site. Populated by MEF.
        /// </summary>
        [Import]
        public GetCmsSiteBusiness BusinessLayer { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            IEnumerable<ISite> sites = null;

            switch (this.ParameterSetName)
            {
                case DISPLAYNAME:
                    sites = this.BusinessLayer.GetSites(this.DisplayName, this.Exact.ToBool());
                    break;
                case IDSETNAME:
                    sites = this.BusinessLayer.GetSites(this.ID);
                    break;
                case NONE:
                    sites = this.BusinessLayer.GetSites();
                    break;
            }

            foreach (var site in sites)
            {
                this.WriteObject(site.UndoActLike());
            }
        }

        #endregion
    }
}
