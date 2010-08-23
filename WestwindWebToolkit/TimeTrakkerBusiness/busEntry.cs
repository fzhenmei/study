using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Westwind.BusinessFramework.LinqToSql;
using System.Data.SqlClient;
using Westwind.Utilities;
using System.Data.Linq;

namespace TimeTrakker
{

    /// <summary>
    /// Business object related a time entry.
    /// </summary>
    //public class busEntry : TimeTrakkerBusinessBase<EntryEntity, TimeTrakkerContext>
    public class busEntry : BusinessObjectLinq<EntryEntity, TimeTrakkerContext>
    {

       
        /// <summary>
        /// Get open entries for a given user
        /// </summary>
        /// <param name="userPk"></param>
        /// <returns></returns>
        public IQueryable<EntryEntity> GetOpenEntries(int userPk)
        {
            IQueryable<EntryEntity> entries = 
                from entry in this.Context.EntryEntities
                where !entry.PunchedOut
                orderby entry.TimeIn
                select entry;

            // *** Add filter for User Pk - otherwise all open entries are returned
            if (userPk > 0)
                entries = entries.Where(entry => entry.UserPk == userPk);

            return entries;
        }

        /// <summary>
        /// Get All open entries
        /// </summary>
        /// <returns></returns>
        public IQueryable<EntryEntity> GetOpenEntries()
        {
            return this.GetOpenEntries(-1);
        }

        /// <summary>
        /// Gets a list of recent entries 
        /// </summary>
        /// <param name="userPk"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public IQueryable<EntryEntity> GetEntries(int userPk)
        {
            IQueryable<EntryEntity> q =
                from e in this.Context.EntryEntities
                where e.UserPk == userPk
                orderby e.TimeIn descending
                select e;
                        
            return q;
        }


        /// <summary>
        /// Returns a query of entries for a given project
        /// </summary>
        /// <param name="projectPk"></param>
        /// <returns></returns>
        public IQueryable<EntryEntity> GetEntriesByProject(int projectPk)
        {
            IQueryable<EntryEntity> q =
                from entry in this.Context.EntryEntities
                where entry.ProjectPk == projectPk 
                orderby entry.TimeIn descending
                select entry;
            
            return q;
        }


        /// <summary>
        /// Returns a query ready for the Time Sheet Report
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public IQueryable<EntryEntity> GetTimeSheetByClient(TimesheetReportParameters parms)
        {            
            IQueryable<EntryEntity> result =
                from entry in Context.EntryEntities
                where entry.TimeIn >= parms.FromDate && 
                      entry.TimeIn < parms.ToDate.Date.AddDays(1) &&
                      entry.PunchedOut &&
                      parms.Companies.Contains(entry.CustomerPk)
                orderby entry.ProjectEntity.Customer.Company, entry.ProjectEntity.ProjectName, entry.TimeIn 
                select entry;

            if (parms.BillType == "Unbilled")
                result = result.Where(e => !e.Billed);
            else if (parms.BillType == "Billed")
                result = result.Where(e => e.Billed);
            
            return result;
        }

   

#region   overridden CRUD operation
       
        
        /// <summary>
        /// Sets default time value
        /// </summary>
        /// <returns></returns>
        public override EntryEntity NewEntity()
        {
            EntryEntity entry = base.NewEntity();
            if (entry == null)            
                return null;

            
            entry.TimeIn = TimeUtils.RoundDateToMinuteInterval(DateTime.Now,
                                                                   App.Configuration.MinimumMinuteInterval,
                                                                   RoundingDirection.RoundUp);
            entry.TimeOut = App.MIN_DATE_VALUE;            
            
            return entry;
        }

        /// <summary>
        /// Fixes up times for Universal Time to the database
        /// </summary>
        /// <returns></returns>
        public override bool Save()
        {
            //if (this.Entity.TimeIn != null)
            //    this.Entity.TimeIn = this.Entity.TimeIn.ToUniversalTime();            
            //if (this.Entity.TimeOut != null)
            //    this.Entity.TimeOut = this.Entity.TimeOut.ToUniversalTime();
            //this.CalculateItemTotals();
            
            return base.Save();
        }

        /// <summary>
        /// Fixes up times for local time from the database
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public override EntryEntity Load(int pk)
        {
            if (base.Load(pk) == null)
                return null;

            //if (this.Entity.TimeIn <= App.MIN_DATE_VALUE)
            //    this.Entity.TimeIn = DateTime.Now;

            if (this.Entity.TimeOut <= App.MIN_DATE_VALUE)
            {
                this.Entity.TimeOut = DateTime.Now;
                this.RoundTimeValues();
            }

            return this.Entity;
        }


        /// <summary>
        /// Checks for empty title and time in values and associations for user, customer and project
        /// </summary>
        /// <returns></returns>
        protected override void OnValidate(EntryEntity entity)
        {            
            if (string.IsNullOrEmpty(entity.Title))
                this.ValidationErrors.Add("The title is required","txtTitle");

            if (entity.TimeIn <= App.MIN_DATE_VALUE)            
                this.ValidationErrors.Add("Time and/or date value is invalid","txtTimeIn");

            if (entity.CustomerPk < 1)
                this.ValidationErrors.Add("A customer must be associated with this entry", "txtCustomerpk");

            if (entity.ProjectPk < 1)
                this.ValidationErrors.Add("A project must be associated with this entry", "txtProjectPk");

            if (entity.UserPk < 1)
                this.ValidationErrors.Add("A user must be associated with this entry", "txtUserPk");
        }
        

        /// <summary>
        /// Punches in a new entry by setting punch in time
        /// </summary>
        /// <returns></returns>
        public bool PunchIn()
        {
            this.Entity.PunchedOut = false;

            if (this.Entity.TimeIn <= App.MIN_DATE_VALUE)
                this.Entity.TimeIn = DateTime.Now;

            return this.Save();
        }

        /// <summary>
        /// Punches in a new entry by setting punch in time
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool PunchIn(EntryEntity entry)
        {
            if (entry == null)
                entry = this.Entity;

            entry.PunchedOut = false;

            if ( entry.TimeIn <= App.MIN_DATE_VALUE )
                entry.TimeIn = DateTime.Now;

            if (entry == this.Entity)
                return this.Save();
            else
                return this.Save(entry);
        }


        /// <summary>
        /// Punches out an individual entry and saves it
        /// </summary>
        /// <returns></returns>
        public bool PunchOut()
        {
            return this.PunchOut(null);
        }

        /// <summary>
        /// Punches out an individual entry and saves it
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool PunchOut(EntryEntity entry)
        {
            if (entry == null)
                entry = this.Entity;

            entry.PunchedOut = true;

            if (entry.TimeOut == null || entry.TimeOut < entry.TimeIn)
                entry.TimeOut = DateTime.Now;

            this.CalculateItemTotals();

            if (entry == this.Entity)
                return this.Save();
            else
                return this.Save(entry);
        }

        /// <summary>
        /// Closes the current entry and saves it
        /// </summary>
        /// <returns></returns>
        public bool LeaveOpen(EntryEntity entity)
        {
            bool IsThisEntity = false;
            if (entity == null)
            {
                IsThisEntity = true;
                entity = this.Entity;
            }
            entity.PunchedOut = false;
            entity.TimeOut = App.MIN_DATE_VALUE;
            entity.TotalHours = 0.00M;
            entity.ItemTotal = 0.00M;

            if (IsThisEntity)
                return this.Save();
            
            return this.Save(entity);
        }



        /// <summary>
        /// Marks a given set of entries as billed based on an array of PKs
        /// passed into the method.
        /// </summary>
        /// <param name="pks">Array of integer Pks</param>
        /// <returns></returns>
        public bool MarkAsBilled(int[] pks, bool markAsBilled)
        {            
            if (pks == null || pks.Length == 0)
                return true;

            // *** Easiest to use string query to apply
            // *** multiple updates to records rather than
            // *** loading multiple LINQ loads and deletes
            StringBuilder sb = new StringBuilder("update " + this.TableInfo.Tablename + " set billed=@Mark where pk in (");
            
            foreach(int pk in pks)
            {
                sb.Append(pk.ToString() + ",");
            }
            // *** Strip off last comma
            sb.Length--;
            
            sb.Append(")");

            if (this.ExecuteNonQuery(sb.ToString(),new SqlParameter("@Mark",markAsBilled))  < 0)
               return false;

            return true;
        }
        /// <summary>
        /// Marks a given sent of entries as billed based on a list of
        /// entries.
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public bool MarkAsBilled(IQueryable<EntryEntity> entries)
        {
            return this.MarkAsBilled(entries.Select(e => e.Pk).ToArray<int>(),true);
        }
        public bool UnMarkAsBilled(IQueryable<EntryEntity> entries)
        {
            return this.MarkAsBilled(entries.Select(e => e.Pk).ToArray<int>(), false);
        }
#endregion

        #region Utility functions

        /// <summary>
        /// Utility function that converts a date time entry value from
        /// a date and time string to a DateTime value
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public DateTime GetTimeFromStringValues(string Date, string Time)
        {
            DateTime val = App.MIN_DATE_VALUE;
            DateTime.TryParse(Date + " " + Time,out val);

            return val;
        }
        public DateTime GetTimeFromStringValues(DateTime Date, string Time)
        {
            DateTime val = App.MIN_DATE_VALUE;
            DateTime.TryParse(Date.ToShortDateString() + " " + Time, out val);

            return val;
        }

        /// <summary>
        /// Calculates Item and Rate totals and sets it on the passed entry object
        /// </summary>
        public void CalculateItemTotals(EntryEntity Entry)
        {
            if (Entry == null)
                Entry = this.Entity;                       

            if (Entry.TimeIn == null || 
                Entry.TimeOut== null || 
                Entry.TimeOut < Entry.TimeIn)
                Entry.TotalHours = 0.00M;
            else if ( Entry.TimeOut > App.MIN_DATE_VALUE && 
                      Entry.TimeIn > App.MIN_DATE_VALUE)
                Entry.TotalHours = (decimal)Entry.TimeOut.Subtract(Entry.TimeIn).TotalHours;

            Entry.ItemTotal = Entry.TotalHours * Entry.Rate;
        }


        /// <summary>
        /// Calculates Item and Rate totals. This version works off the internal Entity object
        /// </summary>
        public void CalculateItemTotals()
        {
            this.CalculateItemTotals(null);
        }

        /// <summary>
        /// Adjusts the time values for rounding conditions
        /// </summary>
        public void RoundTimeValues()
        {
            if (this.Entity.TimeIn > App.MIN_DATE_VALUE)
                this.Entity.TimeIn = 
                    TimeUtils.RoundDateToMinuteInterval(this.Entity.TimeIn,
                        App.Configuration.MinimumMinuteInterval,
                        RoundingDirection.RoundUp);
            if (this.Entity.TimeOut > App.MIN_DATE_VALUE)
                this.Entity.TimeOut = 
                    TimeUtils.RoundDateToMinuteInterval(this.Entity.TimeOut,
                       App.Configuration.MinimumMinuteInterval,
                       RoundingDirection.RoundUp);
        }
        

        ///// <summary>
        ///// Converts a fractional hour value like 1.25 to 1:15  hours:minutes format
        ///// </summary>
        ///// <returns></returns>
        //public string FractionalHoursToString(decimal hours, string format)
        //{

        //    if (string.IsNullOrEmpty(format))
        //        format = "{0}:{1}";

        //    int hoursWhole = (int)hours;
        //    decimal fraction;
        //    if (hoursWhole == 0)
        //        fraction = hours;
        //    else
        //        fraction = decimal.Remainder(hours, (decimal)hoursWhole);

        //    int minutes = (int)(fraction * 60);

        //    return string.Format(format, hoursWhole, minutes);
        //}
        //public string FractionalHoursToString(decimal hours)
        //{
        //    return this.FractionalHoursToString(hours, null);
        //}
        #endregion 
    }


    public class TimesheetReportParameters
    {
        public DateTime FromDate = DateTime.Now.AddMonths(-1);
        public DateTime ToDate = DateTime.Now;
        public string BillType = "Unbilled";
        public bool MarkAsBilled;
        public bool UnmarkAsBilled;
        public bool GenerateXml;
        public bool SummaryReport;
        public string ReportType = "TimeSheetClient";
        public List<int> Companies = new List<int>();
    }

    public class CustomerListResult
    {
        public string Company;
        public int Pk;
    }
}
