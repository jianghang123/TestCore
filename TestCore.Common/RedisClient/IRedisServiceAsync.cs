﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestCore.Common.RedisClient
{
    public partial interface IRedisService
    {
        #region Async Methods

        #region Basic

        /// <summary>
        /// 自增（将值加1）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>返回自增之后的值</returns>
        Task<long> IncrementAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 在原有值上加操作(long)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待加值（long）</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>返回加后结果</returns>
        Task<long> IncrementAsync(RedisKey key, long value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 在原有值上加操作(double)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待加值（double）</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<double> IncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 自减（将值减1）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> DecrementAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 在原有值上减操作(long)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待减值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>返回减后结果</returns>
        Task<long> DecrementAsync(RedisKey key, long value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 在原有值上减操作(double)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待减值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>返回减后结果</returns>
        Task<double> DecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 重命名一个Key，值不变
        /// </summary>
        /// <param name="oldKey">旧键</param>
        /// <param name="newKey">新键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> KeyRenameAsync(RedisKey oldKey, RedisKey newKey, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 获取Key对应Redis的类型
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<RedisType> KeyTypeAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 判断Key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> KeyExistsAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default(CancellationToken));


        #endregion

        #region String
         
        /// <summary>
        /// Item Set操作（包括新增（key不存在）/更新(如果key已存在)）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> ItemSetAsync<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
         
        /// <summary>
        /// Item Set操作（包括新增/更新）,同时可以设置过期时间段
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="expiresAt">过期时间段</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> ItemSetAsync<T>(RedisKey key, T val, DateTime expiresAt, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
         
        /// <summary>
        /// Item Set操作（包括新增/更新）,同时可以设置过期时间点
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="expiresIn">过期时间点</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>true 成功 false 失败</returns>
        Task<bool> ItemSetAsync<T>(RedisKey key, T val, TimeSpan expiresIn, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
         
        /// <summary>
        /// Item Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>如果key存在，找到对应Value,如果不存在，返回默认值.</returns>
        Task<T> ItemGetAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));
         
        /// <summary>
        /// Item Get 操作（获取多条）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ItemGetAsync<T>(IEnumerable<RedisKey> keys, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));
         
        /// <summary>
        /// Sting Del 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>True if the key was removed. else false</returns>
        Task<bool> ItemRemoveAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
         
        /// <summary>
        /// Sting Del 操作（删除多条）
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> ItemRemoveAsync(IEnumerable<RedisKey> keys, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #region List

        /// <summary>
        /// List Insert Left 操作，将val插入pivot位置的左边
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">待插入值</param>
        /// <param name="pivot">参考值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>返回插入左侧成功后List的长度 或 -1 表示pivot未找到.</returns>
        Task<long> ListInsertLeftAsync<T>(RedisKey key, T val, T pivot, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Insert Right 操作，，将val插入pivot位置的右边
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">待插入值</param>
        /// <param name="pivot">参考值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns> 返回插入右侧成功后List的长度 或 -1 表示pivot未找到.</returns>
        Task<long> ListInsertRightAsync<T>(RedisKey key, T val, T pivot, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Left Push  操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Push操作之后，List的长度</returns>
        Task<long> ListLeftPushAsync<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Left Push 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> ListLeftPushRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Right Push 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> ListRightPushAsync<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Right Push 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> ListRightPushRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Left Pop 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<T> ListLeftPopAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Right Pop 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<T> ListRightPopAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>the number of removed elements</returns>
        Task<long> ListRemoveAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Remove All 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> ListRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> ListCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Get By Index 操作 (Index 0表示左侧第一个,-1表示右侧第一个)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<T> ListGetByIndexAsync<T>(RedisKey key, long index, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Get All 操作(注意：从左往右取值)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ListGetAllAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Get Range 操作(注意：从左往右取值)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startIndex">开始索引 从0开始</param>
        /// <param name="stopIndex">结束索引 -1表示结尾</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ListGetRangeAsync<T>(RedisKey key, long startIndex, long stopIndex, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// List Expire At 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> ListExpireAtAsync(RedisKey key, DateTime expireAt, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 设置List缓存过期
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> ListExpireInAsync(RedisKey key, TimeSpan expireIn, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region Set

        /// <summary>
        /// Set Add 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>如果值不存在，则添加到集合，返回True否则返回False</returns>
        Task<bool> SetAddAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Add 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>添加值到集合，如果存在重复值，则不添加，返回添加的总数</returns>
        Task<long> SetAddRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>如果值从Set集合中移除返回True，否则返回False</returns>
        Task<bool> SetRemoveAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SetRemoveRangeAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Remove All 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns> True if the key was removed.</returns>
        Task<bool> SetRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Combine 操作(可以求多个集合并集/交集/差集)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">多个集合的key<see cref="IEnumerable{T}"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SetCombineAsync<T>(IEnumerable<RedisKey> keys, SetOperation operation, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Combine 操作(可以求2个集合并集/交集/差集)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="firstKey">第一个Set的Key</param>
        /// <param name="sencondKey">第二个Set的Key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>list with members of the resulting set.</returns>
        Task<IEnumerable<T>> SetCombineAsync<T>(RedisKey firstKey, RedisKey sencondKey, SetOperation operation, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Combine And Store In StoreKey Set 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="storeKey">新集合Id</param>
        /// <param name="soureKeys">多个集合的key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SetCombineStoreAsync<T>(RedisKey storeKey, IEnumerable<RedisKey> soureKeys, SetOperation operation, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Set Combine And Store In StoreKey Set 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="storeKey">新集合Id</param>
        /// <param name="firstKey">第一个集合Key</param>
        /// <param name="secondKey">第二个集合Key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SetCombineStoreAsync<T>(RedisKey storeKey, RedisKey firstKey, RedisKey secondKey, SetOperation operation, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Move 操作（将元素从soure移动到destination）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceKey">数据源集合</param>
        /// <param name="destinationKey">待添加集合</param>
        /// <param name="val">待移动元素</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SetMoveAsync<T>(RedisKey sourceKey, RedisKey destinationKey, T val, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Exists 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SetExistsAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///  Set Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SetCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SetGetAllAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set Expire At 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SetExpireAtAsync(RedisKey key, DateTime expireAt, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Set Expire In 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SetExpireInAsync(RedisKey key, TimeSpan expireIn, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #region SortedSet

        /// <summary>
        /// SortedSet Add 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SortedSetAddAsync<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// SortedSet Add 操作（多条）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">待添加值集合<see cref="SortedSetEntry"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<long> SortedSetAddAsync(RedisKey key, IEnumerable<SortedSetEntry> vals, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// SortedSet Increment Score 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Incremented score</returns>
        Task<double> SortedSetIncrementScoreAsync<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// SortedSet Decrement Score 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Decremented score</returns>
        Task<double> SortedSetDecrementScoreAsync<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SortedSetRemoveAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Remove 操作(删除多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SortedSetRemoveRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Remove 操作(根据索引区间删除,索引值按Score由小到大排序)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="startIndex">开始索引，0表示第一项</param>
        /// <param name="stopIndex">结束索引，-1标识倒数第一项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SortedSetRemoveAsync(RedisKey key, long startIndex, long stopIndex, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Sorted Remove 操作(根据Score区间删除，同时根据exclue<see cref="Exclude"/>排除删除项)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startScore">开始Score</param>
        /// <param name="stopScore">结束Score</param>
        /// <param name="exclue">排除项<see cref="Exclude"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>the number of elements removed.</returns>
        Task<long> SortedSetRemoveAsync(RedisKey key, double startScore, double stopScore, Exclude exclue, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Remove All 操作(删除全部)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>thre reuslt of all sorted set removed</returns>
        Task<bool> SortedSetRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Trim 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="size">保留条数</param>
        /// <param name="order">根据order<see cref="Order"/>来保留指定区间，如保留前100名，保留后100名</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>移除元素数量</returns>
        Task<long> SortedSetTrimAsync(RedisKey key, long size, Order order = Order.Descending, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SortedSetCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Exists 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SortedSetExistsAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// SortedSet Pop Min Score Element 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<T> SortedSetGetMinByScoreAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// SortedSet Pop Max Score Element 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<T> SortedSetGetMaxByScoreAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Get Page List 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SortedSetGetPageListAsync<T>(RedisKey key, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Get Page List 操作(根据分数区间)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startScore">开始值</param>
        /// <param name="stopScore">停止值</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="exclude">排除规则<see cref="Exclude"/>,默认为None</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SortedSetGetPageListAsync<T>(RedisKey key, double startScore, double stopScore, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, Exclude exclude = Exclude.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Get Page List With Score 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<SortedSetEntry[]> SortedSetGetPageListWithScoreAsync(RedisKey key, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Get Page List With Score 操作(根据分数区间)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startScore">开始值</param>
        /// <param name="stopScore">停止值</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="exclude">排除规则<see cref="Exclude"/>,默认为None</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<SortedSetEntry[]> SortedSetGetPageListWithScoreAsync(RedisKey key, double startScore, double stopScore, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, Exclude exclude = Exclude.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// SortedSet Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SortedSetGetAllAsync<T>(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Combine And Store 操作
        /// </summary>
        /// <param name="storeKey">存储SortedKey</param>
        /// <param name="combineKeys">待合并的Key集合<see cref="Array"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SortedSetCombineAndStoreAsync(RedisKey storeKey, RedisKey[] combineKeys, SetOperation setOperation, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Combine And Store 操作
        /// </summary>
        /// <param name="storeKey">存储SortedKey</param>
        /// <param name="combineKeys">待合并的Key集合<see cref="IEnumerable{T}"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> SortedSetCombineAndStoreAsync(RedisKey storeKey, IEnumerable<RedisKey> combineKeys, SetOperation setOperation, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        ///  Sorted Set Expire At DeteTime 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SortedSetExpireAtAsync(RedisKey key, DateTime expiresAt, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sorted Set Expire In TimeSpan 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> SortedSetExpireInAsync(RedisKey key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region Hash

        /// <summary>
        /// Hash Set 操作（新增/更新）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项Id</param>
        /// <param name="val">值</param>
        /// <param name="when">依据value的执行条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> HashSetAsync<T>(RedisKey key, RedisValue hashField, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Set 操作（新增/更新多条）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值集合<see cref="HashEntry"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task HashSetRangeAsync(RedisKey key, IEnumerable<HashEntry> hashFields, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Remove 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> HashRemoveAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Remove 操作(删除多条)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项集合<see cref="Array"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> HashRemoveAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Remove 操作(删除多条)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项集合<see cref="IEnumerable{T}"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> HashRemoveAsync(RedisKey key, IEnumerable<RedisValue> hashFields, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Remove All 操作(删除全部)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> HashRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Exists 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> HashExistsAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<long> HashCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<T> HashGetAsync<T>(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Hash Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashFields">hash项集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> HashGetAsync<T>(RedisKey key, IEnumerable<RedisValue> hashFields, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IEnumerable<T>> HashGetAllAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Get All 操作 (返回HashEntry)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///  Hahs Expire At DeteTime 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> HashExpireAtAsync(RedisKey key, DateTime expiresAt, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Hash Expire In TimeSpan 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> HashExpireInAsync(RedisKey key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.DemandMaster, CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region Transition

        /// <summary>
        /// Redis 事务操作
        /// </summary>
        /// <param name="action">执行命令</param>
        /// <param name="asyncObjec">同步对象</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> TransactionAsync(Action<ITransaction> action, object asyncObjec = null, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #endregion
    }
}
