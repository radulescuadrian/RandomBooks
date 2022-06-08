namespace RandomBooks.Website.Logic.Services.CustomerService
{
    public interface ICustomerService
    {
        CustomerDetails User { get; set; }
        CustomerDetails TempUser { get; set; }
        string TempImage { get; set; }
        string Image { get; set; }

        event Action OnChange;

        Task<string> AddAddress(Address address);
        Task<string> AddCard(CustomerCard card);
        Address CreateNewAddress();
        CustomerCard CreateNewCard();
        Task<string> DeleteAddress(int addressId);
        Task<string> DeleteCard(int cardId);
        Task GetProfile();
        Task<string> UpdateAddress(Address address);
        Task<string> UpdateCard(CustomerCard card);
        Task<string> UpdateProfile(CustomerDetails user, string image);
    }
}