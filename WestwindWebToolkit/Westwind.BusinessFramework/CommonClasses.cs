using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Westwind.BusinessFramework
{
    /// <summary>
    /// Contains public options that can be set to affect how
    /// the business object operates
    /// </summary>
    public class BusinessObjectOptions
    {
        /// <summary>
        /// Determines whether exceptions are thrown on errors
        /// or whether error messages are merely set.
        /// </summary>
        public bool ThrowExceptions = false;

        /// <summary>
        /// Determines how LINQ is used for object tracking. 
        /// 
        /// In connected mode all changes are tracked until SubmitChanges or Save
        /// is called. Save() reverts to calling SubmitChanges.
        /// 
        /// In disconnected mode a new context is created for each data operation
        /// and save uses Attach to reattach to a context.
        /// 
        /// Use Connected for much better performance use disconnected if you
        /// prefer atomic operations in the database with individual entities.
        /// </summary>
        public TrackingModes TrackingMode = TrackingModes.Connected;

        /// <summary>
        /// Optional Connection string that is used with the data context
        /// 
        /// Note: This property should be set in the constructor/Initialize of the
        /// business object. 
        /// 
        /// If blank the default context connection string is used.
        /// </summary>
        public string ConnectionString = "";


        /// <summary>
        /// Determines the default Conflict Resolution mode for changes submitted
        /// to the context.
        /// </summary>
        public ConflictResolutionModes ConflictResolutionMode = ConflictResolutionModes.ForceChanges;
    }

    /// <summary>
    /// Determines how LINQ Change Tracking is applied
    /// </summary>
    public enum TrackingModes
    {
        /// <summary>
        /// Uses a LINQ connected data context for change management
        /// whenever possible. Save and SubmitChanges operation is used
        /// to persist changes. In general this provides better performance
        /// for change tracking.
        /// </summary>
        Connected,

        /// <summary>
        /// Creates a new DataContext for each operation and performs .Save 
        /// operations by reconnecting to the DataContext.
        /// </summary>
        Disconnected
    }

    public enum ResultListTypes
    {
        DataReader,
        DataTable,
        DataSet
    }

    /// <summary>
    /// Determines how conflicts on SubmitChanges are handled.
    /// </summary>
    public enum ConflictResolutionModes
    {
        /// <summary>
        /// No Conflict resolution - nothing is done when conflicts
        /// occur. You can check Context.ChangeConflicts manually
        /// </summary>
        None,
        /// <summary>
        /// Forces all changes to get written. Last one wins strategy
        /// </summary>
        ForceChanges,
        /// <summary>
        /// Aborts all changes and updates the entities with the values
        /// from the database.
        /// </summary>
        AbortChanges,
        /// <summary>
        /// Writes all changes that are not in conflict. Updates entities
        /// with values from the data.
        /// </summary>
        //WriteNonConflictChanges
    }

}
