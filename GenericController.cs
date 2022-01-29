using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using GenericController.Interfaces;
using GenericController;

namespace GenericController_ini.Controllers
{
    public class GenericController<TEntity> : ControllerBase where TEntity : class, IEntity
    {

        protected DbAplication _context { get; set; }
        protected DbSet<TEntity> _dbSet;

        public GenericController(DbAplication context)
        {

            _context = context;
            _dbSet = context.Set<TEntity>();

        }

        [HttpGet]
        public async Task<ActionResult<List<TEntity>>> Get()
        {
            return await _dbSet.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> Get(int id)
        {
            var item = await _dbSet.FirstAsync(x => x.Id.Equals(id));


            if (item == null) { return NotFound(); }

            return item;
        }


        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Post(TEntity item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        [HttpPut]
        public virtual async Task<ActionResult<TEntity>> Put(TEntity item)
        {
            _dbSet.Attach(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var itemToRemove = await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (itemToRemove == null)
            {
                return NotFound();
            }

            _context.Remove(itemToRemove);
            await _context.SaveChangesAsync();
            return Ok("Item ("+id+"): Excluido com sucesso");
        }
    }
    
}
