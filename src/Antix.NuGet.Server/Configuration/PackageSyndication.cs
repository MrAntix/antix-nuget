using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.Server.Configuration
{
    public static class PackageSyndication
    {
        static readonly XmlQualifiedName Base = new XmlQualifiedName("base", XNamespace.Xml.ToString());

        static readonly XmlQualifiedName D = new XmlQualifiedName("d", XNamespace.Xmlns.ToString());
        const string D_NS = "http://schemas.microsoft.com/ado/2007/08/dataservices";

        static readonly XmlQualifiedName M = new XmlQualifiedName("m", XNamespace.Xmlns.ToString());
        const string M_NS = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        public static void ApplyNS(
            Dictionary<XmlQualifiedName, string> attributeExtensions,
            string rootUrl)
        {
            attributeExtensions.Add(Base, rootUrl);
            attributeExtensions.Add(D, D_NS);
            attributeExtensions.Add(M, M_NS);
        }

        public static SyndicationItem CreateItem(
            IPackageMetadata package, string rootUrl)
        {
            var packageRef = string.Format(
                "Packages(Id='{0}',Version='{1}')",
                package.Id,
                package.Version);

            var item = new SyndicationItem
            {
                Id = string.Format("{0}/{1}", rootUrl, packageRef),
                Title = new TextSyndicationContent(package.Id),
                Summary = new TextSyndicationContent(package.Summary),
                Content = new UrlSyndicationContent(
                    new Uri(
                        string.Format("{0}/package/{1}/{2}",
                            rootUrl,
                            package.Id,
                            package.Version)),
                    "application/zip")
            };

            item.Authors.Add(new SyndicationPerson
            {
                Name = "Anthony Johnston"
            });

            item.Categories.Add(new SyndicationCategory
            {
                Name = "NuGetGallery.V2FeedPackage",
                Scheme = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme"
            });

            item.Links.Add(new SyndicationLink
            {
                RelationshipType = "edit",
                Title = "V2FeedPackage",
                Uri = new Uri(packageRef, UriKind.Relative)
            });

            item.Links.Add(new SyndicationLink
            {
                RelationshipType = "edit-media",
                Title = "V2FeedPackage",
                Uri = new Uri(
                    string.Format("{0}/$value", packageRef),
                    UriKind.Relative)
            });

            var properties = new XElement(XName.Get("properties", M_NS));

            properties.Add(GetString("Version", package.Version));
            properties.Add(GetString("NormalizedVersion", package.Version));

            properties.Add(GetString("Copyright", package.Copyright));
            properties.Add(GetDate("Created", package.Created));
            properties.Add(GetString("Dependencies", package.Dependencies));
            properties.Add(GetString("Description", package.Description));
            properties.Add(GetString("Summary", package.Summary));
            properties.Add(GetString("ReleaseNotes", package.ReleaseNotes));

            properties.Add(GetBool("IsLatestVersion", true));
            properties.Add(GetBool("IsAbsoluteLatestVersion", false));
            properties.Add(GetBool("IsPrerelease", false));

            properties.Add(GetDate("Published", package.Created));

            properties.Add(GetString("Title", package.Title));

            properties.Add(GetString("LicenseUrl", "http://antix.co.uk"));
            properties.Add(GetString("ProjectUrl", "http://antix.co.uk"));
            properties.Add(GetString("Tags", package.Tags));

            // properties.Add(GetString("DownloadCount", ""));
            properties.Add(GetString("GalleryDetailsUrl", ""));
            properties.Add(GetString("IconUrl", ""));

            //properties.Add(GetString("PackageHash", package.MD5Hash));
            //properties.Add(GetString("PackageHashAlgorithm", "MD5"));

            properties.Add(Get("Language"));
            properties.Add(Get("MinClientVersion"));
            properties.Add(Get("LastEdited"));
            properties.Add(Get("LicenseNames"));
            properties.Add(Get("LicenseReportUrl"));


            item.ElementExtensions.Add(properties);

            return item;
        }

        static XElement Get(string name)
        {
            return new XElement(
                XName.Get(name, D_NS),
                new XAttribute(XName.Get("null", M_NS), true)
                );
        }

        static XElement GetString(string name, string value)
        {
            return new XElement(
                XName.Get(name, D_NS),
                value
                );
        }

        static XElement GetBool(string name, bool value)
        {
            return new XElement(
                XName.Get(name, D_NS),
                new XAttribute(XName.Get("type", M_NS), "Edm.Boolean"),
                value
                );
        }

        static XElement GetDate(string name, DateTimeOffset value)
        {
            return new XElement(
                XName.Get(name, D_NS),
                new XAttribute(XName.Get("type", M_NS), "Edm.DateTime"),
                value
                );
        }

    }
}