//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkypeIntlPortfolio.SyncTool.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products_New
    {
        public Products_New()
        {
            this.Releases = new HashSet<Release>();
        }
    
        public int ProductKey { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> StatusKey { get; set; }
        public Nullable<int> PVoiceKey { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProductFamilyKey { get; set; }
        public Nullable<int> FabricTenantKey { get; set; }
        public Nullable<int> ThreadKey { get; set; }
        public string ISS_Ops_Driver { get; set; }
        public Nullable<int> OwnerLocationKey { get; set; }
        public Nullable<int> ReleaseCadenceKey { get; set; }
        public string IPE_Owner { get; set; }
        public string Test_Owner { get; set; }
        public string Product_Owner { get; set; }
        public Nullable<int> LocBuildProcessKey { get; set; }
        public Nullable<int> LocProcessKey { get; set; }
        public Nullable<bool> GB_Impacting { get; set; }
        public Nullable<bool> French_Loc_Required { get; set; }
        public Nullable<bool> Privacy_Statement_Required { get; set; }
        public Nullable<bool> Voice_Prompt_Loc_Required { get; set; }
        public Nullable<bool> Telemetry_Data_available { get; set; }
        public Nullable<bool> Pseudo_Build_Enabled { get; set; }
        public Nullable<bool> Pseudo_Testing_Run_Regular { get; set; }
        public Nullable<bool> Pseudo_Testing_And_Loc_Checks { get; set; }
        public Nullable<bool> Pseudo_Run_Beofre_Check_In { get; set; }
        public string LCG_File_Path { get; set; }
        public string LCT_File_Path { get; set; }
        public string LCL_File_Path { get; set; }
        public string Localization_VSO_Path { get; set; }
        public string Main_Code_Branch { get; set; }
        public string Localization_Code_Branch { get; set; }
        public string Loc_Team_OneNote { get; set; }
        public string Core_PO { get; set; }
        public string Core_Team_SharePoint { get; set; }
        public string Certification_Type { get; set; }
        public string Cert_Location { get; set; }
        public string Last_Release_Certified { get; set; }
        public Nullable<int> ContentPublishedToKey { get; set; }
        public string Release_build_Location { get; set; }
        public string Core_VSO_Backlog_URL { get; set; }
        public Nullable<int> LanguageSettingsKey { get; set; }
        public Nullable<int> CodeReviewUsedKey { get; set; }
        public Nullable<int> BuildSysKey { get; set; }
        public string Localization_VSO_URL { get; set; }
        public string Telemetry_Contact { get; set; }
        public string Core_Engineering_Contact { get; set; }
        public string Core_Design_Contact { get; set; }
    
        public virtual ICollection<Release> Releases { get; set; }
    }
}
