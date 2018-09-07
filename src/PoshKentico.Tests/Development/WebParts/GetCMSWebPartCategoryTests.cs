﻿using FluentAssertions;
using Moq;
using NUnit.Framework;
using PoshKentico.Business.Development.WebParts;
using PoshKentico.Core.Services.Development.WebParts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshKentico.Tests.Development.WebParts
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class GetCMSWebPartCategoryTests
    {
        [TestCase]
        public void ShouldGetWebPartCategories()
        {
            // Setup web part category
            var webPartCategoryMock = new Mock<IWebPartCategory>();
            var webPartCategoryObj = webPartCategoryMock.Object;

            // Setup web part service mock
            var webPartServiceMock = new Mock<IWebPartService>();
            webPartServiceMock
                .Setup(x => x.WebPartCategories)
                .Returns(new IWebPartCategory[]
                {
                    webPartCategoryObj,
                });

            var businessLayer = new GetCMSWebPartCategoryBusiness
            {
                WebPartService = webPartServiceMock.Object,
                WriteDebug = Assert.NotNull,
                WriteVerbose = Assert.NotNull,
            };

            var results = businessLayer.GetWebPartCategories();

            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Single()
                .ShouldBeEquivalentTo(webPartCategoryObj);
        }

        [TestCase]
        public void ShouldGetWebPartCategoriesFromMatchStringWithoutRegexAndWithoutRecurse()
        {
            // Setup web part category
            var webPartCategoryMock1 = new Mock<IWebPartCategory>();
            webPartCategoryMock1
                .Setup(x => x.CategoryDisplayName)
                .Returns("Display Name");
            webPartCategoryMock1
                .Setup(x => x.CategoryName)
                .Returns("Category");
            var webPartCategoryMock2 = new Mock<IWebPartCategory>();
            webPartCategoryMock2
                .Setup(x => x.CategoryDisplayName)
                .Returns("Different");
            webPartCategoryMock2
                .Setup(x => x.CategoryName)
                .Returns("Different");

            // Setup web part service mock
            var webPartServiceMock = new Mock<IWebPartService>();
            webPartServiceMock
                .Setup(x => x.WebPartCategories)
                .Returns(new IWebPartCategory[]
                {
                    webPartCategoryMock1.Object,
                    webPartCategoryMock2.Object,
                });

            var businessLayer = new GetCMSWebPartCategoryBusiness
            {
                WebPartService = webPartServiceMock.Object,
                WriteDebug = Assert.NotNull,
                WriteVerbose = Assert.NotNull,
            };

            // Test Display Name
            var results = businessLayer.GetWebPartCategories("*spla*", false, false);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Single()
                .ShouldBeEquivalentTo(webPartCategoryMock1.Object);

            results = businessLayer.GetWebPartCategories("spla*", false, false);
            results
                .Should()
                .BeEmpty();

            // Test Name
            results = businessLayer.GetWebPartCategories("*cate*", false, false);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Single()
                .ShouldBeEquivalentTo(webPartCategoryMock1.Object);

            results = businessLayer.GetWebPartCategories("*cate", false, false);
            results
                .Should()
                .BeEmpty();
        }

        [TestCase]
        public void ShouldGetWebPartCategoriesFromMatchStringWithoutRegexAndWithRecurse()
        {
            // Setup web part category
            var webPartCategoryMock1 = new Mock<IWebPartCategory>();
            webPartCategoryMock1
                .Setup(x => x.CategoryDisplayName)
                .Returns("Display Name");
            webPartCategoryMock1
                .Setup(x => x.CategoryName)
                .Returns("Category");
            var webPartCategoryMock2 = new Mock<IWebPartCategory>();
            webPartCategoryMock2
                .Setup(x => x.CategoryDisplayName)
                .Returns("Different");
            webPartCategoryMock2
                .Setup(x => x.CategoryName)
                .Returns("Different");

            // Setup web part service mock
            var webPartServiceMock = new Mock<IWebPartService>();
            webPartServiceMock
                .Setup(x => x.WebPartCategories)
                .Returns(new IWebPartCategory[]
                {
                    webPartCategoryMock1.Object,
                    webPartCategoryMock2.Object,
                });
            webPartServiceMock
                .Setup(x => x.GetWebPartCategories(webPartCategoryMock1.Object))
                .Returns(new IWebPartCategory[] { webPartCategoryMock2.Object });

            var businessLayer = new GetCMSWebPartCategoryBusiness
            {
                WebPartService = webPartServiceMock.Object,
                WriteDebug = Assert.NotNull,
                WriteVerbose = Assert.NotNull,
            };

            // Test Display Name
            var results = businessLayer.GetWebPartCategories("*spla*", false, true);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Count()
                .Should()
                .Be(2);

            results
                .Should()
                .Contain(webPartCategoryMock1.Object);

            results
                .Should()
                .Contain(webPartCategoryMock2.Object);

            results = businessLayer.GetWebPartCategories("spla*", false, true);
            results
                .Should()
                .BeEmpty();

            // Test Name
            results = businessLayer.GetWebPartCategories("*cate*", false, true);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Count()
                .Should()
                .Be(2);

            results
                .Should()
                .Contain(webPartCategoryMock1.Object);

            results
                .Should()
                .Contain(webPartCategoryMock2.Object);

            results = businessLayer.GetWebPartCategories("*cate", false, true);
            results
                .Should()
                .BeEmpty();
        }

        [TestCase]
        public void ShouldGetWebPartCategoriesFromMatchStringWithRegexAndWithoutRecurse()
        {
            // Setup web part category
            var webPartCategoryMock1 = new Mock<IWebPartCategory>();
            webPartCategoryMock1
                .Setup(x => x.CategoryDisplayName)
                .Returns("Display Name");
            webPartCategoryMock1
                .Setup(x => x.CategoryName)
                .Returns("Category");
            var webPartCategoryMock2 = new Mock<IWebPartCategory>();
            webPartCategoryMock2
                .Setup(x => x.CategoryDisplayName)
                .Returns("Different");
            webPartCategoryMock2
                .Setup(x => x.CategoryName)
                .Returns("Different");

            // Setup web part service mock
            var webPartServiceMock = new Mock<IWebPartService>();
            webPartServiceMock
                .Setup(x => x.WebPartCategories)
                .Returns(new IWebPartCategory[]
                {
                    webPartCategoryMock1.Object,
                    webPartCategoryMock2.Object,
                });

            var businessLayer = new GetCMSWebPartCategoryBusiness
            {
                WebPartService = webPartServiceMock.Object,
                WriteDebug = Assert.NotNull,
                WriteVerbose = Assert.NotNull,
            };

            // Test Display Name
            var results = businessLayer.GetWebPartCategories("y+ Name$", true, false);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Single()
                .ShouldBeEquivalentTo(webPartCategoryMock1.Object);

            results = businessLayer.GetWebPartCategories("^y+ Name$", true, false);
            results
                .Should()
                .BeEmpty();

            // Test Name
            results = businessLayer.GetWebPartCategories("^cate+", true, false);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Single()
                .ShouldBeEquivalentTo(webPartCategoryMock1.Object);

            results = businessLayer.GetWebPartCategories("cate$", true, false);
            results
                .Should()
                .BeEmpty();
        }

        [TestCase]
        public void ShouldGetWebPartCategoriesFromMatchStringWithtRegexAndWithRecurse()
        {
            // Setup web part category
            var webPartCategoryMock1 = new Mock<IWebPartCategory>();
            webPartCategoryMock1
                .Setup(x => x.CategoryDisplayName)
                .Returns("Display Name");
            webPartCategoryMock1
                .Setup(x => x.CategoryName)
                .Returns("Category");
            var webPartCategoryMock2 = new Mock<IWebPartCategory>();
            webPartCategoryMock2
                .Setup(x => x.CategoryDisplayName)
                .Returns("Different");
            webPartCategoryMock2
                .Setup(x => x.CategoryName)
                .Returns("Different");

            // Setup web part service mock
            var webPartServiceMock = new Mock<IWebPartService>();
            webPartServiceMock
                .Setup(x => x.WebPartCategories)
                .Returns(new IWebPartCategory[]
                {
                    webPartCategoryMock1.Object,
                    webPartCategoryMock2.Object,
                });
            webPartServiceMock
                .Setup(x => x.GetWebPartCategories(webPartCategoryMock1.Object))
                .Returns(new IWebPartCategory[] { webPartCategoryMock2.Object });

            var businessLayer = new GetCMSWebPartCategoryBusiness
            {
                WebPartService = webPartServiceMock.Object,
                WriteDebug = Assert.NotNull,
                WriteVerbose = Assert.NotNull,
            };

            // Test Display Name
            var results = businessLayer.GetWebPartCategories("y+ Name$", true, true);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Count()
                .Should()
                .Be(2);

            results
                .Should()
                .Contain(webPartCategoryMock1.Object);

            results
                .Should()
                .Contain(webPartCategoryMock2.Object);

            results = businessLayer.GetWebPartCategories("^y+ Name$", true, true);
            results
                .Should()
                .BeEmpty();

            // Test Name
            results = businessLayer.GetWebPartCategories("^cate+", true, true);
            results
                .Should()
                .NotBeNullOrEmpty();

            results
                .Count()
                .Should()
                .Be(2);

            results
                .Should()
                .Contain(webPartCategoryMock1.Object);

            results
                .Should()
                .Contain(webPartCategoryMock2.Object);

            results = businessLayer.GetWebPartCategories("cate$", true, true);
            results
                .Should()
                .BeEmpty();
        }
    }
}
