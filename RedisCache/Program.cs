using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Web.Script.Serialization;

namespace RedisCache
{
    class Program
    {
        public static string RedisConnetion = ConfigurationManager.AppSettings["RedisCache"];
        public static string key = "Name";
        public static string value = "Hello World";
        public static void Main(string[] args)
        {
            if (!IsKeyExits(key))
            {
                SetData(key, value);
            }
            string getdata = GetData<string>(key);
            Console.WriteLine(getdata);
            Console.WriteLine(IsKeyDelete(key));
            Console.ReadLine();
        }
        public static void SetData<T>(string key, T data)
        {
            using (var redis = ConnectionMultiplexer.Connect(RedisConnetion))
            {
                IDatabase db = redis.GetDatabase();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;

                db.StringSet(key, json_serializer.Serialize(data));

                redis.Close();
            }
        }

        public static T GetData<T>(string key)
        {
            using (var redis = ConnectionMultiplexer.Connect(RedisConnetion))
            {
                try
                {
                    IDatabase db = redis.GetDatabase();
                    var res = db.StringGet(key);

                    redis.Close();
                    if (res.IsNull)
                        return default(T);
                    else
                        return JsonConvert.DeserializeObject<T>(res);
                }
                catch
                {
                    return default(T);
                }

            }
        }
        public static bool IsKeyExits(string key)
        {
            bool Isexits = false;
            using (var redis = ConnectionMultiplexer.Connect(RedisConnetion))
            {
                IDatabase db = redis.GetDatabase();

                Isexits = db.KeyExists(key);
                redis.Close();
            }
            return Isexits;
        }
        public static bool IsKeyDelete(string key)
        {
            bool Isexits = false;
            using (var redis = ConnectionMultiplexer.Connect(RedisConnetion))
            {
                IDatabase db = redis.GetDatabase();
                Isexits = db.KeyDelete(key);

                redis.Close();
            }
            return Isexits;
        }
    }
}
