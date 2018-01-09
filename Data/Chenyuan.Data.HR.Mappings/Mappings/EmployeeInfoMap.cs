using Chenyuan.Data.HR.Entity.Entities;
using Chenyuan.Date.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.HR.Mappings.Mappings
{
    public class EmployeeInfoMap : EntityBaseMap<EmployeeInfo>
    {
        /// <summary>
        /// 初始化实体属性
        /// </summary>
        protected override void InitProperties()
        {
            base.InitProperties();

            #region Property Mappings for class EmployeeInfo

            this.Property(x => x.CreatedById)
                .IsOptional();

            this.Property(x => x.JoinDate)
                .IsRequired();

            this.Property(x => x.AwayDate)
                .IsOptional();

            this.Property(x => x.LatestJoinDate)
                .IsRequired();

            this.Property(x => x.Birthday)
                .IsRequired();

            this.Property(x => x.EmployeeNo)
                //.HasMaxLength(50)
                .IsRequired();

            this.Property(x => x.ProbationEndDate)
                .IsRequired();

            this.Property(x => x.Salary)
                .IsOptional();

            #endregion
        }

        /// <summary>
        /// 初始化实体关系
        /// </summary>
        protected override void InitAssociations()
        {
            base.InitAssociations();

            #region Navigation Property Mappings for class EmployeeInfo

            //this.HasOptional(x => x.HighestEducationInfo)
            //    .WithMany()
            //    .HasForeignKey(x => x.HighestEducationId)
            //    .WillCascadeOnDelete(false);

            //this.HasOptional(x => x.LastAcademy)
            //    .WithMany()
            //    .HasForeignKey(x => x.LastAcademyId)
            //    .WillCascadeOnDelete(false);

            //this.HasOptional(x => x.ContactInfo)
            //    .WithMany()
            //    .HasForeignKey(x => x.ContactInfoId)
            //    .WillCascadeOnDelete(false);

            //this.HasOptional(x => x.Department)
            //    .WithMany(x => x.Employees)
            //    .HasForeignKey(x => x.DepartmentId)
            //    .WillCascadeOnDelete(false);

            #endregion
        }
    }
}
