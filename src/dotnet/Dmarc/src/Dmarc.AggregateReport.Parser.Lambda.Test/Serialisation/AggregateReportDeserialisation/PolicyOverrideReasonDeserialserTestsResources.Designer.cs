﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dmarc.Lambda.AggregateReport.Parser.Test.Serialisation.AggregateReportDeserialisation {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///    A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PolicyOverrideReasonDeserialserTestsResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        internal PolicyOverrideReasonDeserialserTestsResources() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Dmarc.AggregateReport.Parser.Lambda.Test.Serialisation.AggregateReportDeserialisa" +
                            "tion.PolicyOverrideReasonDeserialserTestsResources", typeof(PolicyOverrideReasonDeserialserTestsResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///    Overrides the current thread's CurrentUICulture property for all
        ///    resource lookups using this strongly typed resource class.
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
        ///    Looks up a localized string similar to &lt;reason&gt;
        ///	&lt;type&gt;forwarded&lt;/type&gt;
        ///	&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///	&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///&lt;/reason&gt;.
        /// </summary>
        public static string DuplicateComment {
            get {
                return ResourceManager.GetString("DuplicateComment", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to &lt;reason&gt;
        ///	&lt;type&gt;forwarded&lt;/type&gt;
        ///	&lt;type&gt;forwarded&lt;/type&gt;
        ///	&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///&lt;/reason&gt;.
        /// </summary>
        public static string DuplicateType {
            get {
                return ResourceManager.GetString("DuplicateType", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to &lt;reason&gt;
        ///	&lt;type&gt;forwarded&lt;/type&gt;
        ///&lt;/reason&gt;.
        /// </summary>
        public static string NoComment {
            get {
                return ResourceManager.GetString("NoComment", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to &lt;reason&gt;
        ///	&lt;not_reason&gt;
        ///		&lt;type&gt;forwarded&lt;/type&gt;
        ///		&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///	&lt;/not_reason&gt;
        ///&lt;/reason&gt;.
        /// </summary>
        public static string NotDirectDescendant {
            get {
                return ResourceManager.GetString("NotDirectDescendant", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to &lt;not_reason&gt;
        ///	&lt;type&gt;forwarded&lt;/type&gt;
        ///	&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///&lt;/not_reason&gt;.
        /// </summary>
        public static string NotReason {
            get {
                return ResourceManager.GetString("NotReason", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to &lt;reason&gt;
        ///	&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///&lt;/reason&gt;.
        /// </summary>
        public static string NoType {
            get {
                return ResourceManager.GetString("NoType", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to &lt;reason&gt;
        ///	&lt;type&gt;forwarded&lt;/type&gt;
        ///	&lt;comment&gt;known forwarder&lt;/comment&gt;
        ///&lt;/reason&gt;.
        /// </summary>
        public static string PolicyOverrideReasonStandard {
            get {
                return ResourceManager.GetString("PolicyOverrideReasonStandard", resourceCulture);
            }
        }
    }
}
