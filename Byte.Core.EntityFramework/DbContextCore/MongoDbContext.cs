using Blueshift.EntityFrameworkCore.MongoDB.Annotations;
using Blueshift.EntityFrameworkCore.MongoDB.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Byte.Core.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using MongoDB.Driver.Core.Operations;

namespace Byte.Core.EntityFramework.IDbContext
{
    //[MongoDatabase("ZxwMongoDb")]
    public class MongoDbContext : BaseDbContext, IMongoDbContext
    {

        private  MongoClient _dbClient = null;
        private  IMongoDatabase _database = null;
        public MongoDbContext(DbContextOption option) : base(option)
        {
            var mongoUrl = new MongoUrl(Option.ConnectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            //var mongoClient = new MongoClient(settings);
            //optionsBuilder.UseMongoDb(Option.ConnectionString);
            _dbClient = new MongoClient(settings);
            _database = _dbClient.GetDatabase(mongoUrl.DatabaseName);
        }
        public MongoDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }

        public MongoDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var mongoUrl = new MongoUrl(Option.ConnectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            //var mongoClient = new MongoClient(settings);
            //optionsBuilder.UseMongoDb(Option.ConnectionString);
             _dbClient = new MongoClient(settings);
            _database=_dbClient.GetDatabase(mongoUrl.DatabaseName);
            base.OnConfiguring(optionsBuilder);
        }



        public override int Add<T>(T entity, bool withTrigger = false)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
                collection.InsertOne(entity);
            return 1;
        }
        public override async Task<int> AddAsync<T>(T entity, bool withTrigger = false) {

            var collection = _database.GetCollection<T>(typeof(T).Name);
            await collection.InsertOneAsync(entity);
            return 1;
        }

        public override async Task<int> UpdateAsync<T, TKey>(T entity, bool withTrigger = false)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            //await collection.UpdateOneAsync(entity);

            // Define the filter object
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);

            // Replace the document
            var result = await collection.ReplaceOneAsync(filter, entity);
            return 1;
        }
        public override Task<int> DeleteAsync<T, TKey>(TKey key, bool withTrigger = false)
        {
            return base.DeleteAsync<T, TKey>(key, withTrigger);
        }



        public IFindFluent<T, T>  GetIFindFluent(Expression<Func<T, bool>> conditions = null)
        {
            return _database.GetCollection<T>(typeof(T).Name).Find(conditions);
        }

        public  IAsyncCursor<T> GetIAsyncCursor(Expression<Func<T, bool>> conditions = null)
        {
            return  _database.GetCollection<T>(typeof(T).Name).FindSync(conditions);
        }

        public override IQueryable<T> GetIQueryable<T>(Expression<Func<T, bool>> where = null, bool asNoTracking = false)
        {
           
        
             IQueryable <T> queryable = _database.GetCollection<T>(typeof(T).Name).AsQueryable();
            if (where != null)
            {
                queryable = queryable.Where(where);
            }

            if (!asNoTracking)// 实体查询,不能new 先赋值(不追踪)
            {
                queryable = queryable.AsNoTracking();
            }

            return queryable;
        }



        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> conditions = null)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            if (conditions != null)
            {
                return await collection.FindSync(conditions).ToListAsync();
            }
            return await collection.FindSync(_ => true).ToListAsync();
        }







        public override DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }
  

        //public override List<DataTable> GetDataTables(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
