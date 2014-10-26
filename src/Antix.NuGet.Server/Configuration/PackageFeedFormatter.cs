using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Antix.NuGet.API.Packages.Models;

namespace Antix.NuGet.Server.Configuration
{
    public class PackageFeedFormatter : MediaTypeFormatter
    {
        const string Atom = "application/atom+xml";

        public PackageFeedFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Atom));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof (PackageFeedResponse);
        }

        public override async Task WriteToStreamAsync(
            Type type, object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext)
        {
            await WritePackages(
                (PackageFeedResponse) value,
                writeStream);
        }

        static async Task WritePackages(
            PackageFeedResponse response,
            Stream stream)
        {
            var rootUri = response.RequestAuthorityUri;
            var feed = new SyndicationFeed
            {
                Id = string.Format("{0}/api/Packages", rootUri),
                Title = new TextSyndicationContent("Packages")
            };

            PackageSyndication.ApplyNS(feed.AttributeExtensions, rootUri);

            feed.Links.Add(new SyndicationLink
            {
                RelationshipType = "self",
                Title = "Packages",
                Uri = new Uri("Packages", UriKind.Relative)
            });
            
            feed.Items = response.Packages
                .Select(package => PackageSyndication.CreateItem(package, rootUri))
                .ToList();
            
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