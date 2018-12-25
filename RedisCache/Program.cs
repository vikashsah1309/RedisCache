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
        
        /// <summary>
        /// SET key “value”: This key sets a value for a key O(1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
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
       
        /// <summary>
        /// GET key: This key gets the value O(1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// GETSET key “value”: Get the old value and sets a new value O(1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool GetSetData<T>(string key, T data)
        {
            bool Isexits = false;
            using (var redis = ConnectionMultiplexer.Connect(RedisConnetion))
            {
                IDatabase db = redis.GetDatabase();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                db.StringGetSet(key, json_serializer.Serialize(data));

                redis.Close();
            }
            return Isexits;
        }
       
        /// <summary>
        /// SETNX key “value”: If key does Not eXist this will sets a value against it O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SETNXData<T>(string key, T data)
        {
            bool Isexits = false;
            using (var redis = ConnectionMultiplexer.Connect(RedisConnetion))
            {
                IDatabase db = redis.GetDatabase();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;

                if (db.StringSet(key, json_serializer.Serialize(data), TimeSpan.MaxValue, When.NotExists))
                {
                    var val = db.StringGet(key);
                    Console.WriteLine("StringGet({0}) value is {1}", key, val);
                }
                else
                {
                    //always goes here
                    Console.WriteLine("Value already exist");
                }
                redis.Close();
            }
            return Isexits;
        }
       
        /// <summary>
        /// KeyExists : Check key exit or not.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// KeyDelete :  exited key delete from redis.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
