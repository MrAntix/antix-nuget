using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Antix.NuGet.Packages.Models;

namespace Antix.NuGet.Server.Configuration
{
    public class SyndicationFeedFormatter : MediaTypeFormatter
    {
        const string Atom = "application/atom+xml";

        public SyndicationFeedFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Atom));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        readonly Func<Type, bool> _supportedType = type =>
        {
            if (type == typeof (IPackageMetadata) || type == typeof (IEnumerable<IPackageMetadata>))
                return true;
            return false;
        };

        public override bool CanReadType(Type type)
        {
            return _supportedType(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _supportedType(type);
        }

        public override async Task WriteToStreamAsync(
            Type type, object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext)
        {
            await WritePackages(
                (IEnumerable<IPackageMetadata>) value,
                writeStream);
        }

        static async Task WritePackages(
            IEnumerable<IPackageMetadata> models,
            Stream stream)
        {
            var feed = new SyndicationFeed
            {
                Id = "http://nuget.antix.co.uk/packages",
                Title = new TextSyndicationContent("Packages")
            };

            feed.AttributeExtensions.Add(
                new XmlQualifiedName("base", XNamespace.Xml.ToString()),
                "https://www.nuget.org/api/v2/"
                );

            var d = new XmlQualifiedName("d", XNamespace.Xmlns.ToString());
            const string dns = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            feed.AttributeExtensions.Add(d, dns);

            var m = new XmlQualifiedName("m", XNamespace.Xmlns.ToString());
            const string mns = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            feed.AttributeExtensions.Add(m, mns);

            feed.Links.Add(new SyndicationLink
            {
                RelationshipType = "self",
                Title = "Search",
                Uri = new Uri("http://nuget.antix.co.uk/packages", UriKind.Absolute)
            });

            var items = new List<SyndicationItem>();
            foreach (var package in models.Take(1))
            {
                var item = new SyndicationItem
                {
                    Id = "http://nuget.antix.co.uk/packages/"+ package.Id,
                    Title = new TextSyndicationContent(package.Title),
                    Summary = new TextSyndicationContent(package.Summary),
                    Content = new UrlSyndicationContent(new Uri("", UriKind.Relative), "application/zip")
                };

                item.Categories.Add(new SyndicationCategory
                {
                    Name = "NuGetGallery.V2FeedPackage",
                    Scheme = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme"
                });

                var properties = new XElement(XName.Get("properties", mns));
                properties.Add(new XElement(XName.Get("Version", dns), package.Version));
                properties.Add(new XElement(XName.Get("Published", dns), DateTime.Now));
                properties.Add(new XElement(XName.Get("LicenseUrl", dns), "http://antix.co.uk"));
                properties.Add(new XElement(XName.Get("ProjectUrl", dns), "http://antix.co.uk"));
                properties.Add(new XElement(XName.Get("Description", dns), "Description"));
                properties.Add(new XElement(XName.Get("Tags", dns), "Tags"));

                item.ElementExtensions.Add(properties);

                //item.Authors.Add(new SyndicationPerson() { Name = u.CreatedBy });
                items.Add(item);
            }
            feed.Items = items;

            using (var writer = XmlWriter
                .Create(stream, new XmlWriterSettings
                {
                    Indent = true
                }))
            {
                var atomformatter = new Atom10FeedFormatter(feed);
                atomformatter.WriteTo(writer);
            }
        }
    }
}