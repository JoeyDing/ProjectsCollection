using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;
using VsoWorkItemsSync.Core.Configuration;
using VsoWorkItemsSync.Core.Exception;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core.WorkItem
{
    public class WorkItemProvider
    {
        protected ConfigurationMappings configurationMappings { get; private set; }
        protected VsoDbMapping VsoDbMapping { get; private set; }

        protected VsoContext VsoContext { get; private set; }
        protected VwsContext DbContext { get; private set; }

        public WorkItemProvider(ConfigurationMappings configurationMappings, VsoDbMapping vsoDbMapping)
        {
            this.configurationMappings = configurationMappings;
            this.VsoDbMapping = vsoDbMapping;
            string authenticationKey = configurationMappings.VsoPrivateKey;
            string vsoRootAccount = configurationMappings.VsoRootAccount;
            this.VsoContext = new VsoContext(vsoRootAccount, authenticationKey);
            this.DbContext = new VwsContext(configurationMappings.DbConnectionString);
        }

        public bool ValidateVsoDbMapping(out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(this.VsoDbMapping.DbTableName))
                errors.Add("DbTableName cannot be empty");

            if (string.IsNullOrWhiteSpace(this.VsoDbMapping.VsoWorkItemName))
                errors.Add("VsoWorkItemName cannot be empty");

            if (!this.VsoDbMapping.Fields.Any())
                errors.Add("Fields cannot be empty");

            if (!errors.Any())
            {
                foreach (var field in this.VsoDbMapping.Fields)
                {
                    if (string.IsNullOrWhiteSpace(field.DbField))
                        errors.Add(string.Format("DbTableName:\"{0}\" / VsoWorkItemName: \"{1}\" / DbField: \"{2}\"/ DbField cannot be empty", this.VsoDbMapping.DbTableName, this.VsoDbMapping.VsoWorkItemName, field.DbField));

                    if (string.IsNullOrWhiteSpace(field.VsoField))
                        errors.Add(string.Format("DbTableName:\"{0}\" / VsoWorkItemName: \"{1}\" / VsoField: \"{2}\"/ VsoField Cannot be empty", this.VsoDbMapping.DbTableName, this.VsoDbMapping.VsoWorkItemName, field.VsoField));

                    if (string.IsNullOrWhiteSpace(field.DataType) || !Enum.GetNames(typeof(ValidDataTypes)).Contains(field.DataType.ToLower()))
                        errors.Add(string.Format("DbTableName:\"{0}\" / VsoWorkItemName: \"{1}\" / field: \"{2}/{3}\"/ DataType must be a valid data type", this.VsoDbMapping.DbTableName, this.VsoDbMapping.VsoWorkItemName, field.DbField, field.VsoField));
                }
            }

            return !errors.Any();
        }

        public void SyncData()
        {
            var errors = new List<string>();
            try
            {
                //Get list of vso fields
                var vsoFields = this.VsoDbMapping.Fields.Select(c => c.VsoField).ToArray();

                //Query vso API
                var result = this.VsoContext.Reporting_GetAllWorkItemRevisions(
                    project: this.configurationMappings.VsoProjectName,
                    type: this.VsoDbMapping.VsoWorkItemName,
                    fields: vsoFields,
                    pauseTimeBetweenBatchInSec: 1
                    );

                //Query list of db fields
            }
            catch (System.Exception e)
            {
                errors.Add(e.ToString());
            }

            if (errors.Any())
                throw new SyncException(errors);
        }

        public List<Dictionary<string, object>> RunDbQuery()
        {
            var errors = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            //create dynamic type based on mappings
            var dbFieldsInfo = this.VsoDbMapping.Fields;
            var typeBuilder = CreateTypeBuilder("DynamicAssembly", "Module", "DynamicType");
            foreach (var fieldInfo in dbFieldsInfo)
            {
                Type fieldType = null;
                switch (fieldInfo.DataType.ToLower())
                {
                    case "string":
                        fieldType = typeof(string);
                        break;

                    case "int":
                    case "int32":
                    case "int64":
                        fieldType = typeof(long?);
                        break;

                    case "bool":
                        fieldType = typeof(bool?);
                        break;

                    case "datetime":
                        fieldType = typeof(DateTime?);
                        break;

                    default:
                        errors.Add(string.Format("Field type:\"{0}\" is not a valid type.", fieldInfo.DataType));
                        break;
                }
                CreateAutoImplementedProperty(typeBuilder, fieldInfo.DbField, fieldType);
            }

            if (errors.Any())
                throw new TypeMappingException(errors);

            //execute dynamic query
            Type resultType = typeBuilder.CreateType();
            var rrRes = this.DbContext.Database.SqlQuery<int>("select 10").ToList();
            dynamic qRes = this.DbContext.Database.SqlQuery(
                 resultType,
                 string.Format("select top 10 {0} from {1}",
                                     dbFieldsInfo.Select(c => c.DbField).Aggregate((a, b) => a + "," + b), this.VsoDbMapping.DbTableName));

            foreach (dynamic item in qRes)
            {
                var exp = new Dictionary<string, object>();
                foreach (var prop in resultType.GetProperties())
                {
                    exp[prop.Name] = prop.GetValue(item);
                }
                result.Add(exp);
            }

            return result;
        }

        #region Private Static Helper

        private static TypeBuilder CreateTypeBuilder(
          string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void CreateAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }

        #endregion Private Static Helper
    }
}