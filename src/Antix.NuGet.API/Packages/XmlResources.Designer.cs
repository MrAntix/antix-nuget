﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Antix.NuGet.API.Packages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class XmlResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal XmlResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Antix.NuGet.API.Packages.XmlResources", typeof(XmlResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;root&gt;
        ///	&lt;!-- 
        ///		Microsoft ResX Schema
        ///
        ///		Version 1.3
        ///
        ///		The primary goals of this format is to allow a simple XML format 
        ///		that is mostly human readable. The generation and parsing of the 
        ///		various data types are done through the TypeConverter classes 
        ///		associated with the data types.
        ///
        ///		Example:
        ///
        ///		... ado.net/XML headers &amp; schema ...
        ///		&lt;resheader name=&quot;resmimetype&quot;&gt;text/microsoft-resx&lt;/resheader&gt;
        ///		&lt;resheader name=&quot;version&quot;&gt;1.3&lt;/resheader&gt;
        ///		&lt;resh [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Metadata {
            get {
                return ResourceManager.GetString("Metadata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; standalone=&quot;yes&quot;?&gt;
        ///&lt;service xml:base=&quot;http://nuget.org/api/v2/&quot; xmlns:atom=&quot;http://www.w3.org/2005/Atom&quot; xmlns:app=&quot;http://www.w3.org/2007/app&quot; xmlns=&quot;http://www.w3.org/2007/app&quot;&gt;
        ///  &lt;workspace&gt;
        ///    &lt;atom:title&gt;Default&lt;/atom:title&gt;
        ///    &lt;collection href=&quot;Packages&quot;&gt;
        ///      &lt;atom:title&gt;Packages&lt;/atom:title&gt;
        ///    &lt;/collection&gt;
        ///  &lt;/workspace&gt;
        ///&lt;/service&gt;.
        /// </summary>
        public static string Root {
            get {
                return ResourceManager.GetString("Root", resourceCulture);
            }
        }
    }
}
