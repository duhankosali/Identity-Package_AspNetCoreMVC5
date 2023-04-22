using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.CustomDescriber
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            //return base.PasswordTooShort(length); // Ana durum

            // Override
            return new()
            {
                Code = "PasswordTooShort",
                Description = $"Parola en az {length} karakter olmalıdır."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            //return base.PasswordRequiresNonAlphanumeric();  // Ana durum

            return new()
            {
                Code = "PasswordRequiresNonAlphenumeric",
                Description = "Parola en az bir alfenümarik (!* vs.) karakter içermelidir."
            };
        }
    }
}
