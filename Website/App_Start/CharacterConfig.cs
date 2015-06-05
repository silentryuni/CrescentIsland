﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using CrescentIsland.Website.Models;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Utilities;

namespace CrescentIsland.Website
{
    public class CharacterManager : IDisposable
    {
        private bool _disposed;

        public CharacterManager(DbContext context)
        {
            Context = context;
        }

        protected internal DbContext Context { get; set; }

        public static CharacterManager Create(IdentityFactoryOptions<CharacterManager> options, IOwinContext context)
        {
            var manager = new CharacterManager(context.Get<ApplicationDbContext>());

            return manager;
        }

        protected virtual async Task<Character> GetCharacterAggregateAsync(IQueryable<Character> entitySet, Expression<Func<Character, bool>> filter)
        {
            var character = await entitySet.FirstOrDefaultAsync(filter).WithCurrentCulture();
            return character;
        }

        public virtual Task<Character> FindCharacterAsync(string charName)
        {
            ThrowIfDisposed();
            if (charName == null)
                throw new ArgumentNullException("charName");

            IQueryable<Character> entityset = Context.Set<Character>();
            return GetCharacterAggregateAsync(entityset, c => c.CharacterName.ToUpper() == charName.ToUpper());
        }


        #region Dispose
        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }
        // <summary>
        /// Dispose this object
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
        /// <summary>
        /// When disposing, actually dipose the store
        /// 
        /// </summary>
        /// <param name="disposing"/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this._disposed)
                return;
            this.Context.Dispose();
            this._disposed = true;
        }
        #endregion
    }
}
