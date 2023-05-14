﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiTemplate.Translations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ValidationTranslations {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationTranslations() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApiTemplate.Translations.ValidationTranslations", typeof(ValidationTranslations).Assembly);
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
        ///   Looks up a localized string similar to The Date/Time value seems to be incorrect..
        /// </summary>
        public static string DateTimeIsNotCorrect {
            get {
                return ResourceManager.GetString("DateTimeIsNotCorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID value must be provided..
        /// </summary>
        public static string Id_MustBeSupplied {
            get {
                return ResourceManager.GetString("Id_MustBeSupplied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The textual value cannot be empty..
        /// </summary>
        public static string StringCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("StringCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The textual value length should be greater than {0} characters..
        /// </summary>
        public static string StringLengthGreaterThan {
            get {
                return ResourceManager.GetString("StringLengthGreaterThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Priority must be set!.
        /// </summary>
        public static string SystemFeedback_Priority {
            get {
                return ResourceManager.GetString("SystemFeedback_Priority", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Countdown datetime should be within start and end times!.
        /// </summary>
        public static string SystemNotification_CountdownTime {
            get {
                return ResourceManager.GetString("SystemNotification_CountdownTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Making emphasize should be within start and end times!.
        /// </summary>
        public static string SystemNotification_EmphasizeTime {
            get {
                return ResourceManager.GetString("SystemNotification_EmphasizeTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Emphasize type must be specified when emphasize time is given!.
        /// </summary>
        public static string SystemNotification_EmphasizeType {
            get {
                return ResourceManager.GetString("SystemNotification_EmphasizeType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Message language code should be a 2-letter code..
        /// </summary>
        public static string SystemNotification_Language {
            get {
                return ResourceManager.GetString("SystemNotification_Language", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification should have at least one message in any language!.
        /// </summary>
        public static string SystemNotification_NoMessages {
            get {
                return ResourceManager.GetString("SystemNotification_NoMessages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End time should be greater than Start time!.
        /// </summary>
        public static string SystemNotification_StartDateLessThanEndDate {
            get {
                return ResourceManager.GetString("SystemNotification_StartDateLessThanEndDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This is a test..
        /// </summary>
        public static string Test {
            get {
                return ResourceManager.GetString("Test", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Placeholder: {0} test..
        /// </summary>
        public static string TestPlaceholder {
            get {
                return ResourceManager.GetString("TestPlaceholder", resourceCulture);
            }
        }
    }
}