# RedisCache
RedisCache Console Application

Download Redis from here https://github.com/dmajkic/redis/downloads and extract the Folder and go to 
C:\redis-2.4.5-win32-win64\64bit accoding to your windows (32bit/64bit) run redis-server and redis-cli

Redis Default port is 6379.

Check redi is runing using ping command and get result pong.

Redis Command using in c#

GET key: This key gets the value O(1)
SET key “value”: This key sets a value for a key O(1)
SETNX key “value”: If key does Not eXist this will sets a value against it O(1)
GETSET key “value”: Get the old value and sets a new value O(1)
KeyExists key: Check key exists or not.
KeyDelete key : exist key delete from redis.
