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
                Title = new TextSyndicationContent(package.Title),
                Summary = new TextSyndicationContent(package.Summary),
                Content = new UrlSyndicationContent(
                    new Uri(
                        string.Format("{0}/package/{1}/{2}",
                            rootUrl,
                            package.Id,
                            package.Version)),
                    "application/zip")
            };

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

            properties.Add(new XElement(XName.Get("Version", D_NS), package.Version));
            properties.Add(new XElement(XName.Get("NormalizedVersion", D_NS), package.Version));

            properties.Add(new XElement(XName.Get("Copyright", D_NS), "Copyright"));
            properties.Add(new XElement(XName.Get("Created", D_NS), package.Created));
            properties.Add(new XElement(XName.Get("Dependencies", D_NS)));
            properties.Add(new XElement(XName.Get("Description", D_NS), "Description"));

            properties.Add(new XElement(XName.Get("IsLatestVersion", D_NS), "true"));
            properties.Add(new XElement(XName.Get("IsAbsoluteLatestVersion", D_NS), "false"));
            properties.Add(new XElement(XName.Get("IsPrerelease", D_NS), "false"));

            properties.Add(new XElement(XName.Get("Published", D_NS), package.Created));

            properties.Add(new XElement(XName.Get("Title", D_NS), package.Title));

            properties.Add(new XElement(XName.Get("LicenseUrl", D_NS), "http://antix.co.uk"));
            properties.Add(new XElement(XName.Get("ProjectUrl", D_NS), "http://antix.co.uk"));
            properties.Add(new XElement(XName.Get("Tags", D_NS), "Tags"));

           // properties.Add(new XElement(XName.Get("DownloadCount", D_NS), ""));
            properties.Add(new XElement(XName.Get("GalleryDetailsUrl", D_NS), ""));
            properties.Add(new XElement(XName.Get("IconUrl", D_NS), ""));

            item.ElementExtensions.Add(properties);

            return item;
        }
    }
}