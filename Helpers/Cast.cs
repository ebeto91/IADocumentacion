using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Reflection;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers
{
    public static class Cast
    {
        public static T CloneObject<T>(this T source) where T : class
        {
            if (source != null)
            {
                return source.ToJson<T>().FromJson<T>();
            }
            //// **** made things  
            return null;
        }

        public static T ConvertTo<T>(this object source) where T : class
        {
            var json = JsonConvert.SerializeObject(source);
            var obj2 = json.FromJson<T>();

            return obj2;
        }

        public static string ToJson<T>(this T obj, JsonSerializerSettings settings)
        {
            if (obj != null)
            {
                settings = settings ?? new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
                };
                return JsonConvert.SerializeObject(obj, settings);
            }
            return null;
        }

        public static T FromJson<T>(this string json, JsonSerializerSettings settings)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    settings = settings ?? new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None,
                        NullValueHandling = NullValueHandling.Ignore,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
                    };
                    return JsonConvert.DeserializeObject<T>(json, settings);
                }
                catch (Exception)
                {

                    return default(T);
                }

            }
            return default(T);
        }

        public static string ToJson<T>(this T obj)
        {
            return ToJson<T>(obj, null);
        }

        public static T FromJson<T>(this string json)
        {
            return FromJson<T>(json, null);
        }

        /// <summary>
        /// Extension method to return an enum value of type T for the given string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            var MyEnum = (T)Enum.Parse(typeof(T), value, true);
            return MyEnum;
        }

        /// <summary>
        /// Extension method to return an enum value of type T for the given int.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value)
        {
            var name = Enum.GetName(typeof(T), value);
            return name.ToEnum<T>();
        }

        public static object CombineObjects(this object item, object add)
        {
            var ret = new ExpandoObject() as IDictionary<string, Object>;

            var props = item.GetType().GetProperties();
            foreach (var property in props)
            {
                if (property.CanRead)
                {
                    ret[property.Name] = property.GetValue(item);
                }
            }

            props = add.GetType().GetProperties();
            foreach (var property in props)
            {
                if (property.CanRead)
                {
                    ret[property.Name] = property.GetValue(add);
                }
            }

            return ret;
        }

        public static void MergeModal<T>(this T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null && (string)value != "")
                    prop.SetValue(target, value, null);
            }
        }

        public static bool IsBase64String(this string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }


        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props =
                    TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
            catch
            {
                return data.TryToDataTable();
            }
        }

        public static DataTable TryToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static List<T> ToListof<T>(this DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    var newValue = Convert.ChangeType(dataRow[properties.Name], properties.PropertyType);
                    properties.SetValue(instanceOfT, newValue, null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }
    }
}
