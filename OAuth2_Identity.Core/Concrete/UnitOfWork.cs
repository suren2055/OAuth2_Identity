namespace OAuth2_Identity.Core.Concrete;

  public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        Task<int> CommitAsync();
    }

    public sealed class UnitOfWork : IUnitOfWork
    {
        #region Variables

        /// <summary>
        /// The DbContext.
        /// </summary>
        private EFDBContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(EFDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves all pending changes.
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            // Save changes with the default options
            return _context.SaveChanges();
        }

        /// <summary>
        /// Saves all pending changes.
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public async Task<int> CommitAsync()
        {
            // Save changes with the default options
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the current object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        #endregion
    }