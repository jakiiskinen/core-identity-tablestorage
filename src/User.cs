using Microsoft.Azure.Cosmos.Table;
using System.Globalization;

namespace Core.Identity.TableStorage
{
    public class User : TableEntity, IUser
    {
        private string _id;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                RowKey = _id.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
