using System.Collections.Generic;

using CamprayPortal.Core.Domain.Catalog;
using CamprayPortal.Core.Domain.Customers;

using CamprayPortal.Core.Domain.Messages;
using CamprayPortal.Core.Domain.News;


using CamprayPortal.Core.Domain.Stores;

namespace CamprayPortal.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        void AddStoreTokens(IList<Token> tokens, Store store, EmailAccount emailAccount);

       

       

        void AddCustomerTokens(IList<Token> tokens, Customer customer);

        void AddNewsLetterSubscriptionTokens(IList<Token> tokens, NewsLetterSubscription subscription);

      
        void AddNewsCommentTokens(IList<Token> tokens, NewsComment newsComment);
         
         

        string[] GetListOfCampaignAllowedTokens();

        string[] GetListOfAllowedTokens();
    }
}
