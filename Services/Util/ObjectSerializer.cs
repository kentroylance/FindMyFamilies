using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FindMyFamilies.Util {
    public class ObjectSerializer<T> {
        protected IFormatter iformatter;

        public ObjectSerializer() {
            this.iformatter = new BinaryFormatter();
        }

        public T GetSerializedObject(string filename) {
            if (File.Exists(filename)) {
                Stream inStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                T obj = (T) this.iformatter.Deserialize(inStream);
                inStream.Close();
                return obj;
            }
            return default(T);
        }

        public void SaveSerializedObject(T obj, string filename) {
            Stream outStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            this.iformatter.Serialize(outStream, obj);
            outStream.Close();
        }
    }
}