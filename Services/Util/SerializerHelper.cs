using System;
using System.Globalization;
using System.IO;
using FindMyFamilies.Data;
using ProtoBuf;

namespace FindMyFamilies.Util {
    /// <summary>
    /// Summary description for SerializerHelper.
    /// </summary>
    public class SerializerHelper {
        public SerializerHelper() {
        }

        public static void Serialize(string filePath, object data) {
            using (var file = System.IO.File.Create(filePath)) {
                Serializer.Serialize(file, data);
            }
        }

       public static object DeSerialize(string filePath) {
           object data;

            using (var file = System.IO.File.OpenRead(filePath)) {
                data = Serializer.Deserialize<Type>(file);
            }
           return data;
       }



//            string filePath = FilePathHelper.GetTempPath() + "\\Persons_" + session.CurrentPerson.Id + ".bin";
//
//            if (File.Exists(filePath)) {
//                File.Delete(filePath);
//            }
//
//            SerializerHelper.Serialize(filePath, persons["KW71-758"]);
//
//            PersonsDO personsDO1;
//            using (var file = System.IO.File.OpenRead(filePath)) {
//                personsDO1 = Serializer.Deserialize<PersonsDO>(file);
//            }
//
//            PersonsDO personsDO3 = (PersonsDO)SerializerHelper.DeSerialize(filePath);
//
//            PersonsDO personsDO2;
//            using (var file = System.IO.File.OpenRead(filePath)) {
//                personsDO1 = Serializer.Deserialize<PersonsDO>(file);
//            }
//
//            using (var file = System.IO.File.Create(filePath)) {
//                Serializer.Serialize(file, persons["KW71-758"]);
//            }
//
//            using (var file = System.IO.File.Create(filePath)) {
//                Serializer.Serialize(file, personsDO);
//            }

   }
}