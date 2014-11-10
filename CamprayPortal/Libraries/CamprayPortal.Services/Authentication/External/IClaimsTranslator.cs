//Contributor:  Nicholas Mayne


namespace CamprayPortal.Services.Authentication.External
{
    public partial interface IClaimsTranslator<T>
    {
        UserClaims Translate(T response);
    }
}