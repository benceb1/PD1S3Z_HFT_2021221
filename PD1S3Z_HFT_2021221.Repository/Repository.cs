﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext ctx;
        public Repository(DbContext ctx)
        {
            this.ctx = ctx;
        }
        public IQueryable<T> GetAll()
        {
            return ctx.Set<T>();
        }

        public abstract T GetOne(int id);

        public abstract T Insert(T entity);

        public abstract bool Remove(int id);

        public abstract void Update(T entity);
    }
}
