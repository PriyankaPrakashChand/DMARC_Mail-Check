﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dmarc.MxSecurityTester.Dao {
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
    public class TlsSecurityProfileDaoResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        internal TlsSecurityProfileDaoResources() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Dmarc.MxSecurityTester.Dao.TlsSecurityProfileDaoResources", typeof(TlsSecurityProfileDaoResources).GetTypeInfo().Assembly);
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
        ///    Looks up a localized string similar to INSERT INTO `certificate`
        ///(`thumb_print`,
        ///`raw_data`)
        ///VALUES.
        /// </summary>
        public static string InsertCertificates {
            get {
                return ResourceManager.GetString("InsertCertificates", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to ON DUPLICATE KEY UPDATE `thumb_print` = VALUES(`thumb_print`);.
        /// </summary>
        public static string InsertCertificatesOnDuplicateKey {
            get {
                return ResourceManager.GetString("InsertCertificatesOnDuplicateKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to (@a{0},
        ///@b{0}).
        /// </summary>
        public static string InsertCertificateValuesFormatString {
            get {
                return ResourceManager.GetString("InsertCertificateValuesFormatString", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to INSERT INTO `certificate_mapping`
        ///(`sequence`,
        ///`dns_record_mx_tls_profile_id`,
        ///`certificate_thumb_print`)
        ///VALUES.
        /// </summary>
        public static string InsertMapping {
            get {
                return ResourceManager.GetString("InsertMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to ON DUPLICATE KEY UPDATE `sequence` = VALUES(`sequence`);
        ///.
        /// </summary>
        public static string InsertMappingOnDuplicateKey {
            get {
                return ResourceManager.GetString("InsertMappingOnDuplicateKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to (@a{0},
        ///@b{0},
        ///@c{0}).
        /// </summary>
        public static string InsertMappingValueFormatString {
            get {
                return ResourceManager.GetString("InsertMappingValueFormatString", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to INSERT INTO `dns_record_mx_tls_profile_results`
        ///(`id`,`mx_record_id`,
        ///`end_date`, 
        ///`failure_count`,
        ///`data`)
        ///VALUES.
        /// </summary>
        public static string InsertRecord {
            get {
                return ResourceManager.GetString("InsertRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to ON DUPLICATE KEY UPDATE `end_date` = VALUES(`end_date`),
        ///`failure_count` = VALUES(`failure_count`),
        ///`last_checked` = UTC_TIMESTAMP();
        ///SELECT LAST_INSERT_ID();.
        /// </summary>
        public static string InsertRecordOnDuplicateKey {
            get {
                return ResourceManager.GetString("InsertRecordOnDuplicateKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to (@a{0},
        ///@b{0},
        ///@c{0},
        ///@d{0},
        ///@e{0}).
        /// </summary>
        public static string InsertRecordValueFormatString {
            get {
                return ResourceManager.GetString("InsertRecordValueFormatString", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Looks up a localized string similar to  SELECT 
        ///    temp.domain_id,
        ///    temp.domain_name,
        ///    mx.id AS &apos;mx_record_id&apos;,
        ///    mx.hostname AS &apos;mx_record_hostname&apos;,
        ///    tls.id AS &apos;tls_security_profile_id&apos;,
        ///	tls.failure_count as &apos;failure_count&apos;,
        ///	tls.data as &apos;data&apos;,	 
        ///    temp.lst_checked AS &apos;last_checked&apos;,
        ///    c.thumb_print AS &apos;certificate_thumb_print&apos;,
        ///    c.raw_data AS &apos;certificate_raw_data&apos;
        ///FROM (
        ///	SELECT d1.id as domain_id,
        ///	d1.name as domain_name,
        ///	MAX(COALESCE(tls1.last_checked, &apos;1900-01-01&apos;)) as lst_checked
        ///	FROM domain d1
        ///	JOIN [rest of string was truncated]&quot;;.
        /// </summary>
        public static string SelectSecurityProfilesToUpdate {
            get {
                return ResourceManager.GetString("SelectSecurityProfilesToUpdate", resourceCulture);
            }
        }
    }
}
