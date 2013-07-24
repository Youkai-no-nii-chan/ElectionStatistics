using System.Linq;

namespace ElectionInfo.Model
{
    public abstract class Repository<TEntity> where TEntity : class 
    {
        protected Repository(ModelContext context)
        {
            Context = context;
        }

        protected ModelContext Context { get; private set; }

        public IQueryable<TEntity> Items
        {
            get { return Context.Set<TEntity>(); }
        }

        protected virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }
    }
}