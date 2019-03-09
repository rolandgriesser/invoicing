using System;
using System.Collections.Generic;
using System.Linq;
using invoicing.server.Data.Repository;
using invoicing.server.Models;

namespace invoicing.server.Data.DataManager {
    public class OrganizationManager : IDataRepository<Organization> {
        readonly InvoicingDbContext _dbContext;
 
        public OrganizationManager(InvoicingDbContext context)
        {
            _dbContext = context;
        }
 
        public IEnumerable<Organization> GetAll()
        {
            return _dbContext.Organizations.ToList();
        }
 
        public Organization Get(Guid id)
        {
            return _dbContext.Organizations
                  .FirstOrDefault(e => e.Id == id);
        }
 
        public void Add(Organization entity)
        {
            _dbContext.Organizations.Add(entity);
            _dbContext.SaveChanges();
        }
 
        public void Update(Organization employee, Organization entity)
        {
            // employee.FirstName = entity.FirstName;
            // employee.LastName = entity.LastName;
            // employee.Email = entity.Email;
            // employee.DateOfBirth = entity.DateOfBirth;
            // employee.PhoneNumber = entity.PhoneNumber;
 
            _dbContext.SaveChanges();
        }
 
        public void Delete(Organization entity)
        {
            _dbContext.Organizations.Remove(entity);
            _dbContext.SaveChanges();
        }

    }
}