﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Antix.NuGet.Application.Packages.Storage {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    public sealed partial class FileSystemStorageSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static FileSystemStorageSettings defaultInstance = ((FileSystemStorageSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new FileSystemStorageSettings())));
        
        public static FileSystemStorageSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("~\\content\\packages")]
        public string RootDirectory {
            get {
                return ((string)(this["RootDirectory"]));
            }
        }
    }
}
