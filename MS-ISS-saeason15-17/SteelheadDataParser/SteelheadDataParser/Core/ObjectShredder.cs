﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SteelheadDataParser.Core
{
    public class ObjectShredder<T>
    {
        private static object _locker = new object();
        private System.Reflection.FieldInfo[] _fi;
        private System.Reflection.PropertyInfo[] _pi;
        private System.Collections.Generic.Dictionary<string, int> _ordinalMap;
        private System.Type _type;

        // ObjectShredder constructor.
        public ObjectShredder()
        {
            _type = typeof(T);
            _fi = _type.GetFields().Where(c => !c.GetCustomAttributes().Any(x => x is ImportIgnoreAttribute)).ToArray();
            _pi = _type.GetProperties().Where(c => !c.GetCustomAttributes().Any(x => x is ImportIgnoreAttribute)).ToArray();
            _ordinalMap = new Dictionary<string, int>();
        }

        /// <summary>
        /// Loads a DataTable from a sequence of objects.
        /// </summary>
        /// <param name="source">The sequence of objects to load into the DataTable.</param>
        /// <param name="table">The input table. The schema of the table must match that
        /// the type T.  If the table is null, a new table is created with a schema
        /// created from the public properties and fields of the type T.</param>
        /// <param name="options">Specifies how values from the source sequence will be applied to
        /// existing rows in the table.</param>
        /// <returns>A DataTable created from the source sequence.</returns>
        public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            lock (_locker)
            {
                // Load the table from the scalar sequence if T is a primitive type.
                if (typeof(T).IsPrimitive)
                {
                    return ShredPrimitive(source, table, options);
                }

                // Create a new table if the input table is null.
                if (table == null)
                {
                    table = new DataTable(typeof(T).Name);
                }

                // Initialize the ordinal map and extend the table schema based on type T.
                table = ExtendTable(table, typeof(T));

                // Enumerate the source sequence and load the object values into rows.
                table.BeginLoadData();
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (e.Current != null)
                        {
                            if (options != null)
                            {
                                table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                            }
                            else
                            {
                                table.LoadDataRow(ShredObject(table, e.Current), true);
                            }
                        }
                    }
                }
                table.EndLoadData();

                // Return the table.
                return table;
            }
        }

        public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            lock (_locker)
            {
                // Create a new table if the input table is null.
                if (table == null)
                {
                    table = new DataTable(typeof(T).Name);
                }

                if (!table.Columns.Contains("Value"))
                {
                    table.Columns.Add("Value", typeof(T));
                }

                // Enumerate the source sequence and load the scalar values into rows.
                table.BeginLoadData();
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    Object[] values = new object[table.Columns.Count];
                    while (e.MoveNext())
                    {
                        values[table.Columns["Value"].Ordinal] = e.Current;

                        if (options != null)
                        {
                            table.LoadDataRow(values, (LoadOption)options);
                        }
                        else
                        {
                            table.LoadDataRow(values, true);
                        }
                    }
                }
                table.EndLoadData();

                // Return the table.
                return table;
            }
        }

        public object[] ShredObject(DataTable table, T instance)
        {
            FieldInfo[] fi = _fi;
            PropertyInfo[] pi = _pi;

            if (instance.GetType() != typeof(T))
            {
                // If the instance is derived from T, extend the table schema
                // and get the properties and fields.
                ExtendTable(table, instance.GetType());
                fi = instance.GetType().GetFields().Where(c => !c.GetCustomAttributes().Any(x => x is ImportIgnoreAttribute)).ToArray();
                pi = instance.GetType().GetProperties().Where(c => !c.GetCustomAttributes().Any(x => x is ImportIgnoreAttribute)).ToArray();
            }

            // Add the property and field values of the instance to an array.
            Object[] values = new object[table.Columns.Count];
            foreach (FieldInfo f in fi)
            {
                values[_ordinalMap[f.Name]] = f.GetValue(instance);
            }

            foreach (PropertyInfo p in pi)
            {
                values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
            }

            // Return the property and field values of the instance.
            return values;
        }

        public DataTable ExtendTable(DataTable table, Type type)
        {
            // Extend the table schema if the input table was null or if the value
            // in the sequence is derived from type T.
            foreach (FieldInfo f in type.GetFields())
            {
                if (!_ordinalMap.ContainsKey(f.Name))
                {
                    if (f.GetCustomAttribute<ImportIgnoreAttribute>() == null)
                    {
                        string columnName = "";
                        if (f.GetCustomAttribute<DBNameAttribute>() != null)
                            columnName = f.GetCustomAttribute<DBNameAttribute>()._dbName;
                        else
                            columnName = f.Name;
                        // Add the field as a column in the table if it doesn't exist
                        // already.
                        DataColumn dc = table.Columns.Contains(columnName) ? table.Columns[columnName]
                            : table.Columns.Add(columnName, f.FieldType);

                        // Add the field to the ordinal map.
                        _ordinalMap.Add(f.Name, dc.Ordinal);
                    }
                }
            }
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (p.GetCustomAttribute<ImportIgnoreAttribute>() == null)
                {
                    if (!_ordinalMap.ContainsKey(p.Name))
                    {
                        string columnName = "";
                        if (p.GetCustomAttribute<DBNameAttribute>() != null)

                            columnName = p.GetCustomAttribute<DBNameAttribute>()._dbName;
                        else
                            columnName = p.Name;
                        // Add the property as a column in the table if it doesn't exist
                        // already.
                        DataColumn dc = table.Columns.Contains(columnName) ? table.Columns[columnName]
                            : table.Columns.Add(columnName, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);

                        // Add the property to the ordinal map.
                        _ordinalMap.Add(p.Name, dc.Ordinal);
                    }
                }
            }

            // Return the table.
            return table;
        }
    }
}