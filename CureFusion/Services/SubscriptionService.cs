using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CureFusion.Entities;
using CureFusion.Persistence;
using Microsoft.Extensions.Configuration;

namespace CureFusion.Services
{

    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SubscriptionService(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Subscription> GetActiveSubscription(int patientId)
        {
            return await _context.subscriptions
                .Include(s => s.Plan)
                .Where(s => s.PatientId == patientId && s.Status == SubscriptionStatus.Active && s.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(s => s.ExpiryDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Subscription> Subscribe(int patientId, string planType, string paymentMethod, string transactionId)
        {
            
            var plan = await _context.subscriptionPlans
                .FirstOrDefaultAsync(p => p.Type == planType);

            if (plan == null)
            {
                throw new Exception($"Subscription plan '{planType}' not found");
            }

  
            var activeSubscription = await GetActiveSubscription(patientId);

            if (activeSubscription != null)
            {
                activeSubscription.Status = SubscriptionStatus.Cancelled;
                _context.subscriptions.Update(activeSubscription);

                // Implement refund logic if needed
                // ...
            }

           
            var subscription = new Subscription
            {
                PatientId = patientId,
                PlanId = plan.Id,
                StartDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(plan.DurationInDays),
                Status = SubscriptionStatus.Active,
                RemainingConsultations = plan.ConsultationsPerPeriod,
                PaymentMethod = paymentMethod,
                TransactionId = transactionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }

        public async Task<bool> CancelSubscription(int subscriptionId)
        {
            var subscription = await _context.subscriptions.FindAsync(subscriptionId);

            if (subscription == null)
            {
                return false;
            }

            subscription.Status = SubscriptionStatus.Cancelled;
            _context.subscriptions.Update(subscription);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsConsultationCovered(int patientId)
        {
            var activeSubscription = await GetActiveSubscription(patientId);

            if (activeSubscription == null)
            {
                return false;
            }

            return activeSubscription.RemainingConsultations > 0;
        }

        public async Task<int> GetRemainingConsultations(int patientId)
        {
            var activeSubscription = await GetActiveSubscription(patientId);

            if (activeSubscription == null)
            {
                return 0;
            }

            return activeSubscription.RemainingConsultations;
        }

        public async Task<bool> DeductConsultation(int patientId)
        {
            var activeSubscription = await GetActiveSubscription(patientId);

            if (activeSubscription == null || activeSubscription.RemainingConsultations <= 0)
            {
                return false;
            }

            activeSubscription.RemainingConsultations--;
            _context.subscriptions.Update(activeSubscription);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetAvailablePlans()
        {
            return await _context.subscriptionPlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.Price)
                .ToListAsync();
        }
    }

   
    

 
   
}
