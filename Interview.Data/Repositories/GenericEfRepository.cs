namespace Interview.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using System.Data.Entity;
    using System.Linq.Expressions;

    public class GenericEfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext dbContext;
        private IDbSet<TEntity> entitySet;

        public GenericEfRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<TEntity>();
        }

        public IDbSet<TEntity> EntitySet
        {
            get { return this.entitySet; }
        }

        public IEnumerable<TEntity> All()
        {
            return this.entitySet;
        }

        public TEntity Find(object id)
        {
            return this.entitySet.Find(id);
        }

        public IEnumerable Where(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> query = dbContext.Set<TEntity>().Where(predicate);
            return query;
        }

        public void Add(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Remove(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void Remove(object id)
        {
            var entity = this.Find(id);
            this.Remove(entity);
            
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        private TEntity ChangeState(TEntity entity, EntityState state)
        {
            var entry = this.dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.entitySet.Attach(entity);
            }

            entry.State = state;
            return entity;
        }
    }
}