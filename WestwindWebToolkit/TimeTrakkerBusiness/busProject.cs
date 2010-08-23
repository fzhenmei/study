using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Westwind.BusinessFramework.LinqToSql;

namespace TimeTrakker
{
    public class busProject : BusinessObjectLinq<ProjectEntity,TimeTrakkerContext>
    {
        /// <summary>
        /// Gets a list of all the open projects
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProjectListResult> GetOpenProjects()
        {
            return
                from p in this.Context.ProjectEntities
                where p.Status != (int) ProjectStatusTypes.Completed
                orderby p.ProjectName
                select new ProjectListResult() { Pk = p.Pk, ProjectName = p.ProjectName };                
        }

        /// <summary>
        /// Returns a list of projects for a given customer that are
        /// still active and open. 
        /// </summary>
        /// <param name="customerPk"></param>
        /// <returns></returns>
        public IQueryable<ProjectEntity> GetProjectsForCustomer(int customerPk)
        {
            return
                from p in this.Context.ProjectEntities
                where p.Status != (int)ProjectStatusTypes.Completed && 
                      p.CustomerPk == customerPk 
                orderby p.ProjectName
                select p;
        }

        /// <summary>
        /// Returns all projects
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProjectEntity> GetAllProjects()
        {
            return
                from p in this.Context.ProjectEntities                
                orderby p.ProjectName
                select p;             
        }

        /// <summary>
        /// Provides a filtered view of projects 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public IQueryable<ProjectEntity> GetFilteredProjects(ProjectQueryFilter filter, string companyName)
        {
            IQueryable<ProjectEntity> projectList = 
                from p in this.Context.ProjectEntities                
                orderby p.ProjectName
                select p;                         

            // *** Apply name filter
            if (!string.IsNullOrEmpty(companyName))
                projectList = projectList.Where(p => p.ProjectName.StartsWith(companyName));
            
            // *** Apply 'general' filter
            if (filter == ProjectQueryFilter.OpenProjects)
                projectList = projectList.Where(p => p.Status < 2)
                    // *** Order by last entered project
                                         .OrderByDescending(p => this.Context.EntryEntities
                                                                      .Where(entry => entry.ProjectPk == p.Pk)
                                                                      .Max(entry => (DateTime?)entry.TimeIn)
                                                            );

            else if (filter == ProjectQueryFilter.RecentProjects)
                projectList = projectList
                    // *** Order by last entered project
                                .OrderByDescending(p => this.Context.EntryEntities
                                                            .Where(entry => entry.ProjectPk == p.Pk)
                                                            .Max(entry => (DateTime?)entry.TimeIn)
                                                  )
                                .Take(10);

            return projectList;
        }

        /// <summary>
        /// Check validation for Projectname and Customer association
        /// </summary>
        protected override void OnValidate(ProjectEntity entity)
        {
            if (string.IsNullOrEmpty(entity.ProjectName))
                this.ValidationErrors.Add("ProjectName can't be empty.", "txtProjectName");
            
            if (entity.CustomerPk < 0)
                this.ValidationErrors.Add("Please ensure a customer is associated with the project", "txtCustomerPk");
        }

    }

    public enum ProjectStatusTypes
    {
        Entered = 0,
        Started = 1,
        Completed = 2
    }
    public class ProjectListResult
    {
        public string ProjectName { get; set; }
        public int Pk {get; set;} 
    }

    public enum ProjectQueryFilter
    {
        OpenProjects,
        RecentProjects,
        AllProjects
    }
}
