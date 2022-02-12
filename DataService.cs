using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TestDataService
{
    public class DataService
    {
        public static void Save(FileType type, string fileName, object Item)
        {
            var tableAdapter= new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();
            byte[] byteData = serializeObject(Item);

            tableAdapter.Save((int)type, fileName,byteData);
        }

        public static T Load<T>(FileType type, string fileName)
            where T : new()
        {
            try
            {
                var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();

                var theBinaryDataRow = tableAdapter.Load((int)type, fileName).LastOrDefault();
                if (theBinaryDataRow == null) return default(T);
                return (T)deserializeBinaryData(theBinaryDataRow.content);

            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public static T Load<T>(int ID)
    where T : new()
        {
            try
            {
                var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();
                var theBinaryDataRow = tableAdapter.LoadByID(ID).FirstOrDefault();

                if (theBinaryDataRow == null) return default(T);
                return (T)deserializeBinaryData(theBinaryDataRow.content);

            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static Dictionary<int,T> LoadAll<T>(FileType type)
            where T:new()
        {
            var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();

            var dataTable = tableAdapter.LoadList((int)type);
            Dictionary<int,T> result = new Dictionary<int,T>();
            foreach (var item in dataTable)
            {
                var deserializedObj = (T)deserializeBinaryData(item.content);
                
                result.Add(item.id, deserializedObj);
            }
            return result;
        }

        public static void Update(FileType type, string fileName,object Item)
        {
            var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();

            var DataTable = tableAdapter.GetData();

            var row = DataTable.Where(r => r.fileName == fileName && r.type == (int)type).FirstOrDefault();

            if(row != null)
            {
                tableAdapter.UpdateRow( serializeObject(Item),row.type, row.fileName);
            }
        }

        public static void Update(int Org_ID, object Item)
        {
            var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();

            var DataTable = tableAdapter.LoadByID(Org_ID);

            var row = DataTable.Where(r => r.id == Org_ID).FirstOrDefault();

            if (row != null)
            {
                tableAdapter.UpdateRow(serializeObject(Item), row.type, row.fileName);
            }
        }

        public static void Delete(FileType type,string fileName)
        {
            var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();
            tableAdapter.DeleteFile((int)type, fileName);
        }
        public static void Delete(int ID)
        {
            var tableAdapter = new TestDataSetTableAdapters.SeralizableObjectsTableAdapter();
            tableAdapter.DeleteByID(ID);

        }
        private static byte[] serializeObject(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private static Object deserializeBinaryData(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

        
    }
}
