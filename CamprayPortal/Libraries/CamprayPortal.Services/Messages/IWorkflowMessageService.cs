 
using CamprayPortal.Core.Domain.Customers;

using CamprayPortal.Core.Domain.Messages;
using CamprayPortal.Core.Domain.News;




namespace CamprayPortal.Services.Messages
{
    public partial interface IWorkflowMessageService
    {
        #region Customer workflow

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerRegisteredNotificationMessage(Customer customer, int languageId);

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerWelcomeMessage(Customer customer, int languageId);

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerEmailValidationMessage(Customer customer, int languageId);

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCustomerPasswordRecoveryMessage(Customer customer, int languageId);
        
        #endregion
 

        #region Newsletter workflow

        /// <summary>
        /// Sends a newsletter subscription activation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendNewsLetterSubscriptionActivationMessage(NewsLetterSubscription subscription,
            int languageId);

        #endregion

        #region Send a message to a friend

       /// <summary>
       /// 
       /// </summary>
       /// <param name="customer"></param>
       /// <param name="languageId"></param>
       /// <param name="customerEmail"></param>
       /// <param name="friendsEmail"></param>
       /// <param name="personalMessage"></param>
       /// <returns></returns>
        int SendProductEmailAFriendMessage(Customer customer, int languageId,
            string customerEmail, string friendsEmail, string personalMessage);

        /// <summary>
        /// Sends wishlist "email a friend" message
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="customerEmail">Customer's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        int SendWishlistEmailAFriendMessage(Customer customer, int languageId,
             string customerEmail, string friendsEmail, string personalMessage);

        #endregion
        
     

       
        
        #region Misc
         


        /// <summary>
        /// Sends a "new VAT sumitted" notification to a store owner
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="vatName">Received VAT name</param>
        /// <param name="vatAddress">Received VAT address</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendNewVatSubmittedStoreOwnerNotification(Customer customer,
            string vatName, string vatAddress, int languageId);

       

        /// <summary>
        /// Sends a news comment notification message to a store owner
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendNewsCommentNotificationMessage(NewsComment newsComment, int languageId);

        

        #endregion
    }
}
