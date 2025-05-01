namespace vegetarian.Commons
{
    public class AppEnums
    {
        public enum UserRole
        {
            Admin,
            User
        }

        public enum BlogStatus
        {
            Draft,
            Published,
            Archived,
            Done
        }

        public enum BlogType
        {
            Blog,
            Event,
            BuffetMenu,
        }

        public enum ProductType
        {
            Food,
            Drink,
            Buffet,
        }
         
        public enum OrderStatus
        {
            WaitForConfirm,
            Processing,
            Done,
        }

        public enum PaymentMethod
        {
            Cash,
            BankTransfer,
            Visa,
        }
    }
}