using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Web.Script.Serialization;

namespace RedisCache
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (!IsKeyExits("Name"))
            {
                SetData("Name", "Vikash Kumar");
            }

            
            string getdata = GetData<string>("Name");
            AppendData("Name", "Sah");
            Console.WriteLine(getdata);
            Console.WriteLine(IsKeyDelete("Name"));
            Console.ReadLine();
        }
        public static void SetData<T>(string key, T data)
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost:6379,password=vpvsedyrs"))
            {
                IDatabase db = redis.GetDatabase();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;

                db.StringSet(key, json_serializer.Serialize(data));

                redis.Close();
            }
        }
        public static void AppendData<T>(string key, T data)
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost:6379,password=vpvsedyrs"))
            {
                IDatabase db = redis.GetDatabase();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                db.StringAppend(key, json_serializer.Serialize(data));

                redis.Close();
            }
        }
        public static T GetData<T>(string key)
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost:6379,password=vpvsedyrs"))
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
            using (var redis = ConnectionMultiplexer.Connect("localhost:6379,password=vpvsedyrs"))
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
            using (var redis = ConnectionMultiplexer.Connect("localhost:6379,password=vpvsedyrs"))
            {
                IDatabase db = redis.GetDatabase();
                Isexits = db.KeyDelete(key);

                redis.Close();
            }
            return Isexits;
        }
    }



}
