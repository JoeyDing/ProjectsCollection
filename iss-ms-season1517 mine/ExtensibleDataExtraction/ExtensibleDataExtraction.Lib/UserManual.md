# User Manual

## ``Introduction``
The User Manual contains all essential information for the user to make full use of the program.  This manual includes a description of the system functions and capabilities.

## ``Purpose and Scope``
Main purpose of this manual is to introduce structure of config.xml file, how it works, what advantages it has, what parameters it needs, how it gets involved with the whole process of the project.


## ``Instruction``

### ``Advantages of xml configuration``
Usually developers get API data with certain conditions only by chanding the code and recompile it again. It seems quite straingforward but costs too high and less efficient. What if there is a config file to configures dll file, classes, parameters , data field and Sql column mapping even which section of json data we need and whatever configuration we want to cutomize. This project reduces developers workload and make it easy to access any library by configuration.

### ``Config.xml example:``

    <?xml version="1.0" encoding="utf-8"?>
    <ExtensibleItems>
    <ExtensibleItem>
        <JsonEndPoint>
        <CustomLib>
            <DllName>ExtensibleDataExtraction.Lib.Tests</DllName>
            <FetchClass>ExtensibleDataExtraction.Lib.Tests.IntegrationTests.CustomLibImplementation.FetchTest</FetchClass>
            <FetchParams>
            <FetchParam>
                <Key>Type</Key>
                <Value>Task</Value>
            </FetchParam>
            <FetchParam>
                <Key>FromDate</Key>
                <Value>[CurrentDate-1]</Value>
            </FetchParam>
            </FetchParams>
        </CustomLib>
        <ResultTemplate>value1\value2\value3</ResultTemplate>
        <ResultItemTemplate>field1\field2\field3</ResultItemTemplate>
        </JsonEndPoint>
        <Mapping>
        <SqlTableName>CountryLanguageCount</SqlTableName>
        <SaveType>Full</SaveType>
        <DeleteDBData>false</DeleteDBData>
        <Fields>
            <Field>
            <JsonFieldName>System.Id</JsonFieldName>
            <SqlColumnName>SystemId</SqlColumnName>
            <IsIdentity>1</IsIdentity>
            </Field>
            <Field>
            <JsonFieldName>System.AreaId</JsonFieldName>
            <SqlColumnName>SystemAreaId</SqlColumnName>
            <IsIdentity>0</IsIdentity>
            </Field>
            <Field>
            <JsonFieldName>System.AreaPath</JsonFieldName>
            <SqlColumnName>SystemAreaPath</SqlColumnName>
            <IsIdentity>0</IsIdentity>
            </Field>
            <Field>
            <JsonFieldName>System.TeamProject</JsonFieldName>
            <SqlColumnName>SystemTeamProject</SqlColumnName>
            <IsIdentity>0</IsIdentity>
            </Field>
        </Fields>
        </Mapping>
    </ExtensibleItem>
    </ExtensibleItems>

### ``Introduction of xml sections:``

| Node |  Value or Child | Type | Introduction |
| :--- | :---: | :---- | :---- |
|```ExtensibleItem ```| JsonEndPoint and Mapping | JsonEndPoint and Mapping | ExtensibleItem includes two sections JsonEndPoint and Mapping. |
|```JsonEndPoint ```| CustomLib, ResultTemplate, ResultItemTemplate | CustomLib, ResultTemplate, ResultItemTemplate | JsonEndPoint contains custom library and templates in which configure custom dll path, class, json array and json object name. |
|```Mapping ```| SqlTableName, SaveType, DeleteDBData, Fields | SqlTableName, SaveType, DeleteDBData, Fields | Mapping contains one-to-one field name and column name pair, either configure save type, table name and delete database data.  |
|```CustomLib ```| DllName, FetchClass, FetchParams| DllName, FetchClass, FetchParams | CustomLib section is to configure dll name and class name, parameters that can be possibly used in dll fetch data class. |
|```FetchParam ```| key and value | key and value string | Key and value separately represents for parameter name and value. |
|```ResultTemplate ```| path to json array.It can be single or multiple value. | string | Path to retrieve json Array. e.g, value1\value2\value3. value1 and value2 are both object names and value3 is name of Array.  |
|```ResultItemTemplate ```| path to json object.It can be single or multiple value. | string | Path to retrieve json Object. e.g, field1\field2\field3. field1, field2, field3 are all object names. |
|```SqlTableName ```| table name | string | Sql table name to save json data. |
|```SaveType ```| Full or Incremental | JsonEndPoint and Mapping| Value can be Full or Incremental. If it is Full, remove all data from table and insert the new json data. If it is Incremental, insert the data that is in json but not in table. |
|```DeleteDBData ```| true or false | bool | If it is true, compare json data and database data, remove data in database if there is any data in database. |
|```Fields ```| field | field | Includes field mapping. |
|```field ```| JsonFieldName, SqlColumnName,  IsIdentity | JsonFieldName, SqlColumnName,  IsIdentity | One field will be one column in database table. |
|```JsonFieldName ```| json field name | string | Name of the json field. |
|```SqlColumnName ```| sql table column name | string | Name of the Sql column. It has the same name as JsonFieldName, but no dot in between. |
|```IsIdentity ```| identity | 0 or 1 |  |



## `Tutorial of it's usage in Console Application.`     

### `Define values of DllName and FetchClass: `
In config file example above we have test library path to reference these two values. Specify dll name and class name for specific dll files. Example ``Config.xml`` file has specified dll path and class name of ``ExtensibleDataExtraction.Lib.Test.`` 

`ExtensibleContext.class , StartProcess(string xmlConfigurationFilePath)` method is workflow that basicly including four steps below: 

    //1.Get exensible items from Config file , will return extensibleItems.
    ConfigurationSerializerService configService = new ConfigurationSerializerService();
    ExtensibleItems extensibleItems = configService.GetExensibleItemsFromConfig(xmlConfigurationFilePath);
    var context = new ExtensibleContext();

    //2.for each of the elements, fetch the data to get the converted file of specific type
    foreach (ExtensibleItem extensibleItem in extensibleItems.Items)
    {
        //3. parse json data. a data set will be generated
        var jsonData = context.RetrieveJsonData(extensibleItem);
        var dataSet = context.ParseJsonData(jsonData, extensibleItem);

        //4. save to db(pass the savetype in, hence user could decide how do thay want to insert data)
        context.SaveToDatabase(dataSet);
    }

This is dummy `FetchClass` which is referenced in `ParseJsonData` method to read json string from json file.


     public string FetchData(ExtensibleItem param)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json.json");
            string jsonString;
            using (StreamReader sr = new StreamReader(configPath))
            {
                jsonString = sr.ReadToEnd();
            }
            return jsonString;
        }

`ParseJsonData` is a class which returns `ExtensibleDataSet` that fulfills the `FetchParam` conditions and `Mapping` conditions such as`JsonFieldName` and `SqlColumnName` that are defined in `Config.xml`.

`SaveToDatabase` saves `ExtensibleDataSet` into the table that is defined `SqlTableName`.If `SaveType` value is `Full`, remove all table data . If the value is `Incremental`, insert data that is not in table. If `DeleteDbData` value is true, compare json data with database data, if there is any data in database, remove all data from database.

Run `Main()` method in `Application.cs`

    public static void Main(string[] args)
        {
            var context = new ExtensibleContext();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            context.StartProcess(configPath);
        }


