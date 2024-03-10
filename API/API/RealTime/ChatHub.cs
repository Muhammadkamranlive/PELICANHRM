using Server.Models;
using Server.Domain;
using Server.Services;
using System.Collections;
using Microsoft.AspNetCore.SignalR;

namespace API.RealTime
{
    public class ChatHub : Hub
    {
        private readonly INotifications_Service _notifications_Service;
        private readonly ICaseComments_Service _caseComments_Service;
        private readonly ICaseManagment_Service caseManageService;
        private IAuthManager _authManager;
        public ChatHub
        (
            INotifications_Service notifications_Service,
            ICaseComments_Service caseComments_Service,
            ICaseManagment_Service service,
            IAuthManager authManager
        )
        {
            _notifications_Service = notifications_Service;
            _caseComments_Service = caseComments_Service;
            caseManageService = service;
            _authManager = authManager;
        }

        public async Task SendNotification(string userId,Guid caseId, string message)
        {
            AllUsersModel user =await _authManager.GetByIduser(userId);
            // Save the new notification to the database
            var newNotification = new CaseComment
            {
                Id        = Guid.NewGuid(),
                CaseId    = caseId,
                UserId    = userId,
                CreatedAt = DateTime.UtcNow,
                Text      = message
              
            };

            await _caseComments_Service.InsertAsync(newNotification);
            await _caseComments_Service.CompleteAync();

            var allNotifications = new ChatModel
            {
                CreatedAt= newNotification.CreatedAt,
                Text=newNotification.Text,
                Email=user.Email,
                Id=newNotification.Id,
                Image=user.Image,
                Name=user.FirstName+" "+user.LastName,
                Roles =user.Roles
            };
            await Clients.All.SendAsync("ReceiveNotification", allNotifications);
        }


        private  async Task<IList> GetCaseComments(Guid id)
        {

            try
            {
                var comments = await _caseComments_Service.GetAll();
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                

                var list = comments.Join
                    (
                      users,
                      (co) => new { co.UserId },
                      (u) => new { UserId = u.Id },
                      (co, u) => new { co, u }
                    )
                   .Where(x=>x.co.CaseId== id)
                    .Select(x => new 
                    {
                        x.co.Id,
                        x.co.Text,
                        x.co.CreatedAt,
                        name = x.u.FirstName + " " + x.u.LastName,
                        x.u.Email,
                        x.u.Roles,
                        x.u.Image,

                    })
                    .OrderBy(x => x.CreatedAt)
                    .ToList();

               
                
                return list;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }

    }
}
