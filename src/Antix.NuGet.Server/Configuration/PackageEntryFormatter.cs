using System;
using System.IO;
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
    public class PackageEntryFormatter : MediaTypeFormatter
    {
        const string Atom = "application/atom+xml";

        public PackageEntryFormatter()
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
            return type == typeof (PackageEntryResponse);
        }

        public override async Task WriteToStreamAsync(
            Type type, object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext)
        {
            await WritePackages(
                (PackageEntryResponse) value,
                writeStream);
        }

        static async Task WritePackages(
            PackageEntryResponse response,
            Stream stream)
        {
            var package = response.Package;
            var rootUri = response.RequestAuthorityUri;

            var item = PackageSyndication.CreateItem(package, rootUri);

            PackageSyndication.ApplyNS(item.AttributeExtensions, rootUri);

            using (var writer = XmlWriter
                .Create(stream, new XmlWriterSettings
                {
                    Indent = true
                }))
            {
                var atomformatter = new Atom10ItemFormatter(item);
                atomformatter.WriteTo(writer);
            }
        }
    }
}