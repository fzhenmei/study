using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Westwind.BusinessFramework;
using TimeTrakker;
using Westwind.Utilities;

namespace TimeTrakkerWeb
{
    public partial class ProjectList : System.Web.UI.Page
    {
        protected busProject Project = new busProject();

        protected void Page_Load(object sender, EventArgs e)
        {
             

#if (true)
            // *** Retrieve a filtered list from the business object
            ProjectQueryFilter filter = (ProjectQueryFilter) Enum.Parse(typeof(ProjectQueryFilter),this.lstFilter.SelectedValue);
            IQueryable<ProjectEntity> projectList = Project.GetFilteredProjects(filter,this.txtNameFilter.Text);

#else       // inline query filtering
            // *** Get the cor list
            IQueryable<ProjectEntity> projectList = Project.GetAllProjects();            

                // *** The following is merely applying filters 
            // *** If you want to be PURE about this you can abstract
            // *** these into the bus object, but the flexibility offered
            // *** here by LINQ for filtering after getting core data is highly useful

            // *** Apply name filter
            if (!string.IsNullOrEmpty(this.txtNameFilter.Text))
                projectList = projectList.Where(p => p.ProjectName.StartsWith(this.txtNameFilter.Text));

            // *** Apply 'general' filter
            if (this.lstFilter.SelectedValue == "OpenProjects")
                projectList = projectList.Where(p => p.Status < 2)
                                         // *** Order by last entered project
                                         .OrderByDescending( p=> this.Project.ReadOnlyContext.EntryEntities
                                                                      .Where(entry => entry.ProjectPk == p.Pk)
                                                                      .Max(entry => (DateTime?)entry.TimeIn) 
                                                            );                

            else if (this.lstFilter.SelectedValue == "RecentProjects")
                projectList = projectList
                                // *** Order by last entered project
                                .OrderByDescending(p => this.Project.ReadOnlyContext.EntryEntities
                                                            .Where(entry => entry.ProjectPk == p.Pk)
                                                            .Max(entry => (DateTime?)entry.TimeIn)
                                                  )
                                .Take(15);            
#endif

                //// *** Create a new filtered query with only needed properties            
            var projList = projectList.Select(p => new
                                                    {
                                                        p.ProjectName,
                                                        p.Customer.Company,
                                                        p.Status,
                                                        p.Entered,
                                                        p.Pk
                                                    });

            //this.ErrorDisplay.ShowMessage(projList.ToString());

            var pList = projList.ToList();
            this.lblStatus.Text = pList.Count.ToString() + " projects";

            this.dgProjects.DataSource = pList;
            this.dgProjects.DataBind();
        }
    }
}
